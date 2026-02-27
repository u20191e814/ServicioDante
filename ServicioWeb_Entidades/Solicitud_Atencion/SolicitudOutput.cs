using ServicioWeb_Entidades.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Solicitud_Atencion
{
    public class StructureOutputSolicitud
    {
        public string Status { get; set; } = "OK";
        public string StatusCode { get; set; } = "0";
        public List<SolicitudOutput> Data { get; set; }
        public string StatusMessage { get; set; } = "OK";
        public string InternalMessage { get; set; } = "";
    }
    public  class SolicitudOutput
    {
        public int listorder { get; set; }  
        public long id_SolicitudAtencion { get; set; }
        public string statusAtencion { get; set; }
        public int fk_userAsig { get; set; }
        public string Fecha_creacion  { get; set; }
        public string info_cliente {  get; set; }   
        public string materials {  get; set; }  
        public string comments { get; set; }
        public string usernameOrig {  get; set; }   
        public string usernameAsig { get; set; }
        public string ubicacion  { get; set; }
        public string servicio { get; set; }
        public string nombre_completo { get; set; } 
        public string direccion { get; set; }  
        public string Telefono { get; set; }
        public string Dni {  get; set; }
        public int count_row {  get; set; } 
    }

}
