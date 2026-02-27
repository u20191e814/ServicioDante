using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Servicios
{
    public  class ServiceUpdate
    {
        public int id_servicio { get; set; }
        public string referencia { get; set; }
        public string direccion { get; set; }
        public float precio { get; set; }
        public string estado { get; set; }
        public float latitud { get; set; } = 0;
        public float longitud { get; set; } = 0;
        public string servicio { get; set; }
        public int fk_user { get; set; } = 0;
        public string informacion_adicional { get; set; }
        public DateTime fechaEstado { get; set; }
        public string suministro { get; set; } = "";
        public int id_comunidad { get; set; } = 0;
    }
}
