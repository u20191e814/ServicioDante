using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Pagos
{
    public class Facturacion
    { 
        public float monto {  get; set; }
        public string nameMonth { get; set; }
        public string nameYear { get; set; }
        public int mes {  get; set; } 
        public float precio  { get; set; }
        public string texto { get
            {
                return "s/. "+ monto + " "   + nameMonth + "/" + nameYear;
            } }
    }
}
