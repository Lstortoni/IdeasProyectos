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
    public class IdeaConcretaCfg : IEntityTypeConfiguration<IdeaConcreta>
    {
        public void Configure(EntityTypeBuilder<IdeaConcreta> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(x => x.Proposito)
               .IsRequired()
               .HasMaxLength(300);

            e.Property(x => x.Color)
                .IsRequired()
                .HasMaxLength(20);
            e.Property(x => x.FechaCreacion).HasPrecision(6);

          
            e.HasOne(i => i.Creador)
              .WithMany(m => m.IdeasCreadas)
              .HasForeignKey(i => i.CreadorId)
              .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(i => i.Rubro)
              .WithMany()                 // Rubro no tiene colección de Ideas
              .HasForeignKey(i => i.RubroId)
              .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
