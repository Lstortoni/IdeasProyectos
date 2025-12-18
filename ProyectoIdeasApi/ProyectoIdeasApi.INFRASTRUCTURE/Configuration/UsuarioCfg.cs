using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.INFRASTRUCTURE.Configuration
{
    public class UsuarioCfg : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.EmailLogin)
                .IsRequired()
                .HasMaxLength(200);

            e.HasOne(u => u.Miembro)
                .WithOne(m => m.Usuario)
                .HasForeignKey<Usuario>(u => u.MiembroId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
