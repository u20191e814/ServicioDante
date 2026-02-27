using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Usuarios
{
    public  class StructureOutputUsers
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<OutputUsers> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public class StructureOutputUser
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public OutputUsers? Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";

    }
    public class StructureOutputUserLogin
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public OutputUsers? Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
        public string TokenBearer { get; set; } = "";
    }
    public class OutputUsers
    {
        public int Pk_user { get; set; }
        public string Nombre { get; set; }       
        public string Apellidos { get; set; }       
        public string Dni { get; set; }      
        public string Correo { get; set; }  
        public string Fecha_creado { get; set; }
        public string UltimaConexion { get; set; }
        public bool Habilitado { get; set; }
        public bool Administrador { get; set; }
        public bool Tecnico { get; set; }=false;
    }
}
