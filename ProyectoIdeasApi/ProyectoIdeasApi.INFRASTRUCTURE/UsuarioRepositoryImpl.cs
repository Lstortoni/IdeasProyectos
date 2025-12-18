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
    public class UsuarioRepositoryImpl : IUsuarioRepository
    {

        private readonly AppDbContext _db;

        public UsuarioRepositoryImpl(AppDbContext db)
        {
            _db = db;
        }



        public Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct)
        {
            return _db.Usuarios
                .Include(u => u.Miembro)
                .FirstOrDefaultAsync(u => u.EmailLogin == email, ct);
        }

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
        {
            return _db.Usuarios.AnyAsync(u => u.EmailLogin == email, ct);
        }

        public async Task<Usuario> CreateUsuarioConMiembroAsync(
            Usuario usuario,
            Miembro miembro,
            CancellationToken ct)
        {
            // EF Core se encarga del grafo entero
            usuario.Miembro = miembro;

            _db.Usuarios.Add(usuario);

            await _db.SaveChangesAsync(ct);

            return usuario;
        }
    }
}
