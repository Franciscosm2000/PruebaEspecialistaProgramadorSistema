using Datos.Db_Context;
using Microsoft.AspNetCore.Mvc;
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

        [TestMethod]
        public async Task CrearProducto()
        {
            //preparacion
            var context = DbConextMemory.Get();

            var Producto = new Web.Models.Producto.CrearViewModel()
            {
                NombreProducto = "prueba",
                CodigoProducto = "123",
                Precio = 10,
                Stock = 10,
                Descripcion = "prueba"

            };

            // prueba
            var CrearProducto = new ProductosController(context);

            var Respuesta = await CrearProducto.Crear(Producto);

            // resultado

            var resulado = Respuesta as IActionResult;
            Assert.IsNotNull(resulado);


            var cantidad = context.producto.Count();

            Assert.AreEqual(1, cantidad);
        }

        [TestMethod]
        public async Task ActualizarProducto()
        {
            //preparacion
            var context = DbConextMemory.Get();

            var Producto = new ProductoModel()
            {
                NombreProducto = "prueba",
                CodigoProducto = "123",
                Precio = 10,
                Stock = 10,
                Descripcion = "prueba",
                UsuarioRegistro = "Admin"
               
            };

            context.producto.Add(Producto);

            context.SaveChanges();

            var IdProducto = 1;
            // prueba
            var ProductoModificar = new Web.Models.Producto.ActualizarViewModel()
            {
                IdProducto = IdProducto,
                NombreProducto = "pruebaUpdate",
                CodigoProducto = "123Update",
                Precio = 10,
                Stock = 10,
                Descripcion = "Actualizacion",
                UsuarioModificacion = "Admin"
            };
            // resultado
            var ProductoUdate = new ProductosController(context);

            var Respuesta = await ProductoUdate.Actualizar(ProductoModificar);

            var resulado = Respuesta as StatusCodeResult;

            Assert.AreEqual(200,resulado.StatusCode);

            var existe = context.producto.Any(d=> d.CodigoProducto == "123Update");

            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task ActivarProducto()
        {
            //preparacion
            var context = DbConextMemory.Get();

            var Producto = new ProductoModel()
            {
                
                NombreProducto = "prueba",
                CodigoProducto = "123",
                Precio = 10,
                Stock = 10,
                Descripcion = "prueba"
            };

            context.producto.Add(Producto);

            context.SaveChanges();
            // prueba

            var ProductoC = new ProductosController(context);

            var IdProducto = 1; var Usuario = "";

            var ActivarRegistro = await ProductoC.Activar(IdProducto,Usuario);

            var RespuestaActivar = ActivarRegistro as StatusCodeResult;

            Assert.AreEqual(200,RespuestaActivar.StatusCode);

        }

        [TestMethod]
        public async Task DesactivarProducto()
        {
            //preparacion
            var context = DbConextMemory.Get();

            var Producto = new ProductoModel()
            {

                NombreProducto = "prueba",
                CodigoProducto = "123",
                Precio = 10,
                Stock = 10,
                Descripcion = "prueba"
            };

            context.producto.Add(Producto);

            context.SaveChanges();
            // prueba

            var ProductoC = new ProductosController(context);

            var IdProducto = 1; var Usuario = "";

            var ActivarRegistro = await ProductoC.Desactivar(IdProducto, Usuario);

            var RespuestaActivar = ActivarRegistro as StatusCodeResult;

            Assert.AreEqual(200,RespuestaActivar.StatusCode);

        }
    }
}
