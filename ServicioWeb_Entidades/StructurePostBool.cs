using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades
{
    public  class StructurePostBool
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public bool Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
}
