using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Servicios
{
    public  class StructureOutputProvinces
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<Provinces> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class Provinces
    {
        public int id_provincia { get; set; }
        public string nombre { get; set; }
        public int id_region { get; set; }
    }
}
