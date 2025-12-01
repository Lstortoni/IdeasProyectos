using ProyectoIdeasApi.CONTRACT.Dto.Rubro;
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
    public class RubroServiceImpl : IRubroService
    {
        private readonly IRubroRepository _rubroRepo;

        public async Task<List<RubroDto>> AddRubro(RubroDto rubroDto, CancellationToken ct = default)
        {
            if (rubroDto == null)
                throw new ArgumentNullException(nameof(rubroDto));

            var entity = MapToEntity(rubroDto);
            entity.Id = Guid.NewGuid();

            await _rubroRepo.AddAsync(entity, ct);
            await _rubroRepo.SaveChangesAsync(ct);

            // Devolvemos la lista completa, según tu firma
            var all = await _rubroRepo.GetAllAsync(ct);
            return all.Select(MapToDto).ToList();
        }

        public async Task<List<RubroDto>> GetAllAsync(CancellationToken ct = default)
        {
            var rubros = await _rubroRepo.GetAllAsync(ct);
            return rubros.Select(MapToDto).ToList();
        }

        public async Task<RubroDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var rubro = await _rubroRepo.GetByIdAsync(id, ct);
            return rubro is null ? null : MapToDto(rubro);
        }

        public async Task<List<RubroDto>> updateRubro(RubroDto oldRubro, CancellationToken ct = default)
        {
            if (oldRubro == null)
                throw new ArgumentNullException(nameof(oldRubro));
            if (oldRubro.Id == Guid.Empty)
                throw new InvalidOperationException("El Rubro debe tener Id para actualizar.");

            var existing = await _rubroRepo.GetByIdAsync(oldRubro.Id, ct)
                           ?? throw new InvalidOperationException("El rubro no existe.");

            // Actualizamos campos
            existing.Nombre = oldRubro.Nombre;
            existing.Descripcion = oldRubro.Descripcion;

            await _rubroRepo.UpdateAsync(existing, ct);
            await _rubroRepo.SaveChangesAsync(ct);

            var all = await _rubroRepo.GetAllAsync(ct);
            return all.Select(MapToDto).ToList();
        }

        // ----------------- Mappers internos -----------------

        private static RubroDto MapToDto(Rubro r) =>
            new RubroDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion
            };

        private static Rubro MapToEntity(RubroDto dto) =>
            new Rubro
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };
    }
}
