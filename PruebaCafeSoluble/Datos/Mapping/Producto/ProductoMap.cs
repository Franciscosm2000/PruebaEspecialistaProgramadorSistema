using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Mapping.Producto
{
    public class ProductoMap : IEntityTypeConfiguration<ProductoModel>
    {
        public void Configure(EntityTypeBuilder<ProductoModel> builder)
        {
            builder.ToTable("PRODUCTO", "PRODUCTO")
                .HasKey(c => c.IdProducto);
        }
    }
}
