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
    public class RubroRepositoryImpl: IRubroRepository
    {
        private readonly AppDbContext _db;

        public RubroRepositoryImpl(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Rubro>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Rubros.ToListAsync(ct);
        }

        public async Task<Rubro?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Rubros.FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task AddAsync(Rubro rubro, CancellationToken ct = default)
        {
            await _db.Rubros.AddAsync(rubro, ct);
        }

        public Task UpdateAsync(Rubro rubro, CancellationToken ct = default)
        {
            _db.Rubros.Update(rubro);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}
