using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Reportes
{
    public class StructureOuput_Cobros
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<cobros> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class cobros
    {
        public int nroFila { get; set; }
        public string fecha_cobro { get; set; }
        public string servicio {  get; set; }
        public string Nombre_completo { get; set; }
        public string ubicacion { get; set; }
        public float monto { get; set; }
        public string nameMonth { get; set; }
        public string nameYear { get; set; }
        public string nameTipoPago { get; set; }
        public int nroRecibo { get; set; }
        public string cobrador { get; set; }
        public string aprobado { get; set; }
        public int Fk_user
        { get; set; }
        
    }
}
