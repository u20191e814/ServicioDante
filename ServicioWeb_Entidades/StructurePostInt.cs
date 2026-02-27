using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades
{
    public  class StructurePostInt
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public int Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }

    public class StructurePostLong
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public long Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
}
