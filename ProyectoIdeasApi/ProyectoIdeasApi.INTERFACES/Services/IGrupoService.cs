using ProyectoIdeasApi.CONTRACT.Dto.Grupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INTERFACES.Services
{
    public interface IGrupoService
    {
        Task<GrupoDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<List<GrupoDto>> GetAllAsync(CancellationToken ct = default);

        Task<List<GrupoDto>> GetByMiembroAsync(Guid miembroId, CancellationToken ct = default);

        Task<List<GrupoDto>> GetByIdeaAsync(Guid ideaId, CancellationToken ct = default);
    }
}
