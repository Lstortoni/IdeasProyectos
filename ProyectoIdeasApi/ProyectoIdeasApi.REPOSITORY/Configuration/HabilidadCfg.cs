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
    public class HabilidadCfg : IEntityTypeConfiguration<Habilidad>
    {
        public void Configure(EntityTypeBuilder<Habilidad> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            // Miembro (principal) 1 ----- * Habilidades (dependiente)
            e.HasOne(h => h.Miembro)
             .WithMany(m => m.Habilidades)
             .HasForeignKey(h => h.MiembroId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
