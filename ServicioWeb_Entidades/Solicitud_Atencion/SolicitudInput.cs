using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Solicitud_Atencion
{
    public  class SolicitudInput
    {
        [Required]
        public int fk_user { get; set; }
        [Required]
        public int fk_userAsig { get; set; }
        [Required]
        public string statusAtencion { get; set; } = string.Empty;
        [Required]
        public long id_servicio { get; set; }
        [Required]
        public string info_cliente { get; set; }
        public string materials {  get; set; }  
        public string comments { get; set; }
    }
}
