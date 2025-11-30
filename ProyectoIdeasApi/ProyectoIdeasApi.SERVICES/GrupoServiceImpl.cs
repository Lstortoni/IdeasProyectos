using ProyectoIdeasApi.CONTRACT.Dto.Grupo;
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
    public class GrupoServiceImpl : IGrupoService
    {
        private readonly IGrupoRepository _grupoRepo;

        public GrupoServiceImpl(IGrupoRepository grupoRepo)
        {
            _grupoRepo = grupoRepo;
        }


        public async Task<List<GrupoDto>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _grupoRepo.GetAllAsync(ct);
            return entities.Select(MapToDto).ToList();
        }

        public async Task<GrupoDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _grupoRepo.GetByIdAsync(id, ct);
            return entity is null ? null : MapToDto(entity);
        }


     
        public async Task<List<GrupoDto>> GetByIdeaAsync(Guid ideaId, CancellationToken ct = default)
        {
            var entities = await _grupoRepo.GetByIdeaAsync(ideaId, ct);
            return entities.Select(MapToDto).ToList();
        }


        public async Task<List<GrupoDto>> GetByMiembroAsync(Guid miembroId, CancellationToken ct = default)
        {
            var entities = await _grupoRepo.GetByMiembroAsync(miembroId, ct);
            return entities.Select(MapToDto).ToList();
        }


        private static GrupoDto MapToDto(Grupo g)
        {
            return new GrupoDto
            {
                Id = g.Id,
                Nombre = g.Nombre,
                FechaCreacion = g.FechaCreacion,
                IdeaConcretaId = g.IdeaConcretaId
            };
        }
    }
}
