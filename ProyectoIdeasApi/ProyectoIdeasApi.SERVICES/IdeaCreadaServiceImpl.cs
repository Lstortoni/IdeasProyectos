using ProyectoIdeasApi.CONTRACT.Dto.Ideas;
using ProyectoIdeasApi.INTERFACES.Infrastructure;
using ProyectoIdeasApi.INTERFACES.Services;
using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.SERVICES
{
    public class IdeaCreadaServiceImpl : IIdeaCreadaService
    {
        private readonly IIdeaCreadaRepository _ideaRepo;
        private readonly IMiembroRepository _miembroRepo;
        private readonly IGrupoRepository _grupoRepo; // lo vamos a usar sobre todo en CrearGrupoDesdeIdeaAsync

        public IdeaCreadaServiceImpl(
            IIdeaCreadaRepository ideaRepo,
            IMiembroRepository miembroRepo,
            IGrupoRepository grupoRepo)
        {
            _ideaRepo = ideaRepo;
            _miembroRepo = miembroRepo;
            _grupoRepo = grupoRepo;
        }
        Task<Grupo> IIdeaCreadaService.CrearGrupoDesdeIdeaAsync(Guid ideaId, List<Guid> miembrosSeleccionados)
        {
            throw new NotImplementedException();
        }

        Task<IdeaConcretaDto> IIdeaCreadaService.CrearIdeaAsync(Guid miembroId, CrearIdeaRequestDto dto)
        {
            throw new NotImplementedException();
        }

        Task IIdeaCreadaService.InteresarseAsync(Guid miembroId, Guid ideaId)
        {
            throw new NotImplementedException();
        }

        Task<List<IdeaConcretaDto>> IIdeaCreadaService.ListarIdeasAbiertasAsync()
        {
            throw new NotImplementedException();
        }

        Task<List<IdeaConcretaDto>> IIdeaCreadaService.ListarIdeasCreadasPorAsync(Guid miembroId)
        {
            throw new NotImplementedException();
        }
    }
}
