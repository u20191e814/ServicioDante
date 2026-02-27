using ServicioWeb_Entidades.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Installs
{
    public  class StructureOutputInstalls
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputInstall> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class StructureOutputInstallsStatus
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputInstallStatus> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class OutputInstall
    {
        public int listorder { get; set; }
        public int id_instalacion { get; set; }
        public string nombre_completo { get; set; }
        public string statusInstalacion { get; set; }
        public string servicio { get; set; }
        public string telefono { get; set; }
        public string info_adicional { get; set; }
        public string materials { get; set; }
        public string Fecha_creacion { get; set; }
        public string comments  { get; set; }
        public int fk_userAsig { get; set; }
        public string usernameOrig { get; set; }
        public string usernameAsig { get; set; }    
        public int count_row { get; set; }

    }


    public class OutputInstallStatus
    {
        
        public int id_instalacion { get; set; }
        public string nombre_completo { get; set; } = "";
        public string servicio { get; set; } = "";
        public string telefono { get; set; } = "";
        public string info_adicional { get; set; } = "";
        public string materials { get; set; } = "";
        public string Fecha_creacion { get; set; } = "";
        public string comments { get; set; } = "";
        

    }

}
