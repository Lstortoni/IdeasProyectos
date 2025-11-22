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
        void IEntityTypeConfiguration<Miembro>.Configure(EntityTypeBuilder<Miembro> builder)
        {
            throw new NotImplementedException();
        }
    }
}
