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
    public class RubroCfg : IEntityTypeConfiguration<Rubro>
    {
        public void Configure(EntityTypeBuilder<Rubro> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(100);
           
            e.Property(x => x.Descripcion)
              .IsRequired()
              .HasMaxLength(200);
        }
    }
}
