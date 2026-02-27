using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades
{
    public  class UserInput
    {
        [Required]
        [StringLength(200, MinimumLength =1)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Apellidos { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 7)]
        public string Dni { get; set; }
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        [Required]
        [StringLength (100, MinimumLength = 1)]
        public string Clave { get; set; }
    }
}
