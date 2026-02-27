using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Deduccion
{
    public  class deduccionInput
    {
        public int id_servicio { get; set; }
        public int mes {  get; set; }
        public string nameMonth { get; set; }
        public string nameYear { get; set; }
        public int Fk_user { get; set; }
        public float monto { get; set; }
    }
}
