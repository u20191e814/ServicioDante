using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicioWeb_BD;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Notificacion;
using ServicioWeb_Entidades.Servicios;
using System.ComponentModel.DataAnnotations;

namespace ServicioWeb.Controllers
{
    
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Notificaciones : ControllerBase
    {
        private readonly IConfiguration _config;
        public Notificaciones(IConfiguration config)
        {

            _config = config;
        }

        ///<remarks>
        ///
        /// Se agregará una nueva notificacion al cliente en la base de datos
        ///    
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Mayor a 0 que es el nuevo id de base de datos",
        ///         "StatusCode":"0",
        ///         "StatusMessage" : "OK",
        ///         "InternalMessage":""
        ///     }             
        /// 
        /// Incorrecto:
        ///  
        ///     {
        ///         "Status" : "400",
        ///         "Data" : 0,
        ///         "StatusCode":"XX",
        ///         "StatusMessage" : "Mensaje del error",
        ///         "InternalMessage":"Detalle del error"
        ///     } 
        ///     
        /// |Http code |Status| Data | StatusCode| StatusMessage|InternalMessage|
        /// |----------|-------|------|------------|---------|------|        
        /// |200|400| 0 |D1|Database error| |            
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpPost]
        [Route("api/notificacion/create/")]
        public StructurePostLong createNotificacion(inputNotificacion notificacion)
        {
            StructurePostLong estructura = new();
            estructura.Status = "400";

            var pusuario = new DbNotificacion(AE.Decrypt(_config["Sqlconnetion"])).createNotificacion(notificacion);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            } 
            return pusuario;

        }

        ///<remarks>
        ///
        /// Se modifica a leido la notificacion al cliente
        ///    
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : True,
        ///         "StatusCode":"0",
        ///         "StatusMessage" : "OK",
        ///         "InternalMessage":""
        ///     }             
        /// 
        /// Incorrecto:
        ///  
        ///     {
        ///         "Status" : "400",
        ///         "Data" : False,
        ///         "StatusCode":"XX",
        ///         "StatusMessage" : "Mensaje del error",
        ///         "InternalMessage":"Detalle del error"
        ///     } 
        ///     
        /// |Http code |Status| Data | StatusCode| StatusMessage|InternalMessage|
        /// |----------|-------|------|------------|---------|------|        
        /// |200|400| 0 |D1|Database error| |            
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpPost]
        [Route("api/notificacion/update/")]
        public StructurePostBool updateNotificacion(updateNotificacion notificacion)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            var pusuario = new DbNotificacion(AE.Decrypt(_config["Sqlconnetion"])).updateNotificacion(notificacion);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            return pusuario;

        }



        ///<remarks>
        ///
        /// Se obtiene la lista de notificaciones por cliente       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Lista de notificaciones",
        ///         "StatusCode":"0",
        ///         "StatusMessage" : "OK",
        ///         "InternalMessage":""
        ///     }             
        /// 
        /// Incorrecto:
        ///  
        ///     {
        ///         "Status" : "400",
        ///         "Data" : 0,
        ///         "StatusCode":"XX",
        ///         "StatusMessage" : "Mensaje del error",
        ///         "InternalMessage":"Detalle del error"
        ///     } 
        ///     
        /// |Http code |Status| Data | StatusCode| StatusMessage|InternalMessage|
        /// |----------|-------|------|------------|---------|------|        
        /// |200|400| 0 |D1|Database error| |             
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpGet]
        [Route("api/notificacion/getNotifications/")]
        public StructureOutputNotification getNotifications([Range(1, int.MaxValue)] int id_client)
        {
            StructureOutputNotification estructura = new();
            estructura.Status = "400";

            var pusuario = new DbNotificacion(AE.Decrypt(_config["Sqlconnetion"])).getNotifications(id_client);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }

            return pusuario;

        }



        ///<remarks>
        ///
        /// Permite eliminar la notificacion         
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : true,
        ///         "StatusCode":"0",
        ///         "StatusMessage" : "OK",
        ///         "InternalMessage":""
        ///     }             
        /// 
        /// Incorrecto:
        ///  
        ///     {
        ///         "Status" : "400",
        ///         "Data" : false,
        ///         "StatusCode":"XX",
        ///         "StatusMessage" : "Mensaje del error",
        ///         "InternalMessage":"Detalle del error"
        ///     } 
        ///     
        /// |Http code |Status| Data | StatusCode| StatusMessage|InternalMessage|
        /// |----------|-------|------|------------|---------|------|        
        /// |200|400| 0 |D1|Database error| |
        /// |200|400| 0 |N6|Could not be deleted| |  
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpDelete]
        [Route("api/notificacion/deleteNotificacion/")]
        public StructurePostBool deleteNotificacion([Required][Range(1, int.MaxValue)] int id_notificacion)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new DbNotificacion(AE.Decrypt(_config["Sqlconnetion"])).deleteNotificacion(id_notificacion);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (!pusuario.Data)
            {
                estructura.StatusCode = "N6";
                estructura.StatusMessage = "Could not be deleted";
                return estructura;
            }

            return pusuario;

        }

    }
}
