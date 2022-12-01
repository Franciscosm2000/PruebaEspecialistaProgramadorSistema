using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Producto
{
    public class ProductoModel
    {        
        public int IdProducto { get; set; }
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string CodigoProducto { get; set; }
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string NombreProducto { get; set; }
        [Required(ErrorMessage = "El {0} es requerido.")]
        public int Stock { get; set; }
        [Required(ErrorMessage = "El {0} es requerido.")]
        public double Precio { get; set; }
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El {0} es requerido.")]
        public bool EstadoFila { get; set; }
        [Required(ErrorMessage = "El {0} es requerido.")]
        public DateTime FechaRegistro { get; set; }
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
