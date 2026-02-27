using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Installs
{
    public  class StructureOutputInstallHistory
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputInstallHistory> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }

    

    public class OutputInstallHistory
    {
        
        public string nombre_completo { get; set; }
        public string statusInstalacion { get; set; }
        public string servicio { get; set; }
        public string telefono { get; set; }
        public string info_adicional { get; set; }
        public string materials { get; set; }
        public string Fecha_creacion { get; set; }
        public string comments { get; set; } 
       
        public string usernameAsig { get; set; }
       

    }

}
