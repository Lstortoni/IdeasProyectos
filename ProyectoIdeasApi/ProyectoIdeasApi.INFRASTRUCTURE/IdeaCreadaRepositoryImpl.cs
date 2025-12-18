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
    public class IdeaCreadaRepositoryImpl : IIdeaCreadaRepository
    {
        private readonly AppDbContext _context;

        public IdeaCreadaRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(IdeaConcreta idea, CancellationToken ct = default)
        {
            await _context.IdeasConcretas.AddAsync(idea, ct);
        }

        public async Task<IdeaConcreta?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            // Incluí lo que te haga falta (Creador, Interesados, Rubro, etc.)
            return await _context.IdeasConcretas
                .Include(i => i.Creador)
                .FirstOrDefaultAsync(i => i.Id == id, ct);
        }

        public async Task<List<IdeaConcreta>> GetIdeasAbiertasAsync(CancellationToken ct = default)
        {
            // Ajustá "EstaAbierta" al nombre real de tu propiedad
            return await _context.IdeasConcretas
                .Where(i => i.EstaAbierta())
                .ToListAsync(ct);
        }

        public async Task<List<IdeaConcreta>> GetIdeasCreadasPorAsync(Guid miembroId, CancellationToken ct = default)
        {
            return await _context.IdeasConcretas
                .Where(i => i.CreadorId == miembroId)
                .ToListAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            return _context.SaveChangesAsync(ct);
        }
    }
}
