using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INTERFACES.Infrastructure
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<Usuario> CreateUsuarioConMiembroAsync(Usuario usuario, Miembro miembro, CancellationToken ct);
    }
}
