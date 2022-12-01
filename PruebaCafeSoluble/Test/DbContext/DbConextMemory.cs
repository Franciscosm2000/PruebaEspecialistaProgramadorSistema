using Datos.Db_Context;
using Microsoft.EntityFrameworkCore;
namespace Test.DbContext
{
    public static class DbConextMemory
    {
        public static DbContextSistema Get()
        {
            var options = new DbContextOptionsBuilder<DbContextSistema>()
                .UseInMemoryDatabase(databaseName: $"Producto.Db")
                .Options;

            return new DbContextSistema(options);
        }
    }
}
