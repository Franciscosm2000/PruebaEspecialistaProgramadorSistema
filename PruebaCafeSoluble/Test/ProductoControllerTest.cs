using Datos.Db_Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modelo.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.DbContext;
using Web.Controllers;

namespace Test
{
    [TestClass]
    public class ProductoControllerTest
    {
        [TestMethod]
        public async Task ListarProducto()
        {
            //preparacion
            var context =  DbConextMemory.Get();

            var Producto = new ProductoModel()
            {
                NombreProducto = "prueba",
                CodigoProducto = "123",
                Precio = 10.2,
                Stock = 10,
                EstadoFila = true,
                Descripcion = "prueba"

            };

            context.producto.Add(Producto);

            context.SaveChanges();

            // prueba
            var CrearProducto = new ProductosController(context);

            var Respuesta =await CrearProducto.Listar();

            // resultado

            var producto = Respuesta.Count();

            Assert.AreEqual(1,producto);
        }
    }
}
