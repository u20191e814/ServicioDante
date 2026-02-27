using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Pagos
{
    public  class StructureOutputPagosPendientes
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<PagosPendientes> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }

    public class PagosPendientes
    {
       
        public long rowNum { get; set; } = 0;
        public long id_servicio { get; set; } = 0;
        public string fechaPago { get; set; } = "";
        public long nroRecibo { get; set; } = 0;
        public string details { get; set; } = "";
        public float monto { get; set; } = 0;
        public string direccion { get; set; } = "";
        public string nombre_completo { get; set; } = "";
        public string ubicacion { get; set; } = "";
    }
}
