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
    public class MiembreIntimoCfg : IEntityTypeConfiguration<MiembroIntimo>
    {
        void IEntityTypeConfiguration<MiembroIntimo>.Configure(EntityTypeBuilder<MiembroIntimo> builder)
        {
            throw new NotImplementedException();
        }
    }
}
