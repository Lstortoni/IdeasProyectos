using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INTERFACES.Infrastructure
{
    public interface IMiembroRepository
    {
        Task<Miembro?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<List<Miembro>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Miembro>> GetAllAsync(CancellationToken ct = default);

        // Por si en algún momento querés editar datos del miembro
        Task UpdateAsync(Miembro miembro, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}