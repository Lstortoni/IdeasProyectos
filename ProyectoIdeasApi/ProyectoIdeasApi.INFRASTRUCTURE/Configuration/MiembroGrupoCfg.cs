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
    public class MiembroGrupoCfg : IEntityTypeConfiguration<MiembroGrupo>
    {
        public void Configure(EntityTypeBuilder<MiembroGrupo> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Rol)
             .IsRequired()
             .HasMaxLength(30);

            e.Property(x => x.FechaIngreso)
             .HasPrecision(6);

            // Relación con Miembro
            e.HasOne(mg => mg.Miembro)
             .WithMany(m => m.GruposDondeParticipa)  // colección en Miembro
             .HasForeignKey(mg => mg.MiembroId)
             .OnDelete(DeleteBehavior.Cascade);

            // Relación con Grupo
            e.HasOne(mg => mg.Grupo)
             .WithMany(g => g.Miembros)             // colección en Grupo
             .HasForeignKey(mg => mg.GrupoId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
