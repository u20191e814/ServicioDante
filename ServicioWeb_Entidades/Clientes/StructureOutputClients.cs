using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Clientes
{
    public  class StructureOutputClients
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputClient> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }

    public class StructureOutputUser
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public OutputClient? Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";

    }

    public class OutputClient
    {
        public int listorder { get; set; }
        public int Pk_clients { get; set; }
        public string Nombre_completo { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Informacion_adicional { get; set; }
        public string Correo { get; set; }
        public string Fecha_creacion { get; set; }
        public int count_row { get; set; }

    }
}
