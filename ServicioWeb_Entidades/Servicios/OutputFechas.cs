using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Servicios
{
    public class OutputFechas
    {
        // ,estado, precio
        public DateTime fechaEstado { get; set; }
        public string estado { get; set; }    
        public float precio { get; set; }   
        public string servicio { get; set; }
    }
}
