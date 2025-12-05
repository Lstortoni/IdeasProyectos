using ProyectoIdeasApi.SERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INFRASTRUCTURE
{
    public abstract class BaseInfrastructure
    {
        protected readonly ILogService _logService;
   

        public BaseInfrastructure(ILogService logService)
        {
        
            _logService = logService;
        }

        protected void HandleException(Exception ex)
        {
            throw ex;
        }
    }
}
