using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Abstractions;
using ProyectoIdeasApi.INFRASTRUCTURE.Data;
using ProyectoIdeasApi.INTERFACES.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INFRASTRUCTURE
{
    public class LogRepositoryImpl : ILogRepository
    {
        private readonly AppDbContext _context;   // usa el nombre de tu DbContext

        public LogRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        async public Task AddAsync(MODEL.LogEntry  entry, CancellationToken ct = default)
        {
            await _context.LogEntries.AddAsync(entry, ct);
            await _context.SaveChangesAsync(ct);
        }
    }
}
