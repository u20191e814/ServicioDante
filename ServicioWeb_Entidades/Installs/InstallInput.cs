using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Installs
{
    public class InstallInput
    {
        [Required]
        public string servicio {  get; set; } 
        [Required]
        public int fk_user { get; set; }
        [Required]
        public int fk_userAsig { get; set; }
        [Required]
        public string nombre_completo { get; set; } = string.Empty;
        [Required]
        public string telefono { get; set; } = string.Empty;
        [Required]
        public string info_adicional { get; set; } = string.Empty;
        
        [Required]
        public string statusInstalacion { get; set; } = string.Empty;

        public string comments { get; set; } = string.Empty;

        public string materials { get; set; } = string.Empty;
    }
}
