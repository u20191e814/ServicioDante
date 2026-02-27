using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Servicios
{
    
    public class StructureHistoryOuput
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<HistoryOuput> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class HistoryOuput
    {
        public string updateService { get; set; } = "";
        public string estado { get; set; } = "";
        public string referencia { get; set; } = "";
        public string precio { get; set; } = "";
        public string direccion { get; set; } = "";
        public float longitud { get; set; } = 0;
        public float latitud { get; set; } = 0;
        public string servicio { get; set; } = "";
        public string usuario { get; set; } = "";
        public string informacion_adicional { get; set; } = "";
        public string fechaEstado { get; set; } = "";
        public string fechaEstado2 { get; set; } = "";
        public string suministro { get; set; } = "";
        public string ubicacion { get; set; } = "";
    }
}
