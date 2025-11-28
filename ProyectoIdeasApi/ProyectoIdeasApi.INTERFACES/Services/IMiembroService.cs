using ProyectoIdeasApi.CONTRACT.Dto.Miembro;
using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INTERFACES.Services
{
    public interface IMiembroService
    {
  
        Task<MiembroDto?> GetByIdAsync(Guid id, CancellationToken ct);

        Task<List<MiembroDto>> GetAllAsync(CancellationToken ct = default);

        // Por si en algún momento querés editar datos del miembro
        Task UpdateAsync(MiembroDto miembro, CancellationToken ct = default);
    }
}
