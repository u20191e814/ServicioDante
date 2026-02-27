using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Reportes
{
    public  class StructureOutputService_Status
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<Service_Status> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class Service_Status
    {
       
        public string servicio { get; set; } = "";
        public string estado { get; set; } = "";
        public int cantidad { get; set; } = 0;
    }
}
