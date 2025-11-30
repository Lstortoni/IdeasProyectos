using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INTERFACES.Infrastructure
{
    public interface IGrupoRepository
    {
        // Crear grupo
        Task AddAsync(Grupo grupo, CancellationToken ct = default);

        // Obtener uno
        Task<Grupo?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // Listar TODOS los grupos
        Task<List<Grupo>> GetAllAsync(CancellationToken ct = default);

        // Listar grupos donde participa un miembro
        Task<List<Grupo>> GetByMiembroAsync(Guid miembroId, CancellationToken ct = default);

        // Listar grupos creados a partir de una IdeaConcreta
        Task<List<Grupo>> GetByIdeaAsync(Guid ideaId, CancellationToken ct = default);

        // Guardar cambios
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
