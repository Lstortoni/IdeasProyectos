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
    public class InteresadoCfg : IEntityTypeConfiguration<Interesado>
    {
        public void Configure(EntityTypeBuilder<Interesado> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.FechaInteres)
              .HasPrecision(6);

            e.Property(x => x.Comentario)
               .IsRequired()
               .HasMaxLength(300);


            e.HasOne(i => i.IdeaConcreta)
              .WithMany(m=>m.Interesados)                 // Rubro no tiene colección de Ideas
              .HasForeignKey(i => i.IdeaConcretaId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
