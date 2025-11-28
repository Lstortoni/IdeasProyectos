using ProyectoIdeasApi.CONTRACT.Dto.Miembro;
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
    public class MiembroServiceImpl : IMiembroService
    {
        private readonly IMiembroRepository _miembroRepo;

        public MiembroServiceImpl(IMiembroRepository miembroRepo)
        {
            _miembroRepo = miembroRepo;
        }

        async Task<MiembroDto?> IMiembroService.GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _miembroRepo.GetByIdAsync(id, ct);
            if (entity is null) return null;

            return MapToDto(entity);
        }
        public async Task<List<MiembroDto>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _miembroRepo.GetAllAsync(ct);
            return entities.Select(MapToDto).ToList();
        }


       async Task IMiembroService.UpdateAsync(MiembroDto miembroDto, CancellationToken ct)
        {
            if (miembroDto == null)
                throw new ArgumentNullException(nameof(miembroDto));

            // 1) Buscar el miembro existente
            var entity = await _miembroRepo.GetByIdAsync(miembroDto.Id, ct);
            if (entity is null)
                throw new InvalidOperationException("El miembro no existe.");

            // 2) Actualizar los campos que sí se pueden editar
            entity.Nombre = miembroDto.Nombre;
            entity.Apellido = miembroDto.Apellido;
            entity.Telefono = miembroDto.Telefono;
            entity.Email = miembroDto.Email;
            entity.AutoDescripcion = miembroDto.AutoDescripcion;

            // Si más adelante el DTO tiene cosas como habilidades, etc.,
            // eso lo manejamos aparte, no en este primer update simple.

            await _miembroRepo.UpdateAsync(entity, ct);
            await _miembroRepo.SaveChangesAsync(ct);
        }

        public static MiembroDto MapToDto(Miembro miembro)
        {
            return new MiembroDto
            {
                Id = miembro.Id,
                Nombre = miembro.Nombre,
                Apellido = miembro.Apellido,
                Telefono = miembro.Telefono,
                Email = miembro.Email,
                AutoDescripcion = miembro.AutoDescripcion
                // Si el DTO tiene más campos, los agregás acá
            };
        }


        


    }
}
