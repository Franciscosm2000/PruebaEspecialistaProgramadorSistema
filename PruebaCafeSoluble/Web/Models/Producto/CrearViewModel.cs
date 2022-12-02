﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Producto
{
    public class CrearViewModel
    {
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
        public string UsuarioRegistro { get; set; }
    }
}
