using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Servicios
{
    public class StructureOutputRegion
    {
       
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<Region> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
        
    }
    public class Region
    {
        public int id_region { get; set; }
        [Required]
        public string nombre { get; set; }
    }
}
