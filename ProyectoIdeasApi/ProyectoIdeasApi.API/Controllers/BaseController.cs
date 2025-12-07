using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdeasApi.SERVICES;
using LogLevel = ProyectoIdeasApi.MODEL.Enum.LogEnums.LogLevel;

namespace ProyectoIdeasApi.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        // Ejemplo de helper que podrías usar en otros controllers
        protected Guid GetMiembroId()
        {
            var claim = User.FindFirst("miembroId");
            return claim is null ? Guid.Empty : Guid.Parse(claim.Value);
        }
    }
}
