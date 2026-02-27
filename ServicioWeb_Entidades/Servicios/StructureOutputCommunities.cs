using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Servicios
{
    public  class StructureOutputCommunities
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<Comunities> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class Comunities
    {
        public int id_comunidad { get; set; }
        public string nombre { get; set; }
        public int id_distrito { get; set; }
    }
}
