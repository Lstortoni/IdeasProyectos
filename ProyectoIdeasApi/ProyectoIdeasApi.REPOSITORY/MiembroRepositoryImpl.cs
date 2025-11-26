using Microsoft.EntityFrameworkCore;
using ProyectoIdeasApi.INFRASTRUCTURE.Data;
using ProyectoIdeasApi.INTERFACES.Infrastructure;
using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INFRASTRUCTURE
{
    public class MiembroRepositoryImpl: IMiembroRepository
    {
        private readonly AppDbContext _db;

        public MiembroRepositoryImpl(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Miembro?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            // Podés agregar Include de Habilidades, Intimos, etc. cuando lo necesites
            return await _db.Miembros
                .FirstOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task<List<Miembro>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var listIds = ids.ToList();
            return await _db.Miembros
                .Where(m => listIds.Contains(m.Id))
                .ToListAsync(ct);
        }

        public async Task<List<Miembro>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Miembros.ToListAsync(ct);
        }

        public Task UpdateAsync(Miembro miembro, CancellationToken ct = default)
        {
            _db.Miembros.Update(miembro);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}
