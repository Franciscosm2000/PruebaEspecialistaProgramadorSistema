using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Seguridad
{
    public class LoginViewModel
    {
        [Required]
        public string usuario { get; set; }
        [Required]
        public string password { get; set; }
    }
}
