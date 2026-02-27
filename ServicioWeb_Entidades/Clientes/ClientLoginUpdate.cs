using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Clientes
{
    public  class ClientLoginUpdate
    {
        [Required]
        [StringLength(9, MinimumLength = 1)]
        public string dni { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string clave { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string nuevaclave { get; set; }
    }
}
