using ProyectoIdeasApi.CONTRACT.Dto.Ideas;
using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INTERFACES.Services
{
    public interface IIdeaCreadaService
    {
        Task<IdeaConcretaDto> CrearIdeaAsync(Guid miembroId, CrearIdeaRequestDto dto);
        Task InteresarseAsync(Guid miembroId, Guid ideaId);
        Task<List<IdeaConcretaDto>> ListarIdeasAbiertasAsync();
        Task<List<IdeaConcretaDto>> ListarIdeasCreadasPorAsync(Guid miembroId);
        Task<Grupo> CrearGrupoDesdeIdeaAsync(Guid ideaId, List<Guid> miembrosSeleccionados);
    }
}
