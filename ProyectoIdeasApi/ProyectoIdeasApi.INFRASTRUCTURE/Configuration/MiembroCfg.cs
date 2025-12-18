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
    public class MiembroCfg : IEntityTypeConfiguration<Miembro>
    {
        public void Configure(EntityTypeBuilder<Miembro> e)
        {
            e.HasKey(m => m.Id);

            e.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(m => m.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(m => m.Telefono)
                .HasMaxLength(20);

            e.Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(m => m.AutoDescripcion)
                .HasMaxLength(300);
            // Intimos / Habilidades los podés configurar cuando tengas claras esas entidades
        }
    }
}
