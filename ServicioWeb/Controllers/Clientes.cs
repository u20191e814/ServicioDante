using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicioWeb_BD;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Clientes;
using System.ComponentModel.DataAnnotations;

namespace ServicioWeb.Controllers
{
    
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Clientes : ControllerBase
    {
        private readonly IConfiguration _config;
        public Clientes(IConfiguration config)
        {

            _config = config;
        }
        ///<remarks>
        ///
        /// Se agregará un nuevo cliente a la base de datos
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
        /// |200|400| 0 |C1|The Dni exists| |             
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpPost]
        [Route("api/client/create/")]
        public StructurePostInt crear(ClientInput usuario)
        {
            StructurePostInt estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Clientes(AE.Decrypt(_config["Sqlconnetion"])).GetClientsByDni(usuario.Dni);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (pusuario.Data > 0)
            {
                estructura.StatusCode = "C1";
                estructura.StatusMessage = "The Dni exists";
                return estructura;
            }
            

            pusuario = new Db_Clientes(AE.Decrypt(_config["Sqlconnetion"])).AddClients(usuario);
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
        /// Permite modificar los datos del cliente         
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
        /// |200|400| 0 |C5|Could not update| |           
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

       
        [HttpPost]
        [Route("api/client/updateClient/")]
        public StructurePostBool updateClient(ClientUpdate usuario)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Clientes(AE.Decrypt(_config["Sqlconnetion"])).UpdateClient(usuario);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (!pusuario.Data)
            {
                estructura.StatusCode = "C5";
                estructura.StatusMessage = "Could not update";
                return estructura;
            }

            return pusuario;

        }

        ///<remarks>
        ///
        /// Se obtiene la lista de clientes
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Lista de usuarios",
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
        /// |200|400| 0 |C3|There are no registered clients| |               
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>
      
        [HttpGet]
        [Route("api/client/getClients/")]
        public StructureOutputClients getUsers(  [Range(1,int.MaxValue)] int pagina, [Range(1, int.MaxValue)] int registros, string dni=" ")
        {
            StructureOutputClients estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Clientes(AE.Decrypt(_config["Sqlconnetion"])).ObtenerClients(dni, pagina, registros);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (pusuario.Data == null || pusuario.Data.Count == 0)
            {
                estructura.StatusCode = "C3";
                estructura.StatusMessage = "There are no registered clients";
                estructura.Data = pusuario.Data;
                return estructura;
            }



            return pusuario;

        }

        ///<remarks>
        ///
        /// Se obtiene la lista de los cambios por cliente
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Lista de cambios del cliente",
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
        [Route("api/client/getClientsHistory/")]
        public StructureOutputClients getClientsHistory([Range(1, int.MaxValue)] int Fk_Client)
        {
            StructureOutputClients estructura = new();
            estructura.Status = "400";

            var pusuario = new DbClients_History(AE.Decrypt(_config["Sqlconnetion"])).ObtenerClients(Fk_Client);
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
        /// Permite eliminar el cliente        
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
        /// |200|400| 0 |S6|Could not be deleted| |  
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpDelete]
        [Route("api/client/deleteClient/")]
        public StructurePostBool deleteClient([Required][Range(1, int.MaxValue)] int id_client)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Clientes(AE.Decrypt(_config["Sqlconnetion"])).deleteClient(id_client);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (!pusuario.Data)
            {
                estructura.StatusCode = "S6";
                estructura.StatusMessage = "Could not be deleted";
                return estructura;
            }

            return pusuario;

        }

    }
}
