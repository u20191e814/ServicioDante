using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Pagos
{
    public  class StructureOutputYears
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<Years> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class Years
    {
        public int PK_Anio { get; set; }
        public string nombre { get; set; }
        public bool status { get; set; }
    }
}
