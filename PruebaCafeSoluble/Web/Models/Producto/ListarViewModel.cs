using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Producto
{
    public class ListarViewModel
    {
        public int IdProducto { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Stock { get; set; }
        public double Precio { get; set; }
        public string Descripcion { get; set; }
        public string EstadoFila { get; set; }
        public string FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
