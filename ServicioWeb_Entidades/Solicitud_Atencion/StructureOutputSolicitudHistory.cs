using ServicioWeb_Entidades.Installs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Solicitud_Atencion
{
    public class StructureOutputSolicitudHistory
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputSolicitudHistory> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";

    }
    public class OutputSolicitudHistory
    {
        public string statusAtencion { get; set; }
        public string Fecha_creacion { get; set; }
        public string usernameAsig { get; set; }
        public string info_cliente { get; set; }
        public string materials { get; set; }
        public string comments { get; set; }
        
    }
}
