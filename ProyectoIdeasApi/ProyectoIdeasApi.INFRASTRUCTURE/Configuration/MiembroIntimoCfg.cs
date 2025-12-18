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
    public class MiembroIntimoCfg : IEntityTypeConfiguration<MiembroIntimo>
    {
        public void Configure(EntityTypeBuilder<MiembroIntimo> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.FechaAgregado)
             .HasPrecision(6);

            e.Property(x => x.Nota)
             .HasMaxLength(300);

            // Relación: un Miembro (Propietario) tiene muchos MiembroIntimo
            e.HasOne(mi => mi.Propietario)
             .WithMany(m => m.Intimos)               // colección en Miembro
             .HasForeignKey(mi => mi.PropietarioId)
             .OnDelete(DeleteBehavior.Cascade);

            // Relación: el Intimo también es un Miembro
            e.HasOne(mi => mi.Intimo)
             .WithMany()                             // por ahora sin colección inversa
             .HasForeignKey(mi => mi.IntimoId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
