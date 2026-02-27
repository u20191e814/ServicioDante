using ServicioWeb_Entidades.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Notificacion
{
    public class StructureOutputNotification
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputNotification> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class OutputNotification
    {
        public long id_notificacion { get; set; }
        public string titulo { get; set; }
        public string mensaje { get; set; }
        public DateTime fecha { get; set; }
        public long id_cliente { get; set; }
        public bool isready { get; set; }
        public string type { get; set; }
       
    }
}
