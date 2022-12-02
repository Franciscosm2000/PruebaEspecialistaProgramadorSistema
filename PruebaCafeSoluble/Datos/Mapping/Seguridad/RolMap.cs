using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Mapping.Seguridad
{
    public class RolMap : IEntityTypeConfiguration<RolModel>
    {
        public void Configure(EntityTypeBuilder<RolModel> builder)
        {
            builder.ToTable("ROL", "SEGURIDAD")
                .HasKey(c => c.IdRol);
        }
    }
}
