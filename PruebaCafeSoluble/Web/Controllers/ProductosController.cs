using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos.Db_Context;
using Modelo.Producto;
using Web.Models.Producto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Web.Controllers
{
    /// <summary>
    /// CONTROLADOR DE PRODUCTOS
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly DbContextSistema _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ProductosController(DbContextSistema context)
        {
            _context = context;
        }

        #region METODOS API
        /// <summary>
        /// CREA UN NUEVO PRODUCTO BASADO EN EL MODELO CREARVIEWMODEL
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CodProducto = await CodRepetido(model.CodigoProducto,0);

            //validar que el codigo no se repita
            if (CodProducto.Item1)
            {
                return BadRequest(CodProducto.Item2);
            }

            if (model.Precio <= 0 || model.Stock <= 0)
            {
                return BadRequest("El precio o el stock no pueden ser menor o igual a 0");
            }

            ProductoModel pro = new ProductoModel
            {
                CodigoProducto = model.CodigoProducto,
                NombreProducto = model.NombreProducto,
                Stock = model.Stock,
                Precio = model.Precio,
                Descripcion = model.Descripcion,
                EstadoFila = true,
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = model.UsuarioRegistro
            };

            _context.producto.Add(pro);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok("Registrado Correctamente.");
        }
        /// <summary>
        /// ACTUALIZA UN REGISTRO EXISTENTE EN LA BD
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdProducto <= 0)
            {                
                return BadRequest("El registro no ha sido encontrado.");
            }

            var producto = await _context.producto
                                .FirstOrDefaultAsync(p => p.IdProducto == model.IdProducto);

            if (producto == null)
            {                
                return NotFound("Registro no encontrado.");
            }

            if (model.Precio <= 0 || model.Stock <= 0)
            {
                return BadRequest("El precio o el stock no pueden ser menor o igual a 0");
            }


            var ValidarCodProducto = await CodRepetido(model.CodigoProducto,1);

            producto.CodigoProducto = model.CodigoProducto;
            producto.NombreProducto = model.NombreProducto;
            producto.Descripcion = model.Descripcion;
            producto.Stock = model.Stock;
            producto.Precio = model.Precio;
            producto.FechaModificacion = DateTime.Now;
            producto.UsuarioModificacion = model.UsuarioModificacion;

            try
            {
                await _context.SaveChangesAsync();               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }
        /// <summary>
        /// LISTA O MUESTRA TODOS LOS REGISTROS EN LA BD, CON SU RESPECTIVO ESTADO 
        /// PUEDE SER ACTIVO E INACTIVO
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<ListarViewModel>> Listar()
        {
            var tipo = await _context.producto.OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();

            return tipo.Select(p => new ListarViewModel
            {
                IdProducto = p.IdProducto,
                CodigoProducto = p.CodigoProducto,
                NombreProducto = p.NombreProducto,
                Stock = p.Stock,
                Precio = p.Precio,
                Descripcion = p.Descripcion,
                UsuarioModificacion = p.UsuarioModificacion,
                UsuarioRegistro = p.UsuarioRegistro,
                FechaModificacion = p.FechaModificacion?.ToString("dd/MM/yyyy hh:mm:ss"),
                FechaRegistro = p.FechaRegistro.ToString("dd/MM/yyyy hh:mm:ss"),
                EstadoFila = p.EstadoFila ? "Activo" : "Inactivo"
            });

        }
        /// <summary>
        /// ESTE METODO REALIZA UN ELIMINADO LOGICO, LA IDEA ES QUE SOLO EL ADMINISTRADOR TENGA PERMISO
        /// DE VER LOS ITEMS CON ESTADO DESACTIVADO
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPut("[action]/{id}/{usuario}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id, string usuario)
        {

            if (id <= 0 || id.Equals(""))
            {
                return BadRequest("Id no encontrado.");
            }

            var cat = await _context.producto.FirstOrDefaultAsync(p => p.IdProducto == id);

            if (cat == null)
            {
                return NotFound("Registro no encontrado.");
            }

            cat.EstadoFila = false;
            cat.UsuarioModificacion = usuario;
            cat.FechaModificacion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }
        /// <summary>
        /// PERMITE ACTIVAR O RESTRAURAR UN ELEMENTO ANTERIORMENTE BORRADO (BORRADO LOGICO)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPut("[action]/{id}/{usuario}")]
        public async Task<IActionResult> Activar([FromRoute] int id, string usuario)
        {

            if (id <= 0 || id.Equals(""))
            {
                return BadRequest("Registro no encontrado.");
            }

            var cat = await _context.producto.FirstOrDefaultAsync(p => p.IdProducto == id);

            if (cat == null)
            {
                return NotFound("Id no encontrado.");
            }

            cat.EstadoFila = true;
            cat.UsuarioModificacion = usuario;
            cat.FechaModificacion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Guardar Excepción
                return BadRequest(ex);

            }

            return Ok();
        }
        #endregion

        #region VALIDACIONES
        [HttpGet]
        public async Task<(bool, string)> CodRepetido(string CodProducto, int accion = 0)
        {
            var Productos = await _context.producto
                .Where(d=> d.CodigoProducto.Trim() == CodProducto.Trim()).ToListAsync();

            var Resultado = false;

            if (accion == 0) // nuevo
            {
                Resultado = Productos.Count() > 0 ? true : false;
            }
            else if (accion == 1) // actualizar
            {
                Resultado = Productos.Count() > 1 ? true : false;
            }


            return (Resultado, Resultado ? string.Format("El Código {0}, ya se encuentra registrado.",CodProducto):"");
        }
        #endregion

    }
}
