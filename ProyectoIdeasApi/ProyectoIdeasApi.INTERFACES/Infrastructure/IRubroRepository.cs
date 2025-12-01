using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INTERFACES.Infrastructure
{
    public interface IRubroRepository
    {
        Task<List<Rubro>> GetAllAsync(CancellationToken ct = default);
        Task<Rubro?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Rubro rubro, CancellationToken ct = default);
        Task UpdateAsync(Rubro rubro, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
