using ProyectoIdeasApi.CONTRACT.Dto.Ideas;
using ProyectoIdeasApi.INTERFACES.Infrastructure;
using ProyectoIdeasApi.INTERFACES.Services;
using ProyectoIdeasApi.MODEL;
using ProyectoIdeasApi.MODEL.Enum;
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
       async Task<Grupo> IIdeaCreadaService.CrearGrupoDesdeIdeaAsync(Guid ideaId, List<Guid> miembrosSeleccionados)
        {

            // 1) Obtener idea
            var idea = await _ideaRepo.GetByIdAsync(ideaId)
                       ?? throw new InvalidOperationException("La idea no existe.");

            // 2) Idea ya tiene un grupo?
            if (idea.Grupo != null && idea.Grupo.Id != Guid.Empty)
                throw new InvalidOperationException("La idea ya tiene un grupo creado.");

            // 3) Idea abierta?
            if (!idea.Activa)
                throw new InvalidOperationException("La idea no está activa o ya fue cerrada.");


            // 4) Validar que los miembros seleccionados SON interesados
            var interesadosIds = idea.Interesados.Select(i => i.MiembroId).ToHashSet();


            foreach (var id in miembrosSeleccionados)
            {
                if (!interesadosIds.Contains(id))
                    throw new InvalidOperationException($"El miembro {id} no es interesado de esta idea.");
            }

            // 5) Obtener miembros desde repo
            var miembros = await _miembroRepo.GetByIdsAsync(miembrosSeleccionados);

            if (miembros.Count != miembrosSeleccionados.Count)
                throw new InvalidOperationException("Alguno de los miembros seleccionados no existe.");

            // 6) Crear Grupo
            var grupo = new Grupo
            {
                Nombre = $"Grupo - {idea.Nombre}",
                FechaCreacion = DateTime.UtcNow,
                IdeaConcretaId = idea.Id,
                IdeaConcreta = idea
            };

            // 7) Creador también entra como líder del grupo
            grupo.Miembros.Add(new MiembroGrupo
            {
                MiembroId = idea.CreadorId,
                Rol = RolEnGrupo.Creador
            });

            // 8) Agregar participantes seleccionados
            foreach (var m in miembros)
            {
                grupo.Miembros.Add(new MiembroGrupo
                {
                    MiembroId = m.Id,
                    Rol = RolEnGrupo.Participante
                });
            }

            // 9) Asociar grupo a idea (en memoria)
            idea.Grupo = grupo;

            // 10) Guardar en DB
            await _grupoRepo.AddAsync(grupo);
            await _grupoRepo.SaveChangesAsync();


            return grupo;
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
