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
    public class GrupoCfg : IEntityTypeConfiguration<Grupo>
    {


        public void Configure(EntityTypeBuilder<Grupo> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre)
             .IsRequired()
             .HasMaxLength(100);

            e.Property(x => x.FechaCreacion)
             .HasPrecision(6);

            // IdeaConcreta (principal) 1 ---- 0..1 Grupo (dependiente)
            e.HasOne(g => g.IdeaConcreta)
             .WithOne(i => i.Grupo)
             .HasForeignKey<Grupo>(g => g.IdeaConcretaId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
