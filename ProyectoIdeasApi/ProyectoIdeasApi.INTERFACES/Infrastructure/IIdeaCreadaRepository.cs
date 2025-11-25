using ProyectoIdeasApi.CONTRACT.Dto.Ideas;
using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INTERFACES.Infrastructure
{
    public interface IIdeaCreadaRepository
    {
        Task AddAsync(IdeaConcreta idea, CancellationToken ct = default);

        Task<IdeaConcreta?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<List<IdeaConcreta>> GetIdeasAbiertasAsync(CancellationToken ct = default);

        Task<List<IdeaConcreta>> GetIdeasCreadasPorAsync(Guid miembroId, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
