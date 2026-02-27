using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Pagos
{
    public  class StructureOutputPagos
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputPagos> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class OutputPagos
    {

        public int id_facturacion { get; set; } = 0;
        public string fechaPago { get; set; } = "";
        public string comprobante { get; set; } = "";
        public string details { get; set; } = "";
        public string fechaIngresado { get; set; } = "";
        public int FK_Anio { get; set; } = 0;
        public int mes { get; set; } = 0;
        public string nameMonth { get; set; } = "";
        public int tipoPago { get; set; } = 0;
        public float monto { get; set; } = 0;
        public string nameYear { get; set; } = "";
        public string nameTipoPago { get; set; } = "";
        public long nroRecibo { get; set; } = 0;    
        public bool isApproved { get; set; } = false;
        public string servicio { get; set; } = "";
        //public string isApprovedText { }
    }
}
