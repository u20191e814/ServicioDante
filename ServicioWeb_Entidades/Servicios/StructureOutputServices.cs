using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Servicios
{
    public  class StructureOutputServices
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputService> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";

    }
    public class OutputService
    {
        public int id_servicio { get; set; }
        public string servicio { get; set; }
        public string estado { get; set; }
        public float precio { get; set; }
        public string fechainstalacion { get; set; }
        public string direccion { get; set; }
        public string referencia { get; set; }
        public int id_client { get; set; }
        public string ubicacion { get; set; }
        public float latitud { get; set; } = 0;
        public float longitud { get; set;} = 0;
        public string informacion_adicional { get; set; } = "";
        public string fechaEstado { get; set; } = "";
        public string fechaEstado2 { get; set; } = "";

        public int yearPagoInicio { get; set; }
        public int mesPagoInicio { get; set; }
        public float precioPagoInicio { get; set; }

        public int yearPagoFin { get; set; }
        public int mesPagoFin { get; set; }
        public float precioPagoFin { get; set; }
        public string suministro { get; set; } = "";
        public string pagado { get; set; } = "";
        public string pendiente { get; set; } = "";
        public string pendienteJson { get; set; } = "";
        public int id_region { get; set; } = 0;
        public int id_provincia { get; set; } = 0;
        public int id_distrito { get; set; } = 0;
        public int id_comunidad { get; set; } = 0;
        public string Dni { get; set; } = "";
        public string Nombre_completo { get; set; } = "";
        public string Telefono { get; set; } = "";
        //r.,p.,c. ,c.
    }
}
