using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.ErrorHandlingMiddleware
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? Content { get; }

        public ApiException(string message, HttpStatusCode statusCode, string? content = null, Exception? inner = null)
            : base(message, inner)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
    public static class ApiErrorCatalog
    {
        #region Titles & Messages

        public static string GetTitleForStatusCode(int statusCode) => statusCode switch
        {
            // 1xx
            100 => "Continuar",
            101 => "Cambio de protocolo",
            102 => "Procesando",

            // 2xx
            200 => "Solicitud exitosa",
            201 => "Recurso creado",
            202 => "Aceptado para procesamiento",
            204 => "Sin contenido",
            206 => "Contenido parcial",

            // 3xx
            300 => "Múltiples opciones",
            301 => "Recurso movido permanentemente",
            302 => "Recurso encontrado",
            303 => "Ver otro recurso",
            304 => "No modificado",
            307 => "Redirección temporal",
            308 => "Redirección permanente",

            // 4xx
            400 => "Solicitud incorrecta",
            401 => "No autorizado",
            402 => "Pago requerido",
            403 => "Acceso prohibido",
            404 => "Recurso no encontrado",
            405 => "Método no permitido",
            406 => "Formato no aceptable",
            408 => "Tiempo de espera agotado",
            409 => "Conflicto de datos",
            410 => "Recurso eliminado",
            411 => "Longitud requerida",
            412 => "Precondición fallida",
            413 => "Entidad demasiado grande",
            414 => "URI demasiado larga",
            415 => "Tipo de contenido no soportado",
            416 => "Rango no satisfactorio",
            417 => "Expectativa fallida",
            422 => "Entidad no procesable",
            426 => "Se requiere actualización de protocolo",
            428 => "Precondición requerida",
            429 => "Límite de peticiones alcanzado",
            431 => "Encabezados de solicitud demasiado grandes",
            451 => "No disponible por razones legales",

            // 5xx
            500 => "Error interno del servidor",
            501 => "No implementado",
            502 => "Puerta de enlace inválida",
            503 => "Servicio no disponible",
            504 => "Tiempo de espera de la puerta de enlace",
            505 => "Versión HTTP no soportada",
            507 => "Almacenamiento insuficiente",
            508 => "Bucle detectado",
            510 => "No extendido",
            511 => "Autenticación de red requerida",

            _ => "Error inesperado"
        };

        public static string GetMessageForStatusCode(int statusCode) => statusCode switch
        {
            // 1xx
            100 => "El cliente debe continuar con la solicitud.",
            101 => "El servidor acepta cambiar el protocolo.",
            102 => "El servidor está procesando la solicitud, pero no hay respuesta todavía.",

            // 2xx
            200 => "La operación se completó correctamente.",
            201 => "El recurso fue creado exitosamente.",
            202 => "La solicitud fue aceptada y está en proceso.",
            204 => "No hay contenido que devolver, pero la solicitud fue exitosa.",
            206 => "Se devuelve solo una parte del recurso.",

            // 3xx
            300 => "Hay varias opciones disponibles para el recurso.",
            301 => "El recurso fue movido de forma permanente.",
            302 => "El recurso fue encontrado en otra ubicación.",
            303 => "Consulta otro recurso para continuar.",
            304 => "El recurso no ha cambiado desde la última vez.",
            307 => "La solicitud debe repetirse en otra ubicación (temporal).",
            308 => "El recurso ha sido redirigido permanentemente.",

            // 4xx
            400 => "Los datos enviados no cumplen con los requisitos esperados.",
            401 => "El token de autenticación no es válido, ha expirado o ya no tiene vigencia.",
            402 => "Se requiere un pago para acceder a este recurso.",
            403 => "No tienes los permisos necesarios.",
            404 => "El recurso solicitado no existe.",
            405 => "El método HTTP utilizado no está permitido para este recurso.",
            406 => "No se puede proporcionar una respuesta en el formato solicitado.",
            408 => "El servidor tardó demasiado en responder.",
            409 => "Ya existe un recurso con los mismos datos o hay conflicto de estado.",
            410 => "El recurso existió pero ya no está disponible.",
            411 => "La longitud del contenido es obligatoria.",
            412 => "Una condición previa no se cumplió.",
            413 => "El contenido enviado excede el límite permitido.",
            414 => "La URI solicitada es demasiado larga.",
            415 => "El tipo de contenido enviado no está soportado.",
            416 => "El rango solicitado no se puede cumplir.",
            417 => "El servidor no pudo cumplir con lo esperado.",
            422 => "Los datos son válidos pero no se pueden procesar.",
            426 => "Es necesario actualizar el protocolo de comunicación.",
            428 => "El servidor requiere una condición previa para esta solicitud.",
            429 => "Demasiadas peticiones. Intenta más tarde o respeta los headers de retry-after.",
            431 => "Los encabezados enviados son demasiado grandes.",
            451 => "El recurso está bloqueado por razones legales.",

            // 5xx
            500 => "Ha ocurrido un error interno inesperado.",
            501 => "El servidor no implementa esta funcionalidad.",
            502 => "El servidor actuó como proxy y recibió una respuesta inválida.",
            503 => "El servicio no está disponible actualmente. Intenta más tarde.",
            504 => "El servidor proxy no recibió respuesta a tiempo.",
            505 => "La versión de HTTP utilizada no está soportada.",
            507 => "El servidor no tiene suficiente espacio para completar la operación.",
            508 => "Se detectó un bucle infinito en el procesamiento.",
            510 => "El recurso requiere extensiones adicionales no soportadas.",
            511 => "Se requiere autenticación de red para acceder a internet.",

            _ => "Ocurrió un error desconocido."
        };

        #endregion

        #region Exception <-> StatusCode mapping

        /// <summary>
        /// Mapea una excepción a un código HTTP que sea representativo.
        /// </summary>
        public static int MapExceptionToStatusCode(Exception ex)
        {
            // ApiException ya contiene el status real
            if (ex is ApiException apiEx)
                return (int)apiEx.StatusCode;

            return ex switch
            {
                // HttpRequestException puede traer StatusCode en versiones recientes de .NET
                HttpRequestException httpEx when GetStatusCodeFromHttpRequestException(httpEx) is HttpStatusCode sc => (int)sc,

                // Problemas de autorización / seguridad
                UnauthorizedAccessException => 401,
                SecurityException => 403, // si importás System.Security

                // Not found / state issues
                KeyNotFoundException => 404,
                ArgumentNullException or ArgumentException => 400,
                ValidationException => 400,

                // Conflictos / malas operaciones
                InvalidOperationException => 409,
                NotSupportedException => 415,
                FormatException => 422,

                // Timeouts / cancellations
                TimeoutException => 408,
                TaskCanceledException => 408,
                OperationCanceledException => 499, // 499 Client Closed Request (NGINX style) or map to 408 if prefieres
                                                   // elegimos 499 para distinguir cancelaciones deliberadas

                // Polly circuit
                BrokenCircuitException => 503,

                // Not implemented
                NotImplementedException => 501,

                // Common fallback
                _ => 500
            };
        }

        private static HttpStatusCode? GetStatusCodeFromHttpRequestException(HttpRequestException ex)
        {
            // Desde .NET 5+ HttpRequestException tiene StatusCode? en algunos runtimes.
            // Si no la tiene, intenta parsear del mensaje (fallback).
#if NET5_0_OR_GREATER
            if (ex.StatusCode.HasValue)
                return ex.StatusCode.Value;
#endif
            // Fallback: si el InnerException es WebException se podría obtener el status, o parsear el mensaje.
            return null;
        }

        #endregion

        #region Throw exception from status code

        /// <summary>
        /// Lanza una excepción representativa para un código de estado HTTP.
        /// - Usa excepciones estándar cuando exista una buena correspondencia.
        /// - En otros casos lanza ApiException con el status y (opcional) contenido.
        /// </summary>
        /// <param name="statusCode">Código HTTP</param>
        /// <param name="content">Opcional: contenido o mensaje devuelto por el servicio</param>
        public static void ThrowExceptionForStatusCode(int statusCode, string? content = null)
        {
            // Preferimos usar HttpStatusCode enum cuando sea posible
            var sc = Enum.IsDefined(typeof(HttpStatusCode), statusCode)
                ? (HttpStatusCode)statusCode
                : HttpStatusCode.InternalServerError;

            var message = $"{(int)sc} {sc}: {GetTitleForStatusCode((int)sc)}. {GetMessageForStatusCode((int)sc)}";
            if (!string.IsNullOrWhiteSpace(content))
                message += $" Detalle: {content}";

            switch (statusCode)
            {
                // 4xx
                case 400:
                    throw new ValidationException(message);
                case 401:
                    throw new UnauthorizedAccessException(message);
                case 402:
                    throw new ApiException(message, sc, content);
                case 403:
                    throw new UnauthorizedAccessException(message); // o crear ForbiddenException si preferís
                case 404:
                    throw new KeyNotFoundException(message);
                case 405:
                    throw new NotSupportedException(message);
                case 408:
                    throw new TimeoutException(message);
                case 409:
                    throw new InvalidOperationException(message);
                case 410:
                    throw new InvalidOperationException(message);
                case 411:
                    throw new ArgumentException("Longitud requerida. " + message);
                case 412:
                    throw new ApiException(message, sc, content);
                case 413:
                    throw new InvalidOperationException("Entidad demasiado grande. " + message);
                case 415:
                    throw new NotSupportedException(message);
                case 422:
                    throw new FormatException(message);
                case 426:
                    throw new ApiException(message, sc, content);
                case 428:
                    throw new ApiException(message, sc, content);
                case 429:
                    // no hay excepción estándar; usar ApiException para poder contener RetryAfter si lo necesitás
                    throw new ApiException(message, sc, content);

                // 5xx
                case 500:
                    throw new Exception(message);
                case 501:
                    throw new NotImplementedException(message);
                case 502:
                    throw new ApiException(message, sc, content);
                case 503:
                    throw new ApiException(message, sc, content);
                case 504:
                    throw new TimeoutException(message);
                case 505:
                    throw new NotSupportedException(message);
                case 507:
                case 508:
                case 510:
                case 511:
                    throw new ApiException(message, sc, content);

                default:
                    // Para cualquier código no mapeado, lanzo ApiException (contiene el status).
                    throw new ApiException(message, sc, content);
            }
        }

        #endregion
    }
}
