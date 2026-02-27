using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Solicitud_Atencion
{
    public  class SolicitudUpdate
    {
        [Required]
        public long id_SolicitudAtencion  { get; set; }

        [Required]
        public int fk_userAsig { get; set; }
        [Required]
        public string statusAtencion { get; set; } =  "";
        
        [Required]
        public string info_cliente { get; set; }
        public string materials { get; set; }
        public string comments { get; set; }
    }

    public class SolicitudUpdateStatus
    {
        [Required]
        public long id_SolicitudAtencion { get; set; }

        public string statusAtencion { get; set; } = "";


    }

    public class SolicitudUpdateStatusDetails
    {
        [Required]
        public long id_SolicitudAtencion { get; set; }

        public string materials { get; set; } = "";
        [Required]
        public string statusAtencion { get; set; } = "";
        [Required]
        public string comments { get; set; } = "";
    }
}
