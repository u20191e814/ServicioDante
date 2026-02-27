using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Notificacion
{
    public  class inputNotificacion
    {
        [Required]
        public string titulo { get; set; }
        [Required]
        public string mensaje { get; set; }
        [Required]
        public DateTime fecha { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public long id_cliente { get; set; }
        [Required]
        public string type { get; set; }
        
    }
}
