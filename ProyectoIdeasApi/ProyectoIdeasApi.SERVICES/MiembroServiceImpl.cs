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


       async Task IMiembroService.UpdateAsync(Guid id,UpdateMiembroDto miembroDto, CancellationToken ct)
        {
            if (miembroDto == null)
                throw new ArgumentNullException(nameof(miembroDto));

            // 1) Buscar el miembro existente
            var entity = await _miembroRepo.GetByIdAsync(id, ct);
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

        async Task IMiembroService.MarcarComoIntimoAsync(Guid miembroId, Guid intimoId,string? nota, CancellationToken ct)
        {
            if (miembroId == intimoId)
                throw new InvalidOperationException("No podés marcarte a vos mismo como íntimo.");

            // 1) Traer al miembro con sus íntimos
            var miembro = await _miembroRepo.GetByIdWithIntimosAsync(miembroId, ct)
                          ?? throw new InvalidOperationException("El miembro no existe.");

            // 2) Traer al miembro 'intimo'
            var intimo = await _miembroRepo.GetByIdAsync(intimoId, ct)
                         ?? throw new InvalidOperationException("El miembro íntimo no existe.");

            // 3) Evitar duplicados
            var yaEsIntimo = miembro.Intimos.Any(i => i.IntimoId == intimoId);
            if (yaEsIntimo)
                return; // o lanzar excepción si querés ser más estricto

            // 4) Crear relación
            var relacion = new MiembroIntimo
            {
                // Id se genera solo
                PropietarioId = miembro.Id,   // el "dueño" de la lista
                Propietario = miembro,
                IntimoId = intimo.Id,
                Intimo = intimo,
                Nota = nota,
                FechaAgregado = DateTime.UtcNow
            };

            miembro.Intimos.Add(relacion);

            // 5) Guardar cambios
            await _miembroRepo.SaveChangesAsync(ct);
        }

       async  Task IMiembroService.QuitarIntimoAsync(Guid miembroId, Guid intimoId, CancellationToken ct)
        {
            var miembro = await _miembroRepo.GetByIdWithIntimosAsync(miembroId, ct)
              ?? throw new InvalidOperationException("El miembro no existe.");

            var relacion = miembro.Intimos
                                  .FirstOrDefault(r => r.IntimoId == intimoId);

            if (relacion == null)
                throw new InvalidOperationException("Ese íntimo no está asociado al miembro.");

            // Quitar de la colección del miembro
            miembro.Intimos.Remove(relacion);

            // O eliminarlo directo desde el repo
            await _miembroRepo.RemoveIntimoAsync(relacion, ct);

            await _miembroRepo.SaveChangesAsync(ct);
        }

        async Task<List<MiembroDto>> IMiembroService.ListarIntimosAsync(Guid miembroId, CancellationToken ct)
        {
            var miembro = await _miembroRepo.GetByIdWithIntimosAsync(miembroId, ct)
                  ?? throw new InvalidOperationException("El miembro no existe.");

            // miembro.Intimos es List<MiembroIntimo>
            var lista = miembro.Intimos
                               .Select(rel => MapToDto(rel.Intimo))
                               .ToList();

            return lista;
        }
    }
}
