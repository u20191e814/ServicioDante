using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Reportes
{
   

    public class StructureOuputRep_Global
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public Global Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class Global
    {
        public int cantidad_client { get; set; } = 0;
        public int cantidad_service { get; set; } = 0;
        public int servicio_activo { get; set; } = 0;
        public int servicio_suspendido { get; set; } = 0;
        public int servicio_cortado { get; set; } = 0;
        public float monto_activo { get; set; } = 0;
        public float monto_suspendido { get;set; } = 0;
        public float monto_cortado { get; set; } = 0;   
    }
}
