using ServicioWeb_Entidades.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.B1
{
    public class StructureOuputSearchB1
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OuputSearchB1> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class OuputSearchB1
    {
        public int id_servicio { get; set; } = 0;
        public string Dni { get; set; } = string.Empty;
        public string Nombre_completo { get; set; } = string.Empty;
        public string servicio { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
        public string fechaInstalacion { get; set; } = string.Empty;
        public string direccion { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public float precio { get; set; } = 0;

        public string pendiente {  get; set; } = string.Empty;
        public string pagado {  get; set; } = string.Empty;
        public DateTime fechaEstado {  get; set; } = DateTime.MinValue;
        public float latitud { get; set; } = 0;
        public float longitud { get; set; } = 0;
        public string suministro {  get; set; } = string.Empty; 
        public string Telefono {  get; set; } = string.Empty;

        public int yearPagoInicio { get; set; }
        public int mesPagoInicio { get; set; }
        public float precioPagoInicio { get; set; }

        public int yearPagoFin { get; set; }
        public int mesPagoFin { get; set; }
        public float precioPagoFin { get; set; }
        public string referencia {  get; set; } = string.Empty;
    }
}
