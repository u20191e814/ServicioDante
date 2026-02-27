using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Pagos
{
    public  class BillInput
    {
        //public int id_facturacion { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int id_servicio {  get; set; }
        [Required]
        public DateTime fechaPago  { get; set; }
        [Required]
        public string details { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int FK_Anio { get; set; }
        [Required]
        [Range(1, 13)]
        public int  mes {  get; set; }
        [Required]
        public int tipoPago  { get; set; }
        [Required]       
        [Range(1, int.MaxValue)]
        public float monto  { get; set; }
        public string nameMonth  { get; set; }
        public string comprobante { get; set; } = "";
        public string nameYear { get; set; }
        public string nameTipoPago { get; set; }

        
        public int Fk_user { get; set; }

        public string detailMeses { get; set; } ="";
        [Required]
        [Range(1, long.MaxValue)]
        public long nroRecibo { get; set; } = 0;
        public bool isApproved { get; set; } = true;
        public string servicio { get; set; } = "";
    }
}
