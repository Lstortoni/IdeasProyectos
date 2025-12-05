using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdeasApi.SERVICES;
using LogLevel = ProyectoIdeasApi.MODEL.Enum.LogEnums.LogLevel;

namespace ProyectoIdeasApi.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogService _logService;

        protected BaseController(ILogService logService)
        {
            _logService = logService;
        }





        protected void HandleException(Exception ex)
          => _logService.LogMessage(LogLevel.ERROR, ex.Message, ex);
    }
}
