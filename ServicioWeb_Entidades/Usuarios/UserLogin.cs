using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Usuarios
{
    public  class UserLogin
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Clave { get; set; }
    }
}
