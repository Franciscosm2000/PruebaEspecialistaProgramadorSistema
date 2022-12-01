using Datos.Mapping.Producto;
using Microsoft.EntityFrameworkCore;
using Modelo.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Db_Context
{
    public class DbContextSistema : DbContext
    {
        #region PRODUCTO
        public DbSet<ProductoModel> producto { get; set; }
        #endregion


        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region PRODUCTO
            modelBuilder.ApplyConfiguration(new ProductoMap());
            #endregion
        }
}
}
