using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Reportes
{
    public class StructureOutputGlobalChart
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List< SerieChart> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }

    public class SerieChart
    {
        public string serie { get; set; }
        public int cantidad { get; set; }
    }
}
