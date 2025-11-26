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

        async Task<IdeaConcretaDto> IIdeaCreadaService.CrearIdeaAsync(Guid miembroId, CrearIdeaRequestDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var creador = await _miembroRepo.GetByIdAsync(miembroId)
                           ?? throw new InvalidOperationException("El miembro creador no existe.");

            var idea = new IdeaConcreta
            {
                Nombre = dto.Nombre,
                Proposito = dto.Proposito,
                Color = dto.Color,
                FechaCreacion = DateTime.UtcNow,
                CreadorId = miembroId,
                Creador = creador,
                RubroId = dto.RubroId
            };

            await _ideaRepo.AddAsync(idea);
            await _ideaRepo.SaveChangesAsync();

            return MapToDto(idea);
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

        private static IdeaConcretaDto MapToDto(IdeaConcreta idea)
        {
            return new IdeaConcretaDto
            {
                Id = idea.Id,
                Nombre = idea.Nombre,
                Proposito = idea.Proposito,
                Color = idea.Color,
                FechaCreacion = idea.FechaCreacion,
                CreadorId = idea.CreadorId,
                RubroId = idea.RubroId
                // lo que tenga tu DTO
            };
        }
    }
}
