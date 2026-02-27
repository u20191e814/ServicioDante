using ServicioWeb_Entidades.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Deduccion
{
    public  class StructureOutputDeduccion
    {
        
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputDeduccion> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
        
    }
    public class OutputDeduccion
    {
        public int id_deduccion { get; set; }   
        public string nameMonth { get; set; }
        public string nameYear { get; set; }
        public string usuario { get; set; }
        public float monto { get; set; }
        public string fecha { get; set; }
        public int mes {  get; set; }   
    }
}
