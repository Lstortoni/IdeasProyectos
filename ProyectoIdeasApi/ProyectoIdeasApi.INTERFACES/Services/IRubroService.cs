using ProyectoIdeasApi.CONTRACT.Dto.Grupo;
using ProyectoIdeasApi.CONTRACT.Dto.Rubro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INTERFACES.Services
{
    public interface IRubroService
    {
        Task<RubroDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<List<RubroDto>> GetAllAsync(CancellationToken ct = default);

        Task<List<RubroDto>> AddRubro(RubroDto rubroDto, CancellationToken ct = default);

        Task<List<RubroDto>> updateRubro(RubroDto oldRubro, CancellationToken ct = default);
    }
}
