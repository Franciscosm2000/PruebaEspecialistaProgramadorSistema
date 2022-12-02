using Datos.Mapping.Producto;
using Datos.Mapping.Seguridad;
using Microsoft.EntityFrameworkCore;
using Modelo.Producto;
using Modelo.Seguridad;
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

        #region SEGURIDAD
        public DbSet<UsuarioModel> usuario { get; set; }
        public DbSet<RolModel> rol { get; set; }
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

            #region SEGURIDAD
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new RolMap());
            #endregion
        }
    }
}
