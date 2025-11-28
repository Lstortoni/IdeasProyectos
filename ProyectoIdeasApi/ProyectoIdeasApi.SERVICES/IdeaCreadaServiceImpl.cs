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

        async Task IIdeaCreadaService.InteresarseAsync(Guid miembroId, Guid ideaId)
        {
            // 1) Validar miembro
            var miembro = await _miembroRepo.GetByIdAsync(miembroId)
                           ?? throw new InvalidOperationException("El miembro no existe.");

            // 2) Validar idea
            var idea = await _ideaRepo.GetByIdAsync(ideaId)
                       ?? throw new InvalidOperationException("La idea no existe.");

            // 3) Validar idea abierta
            if (!idea.EstaAbierta())
                throw new InvalidOperationException("La idea ya no está abierta a nuevos interesados.");

            // 4) Evitar duplicados
            var yaInteresado = idea.Interesados.Any(i => i.MiembroId == miembroId);
            if (yaInteresado)
                return; // o lanzar error, según tu regla de negocio

            // 5) Agregar interesado
            idea.Interesados.Add(new Interesado
            {
                Id = Guid.NewGuid(),
                MiembroId = miembroId,
                Miembro = miembro,
                IdeaConcretaId = idea.Id,
                IdeaConcreta = idea
            });

            // 6) Guardar cambios
            await _ideaRepo.SaveChangesAsync();
        }

        async Task<List<IdeaConcretaDto>> IIdeaCreadaService.ListarIdeasAbiertasAsync()
        {
            var ideas = await _ideaRepo.GetIdeasAbiertasAsync();
            return ideas.Select(MapToDto).ToList();
        }

        async Task<List<IdeaConcretaDto>> IIdeaCreadaService.ListarIdeasCreadasPorAsync(Guid miembroId)
        {
            var ideas = await _ideaRepo.GetIdeasCreadasPorAsync(miembroId);
            return ideas.Select(MapToDto).ToList();
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
