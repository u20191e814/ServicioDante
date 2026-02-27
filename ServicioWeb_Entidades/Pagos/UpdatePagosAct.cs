using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Pagos
{
    public class UpdatePagosAct
    {
        [Required]
        public long nroRecibo { get; set; }
        public bool isApproved { get; set; }
        
        public int Fk_userApproved { get; set; }

         
    }
}
