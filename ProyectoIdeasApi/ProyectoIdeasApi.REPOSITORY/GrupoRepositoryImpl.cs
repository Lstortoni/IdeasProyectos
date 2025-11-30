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
    public class GrupoRepositoryImpl : IGrupoRepository
    {
        private readonly AppDbContext _context;

        public GrupoRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }
       async Task IGrupoRepository.AddAsync(Grupo grupo, CancellationToken ct)
        {
            await _context.Grupos.AddAsync(grupo, ct);
        }

        async Task<List<Grupo>> IGrupoRepository.GetAllAsync(CancellationToken ct)
        {
            return await _context.Grupos
                .Include(g => g.Miembros)
                .ThenInclude(mg => mg.Miembro)
                .ToListAsync(ct);
        }

       async Task<Grupo?> IGrupoRepository.GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Grupos
               .Include(g => g.Miembros)
               .ThenInclude(mg => mg.Miembro)
               .Include(g => g.IdeaConcreta)
               .FirstOrDefaultAsync(g => g.Id == id, ct);
        }

       async Task<List<Grupo>> IGrupoRepository.GetByIdeaAsync(Guid ideaId, CancellationToken ct)
        {
            return await _context.Grupos
                .Where(g => g.IdeaConcretaId == ideaId)
                .Include(g => g.Miembros)
                .ThenInclude(mg => mg.Miembro)
                .ToListAsync(ct);
        }

        async Task<List<Grupo>> IGrupoRepository.GetByMiembroAsync(Guid miembroId, CancellationToken ct)
        {
            return await _context.Grupos
              .Where(g => g.Miembros.Any(mg => mg.MiembroId == miembroId))
              .Include(g => g.Miembros)
              .ThenInclude(mg => mg.Miembro)
              .ToListAsync(ct);
        }

        Task IGrupoRepository.SaveChangesAsync(CancellationToken ct)
        {
            return _context.SaveChangesAsync(ct);
        }
    }
}
