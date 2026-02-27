using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Clientes
{
    public  class ClientInput
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Nombre_completo { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 7)]
        public string Dni { get; set; }=string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Informacion_adicional { get; set; }=string.Empty ;
        public string Correo { get; set; } = string.Empty;

    }
}
