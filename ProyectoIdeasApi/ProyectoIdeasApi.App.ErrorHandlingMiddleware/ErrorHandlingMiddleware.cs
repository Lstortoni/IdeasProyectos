using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoIdeasApi.SERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LogLevel = ProyectoIdeasApi.MODEL.Enum.LogEnums.LogLevel;
namespace ProyectoIdeasApi.ErrorHandlingMiddleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
  

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            
        }

        public async Task Invoke(HttpContext context)
        {
            // Scoped por request (acá sí se puede)
            var _logService = context.RequestServices.GetRequiredService<ILogService>();

            var originalBody = context.Response.Body;
            using var responseBody = new MemoryStream();
            try
            {
                context.Response.Body = responseBody;
                await _next(context);

                if (IsCustomHandledStatusCode(context.Response.StatusCode))
                {
                    var responseBodyString = await GetResponseBody(responseBody);
                    if (string.IsNullOrEmpty(responseBodyString) && context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                    {
                        LogDecodedJwt(context);
                        context.Response.Body = originalBody;
                        context.Response.ContentType = "application/json";

                        var errorJson = new
                        {
                            title = ApiErrorCatalog.GetTitleForStatusCode(401),
                            status = 401,
                            error = ApiErrorCatalog.GetMessageForStatusCode(401),
                            traceId = context.TraceIdentifier
                        };

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorJson));
                        return;
                    }

                    var responseDto = RemoveFields(responseBodyString);
                    var updatedResponseDto = UpdateErrorTitle(responseDto, ApiErrorCatalog.GetTitleForStatusCode(context.Response.StatusCode));

                    context.Response.ContentType = "application/json";
                    context.Response.Body = originalBody;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(updatedResponseDto));
                }
                else
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBody);
                }
            }
            catch (Exception ex)
            {

                // 1) Log en logger estándar (consola, archivo, etc.)
                _logger.LogError(ex, "Unhandled exception");

                // 2) Log en base de datos con tu servicio
                await _logService.LogMessage(LogLevel.ERROR, ex.Message, ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ApiErrorCatalog.MapExceptionToStatusCode(ex);

                var errorResponse = new
                {
                    title = GetTitleForException(ex),
                    status = context.Response.StatusCode,
                    error = GetMessageForException(ex),
                    traceId = context.TraceIdentifier
                };

                context.Response.Body = originalBody;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }

        private static bool IsCustomHandledStatusCode(int statusCode) =>
            statusCode is (int)HttpStatusCode.BadRequest
                       or (int)HttpStatusCode.NotFound
                       or (int)HttpStatusCode.Unauthorized
                       or (int)HttpStatusCode.Forbidden
                       or (int)HttpStatusCode.TooManyRequests;

        private static async Task<string> GetResponseBody(Stream responseBody)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(responseBody, Encoding.UTF8).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private static object RemoveFields(string responseBody)
        {
            try
            {
                if (string.IsNullOrEmpty(responseBody)) return responseBody;
                var json = JObject.Parse(responseBody);
                json.Remove("type");
                return json;
            }
            catch
            {
                return responseBody;
            }
        }

        private static object UpdateErrorTitle(object responseDto, string errorTitle)
        {
            if (responseDto is JObject jsonObject)
            {
                jsonObject["title"] = errorTitle;
                return jsonObject;
            }

            return new { title = errorTitle };
        }



        private static string GetTitleForException(Exception ex)
        {
            int status = ApiErrorCatalog.MapExceptionToStatusCode(ex);
            return ApiErrorCatalog.GetTitleForStatusCode(status);
        }
        private static string GetMessageForException(Exception ex)
        {
            if (ex.InnerException is Npgsql.PostgresException pgEx)
            {
                // Podés inspeccionar pgEx.SqlState, pgEx.MessageText, etc.
                return pgEx.MessageText;   // mensaje crudo de PostgreSQL
            }

            // 2) Si querés leer errores de EF Core (por ejemplo validación de UPDATE/INSERT)
            if (ex is DbUpdateException dbEx && dbEx.InnerException is Npgsql.PostgresException innerPg)
            {
                return innerPg.MessageText;
            }

            // 3) Caso general: mensaje estándar según catálogo
            int status = ApiErrorCatalog.MapExceptionToStatusCode(ex);
            return ApiErrorCatalog.GetMessageForStatusCode(status);
        }
        private void LogDecodedJwt(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Bearer ")) return;

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var tokenParts = token.Split('.');
            if (tokenParts.Length < 2 || string.IsNullOrEmpty(tokenParts[1])) return;

            try
            {
                string base64 = tokenParts[1].PadRight(tokenParts[1].Length + (4 - tokenParts[1].Length % 4) % 4, '=');
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                _logger.LogInformation($"TOKEN decodificado: {decoded}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error al decodificar token: {e.Message}");
            }
        }
    }
}
