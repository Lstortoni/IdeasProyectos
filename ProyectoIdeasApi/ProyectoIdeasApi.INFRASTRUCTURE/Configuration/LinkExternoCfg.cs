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
    public class LinkExternoCfg : IEntityTypeConfiguration<LinkExterno>
    {
        public void Configure(EntityTypeBuilder<LinkExterno> e)
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(x => x.Url);

            // Miembro (principal) 1 ----- * Habilidades (dependiente)
            e.HasOne(h => h.Grupo)
             .WithMany(m => m.LinksExternos)
             .HasForeignKey(h => h.GrupoId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
