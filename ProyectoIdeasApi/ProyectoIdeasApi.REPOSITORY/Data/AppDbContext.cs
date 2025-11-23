using Microsoft.EntityFrameworkCore;
using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INFRASTRUCTURE.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        // 🔹 Cada DbSet representa una tabla en la base
        public DbSet<Grupo> Grupos => Set<Grupo>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Miembro> Miembros => Set<Miembro>();
        public DbSet<Interesado> Interesados => Set<Interesado>();
        public DbSet<IdeaConcreta> IdeasConcretas => Set<IdeaConcreta>();
        public DbSet<Habilidad> Habilidades => Set<Habilidad>();
        public DbSet<MiembroGrupo> MiembrosGrupo => Set<MiembroGrupo>();
        public DbSet<MiembroIntimo> MiembrosIntimo => Set<MiembroIntimo>();
        public DbSet<Rubro> Rubros => Set<Rubro>();
        public DbSet<LinkExterno> LinksExterno => Set<LinkExterno>();
        protected override void OnModelCreating(ModelBuilder b)
        {
            // Esto le dice a EF que aplique todas las clases de configuración
            // que implementen IEntityTypeConfiguration<T> dentro del mismo assembly.
            b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
