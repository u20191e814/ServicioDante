using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Servicios
{
    public  class ServiceInput
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int id_comunidad { get; set; }
        [Required]
        public string  servicio { get; set; }
        [Required]
        public string estado_servicio { get; set; }
        [Required]
        public float precio_servicio { get; set; }

        public DateTime fechainstalacion { get; set; }
        [Required]
        public string direccion_servicio { get; set; }
        public string referencia_servicio { get; set; }

        public float latitud {  get; set; } =0;
        public float longitud { get; set; }=0;
        [Required]
        [Range(1, int.MaxValue)]
        public int id_client { get; set; }
        [Required]
        [Range (1, int.MaxValue)]
        public int fk_user { get; set; } = 0;

        public string informacion_adicional { get; set; } = "";
        public string suministro { get; set; } = "";

    }
}
