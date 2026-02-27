using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicioWeb_BD;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Clientes;
using ServicioWeb_Entidades.Installs;
using ServicioWeb_Entidades.Servicios;
using System.ComponentModel.DataAnnotations;

namespace ServicioWeb.Controllers
{
     
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   // [AllowAnonymous]
    [ApiController]
    public class Instalacion : ControllerBase
    {
        private readonly IConfiguration _config;
        public Instalacion(IConfiguration config)
        {

            _config = config;
        }

        ///<remarks>
        ///
        /// Se agregará una nueva solicitud de instalacion
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
        [Route("api/install/create/")]
        public StructurePostInt crear(InstallInput servicio)
        {
            StructurePostInt estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Install(AE.Decrypt(_config["Sqlconnetion"])).crear(servicio);
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
        /// Permite modificar los datos de la instalacion         
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
        [Route("api/install/updateInstall/")]
        public StructurePostBool updateClient(InstallUpdate usuario)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Install(AE.Decrypt(_config["Sqlconnetion"])).UpdateInstall(usuario);
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
        /// Se obtiene la lista de instalaciones
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Lista de instalaciones",
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
        /// |200|400| 0 |I3|There are no registered installs| |               
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpGet]
        [Route("api/install/getInstalls/")]
        public StructureOutputInstalls getInstalls([Range(1, int.MaxValue)] int pagina, [Range(1, int.MaxValue)] int registros, string telOrNameclient = " ")
        {
            StructureOutputInstalls estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Install(AE.Decrypt(_config["Sqlconnetion"])).getInstalls(telOrNameclient, pagina, registros);
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
                estructura.StatusMessage = "There are no registered Installs";
                estructura.Data = pusuario.Data;
                return estructura;
            }



            return pusuario;

        }


        ///<remarks>
        ///
        /// Permite eliminar una instalacion        
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
        [Route("api/install/deleteInstall/")]
        public StructurePostBool deleteInstall([Required][Range(1, int.MaxValue)] int id_instalacion)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Install(AE.Decrypt(_config["Sqlconnetion"])).deleteInstall(id_instalacion);
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




        ///<remarks>
        ///
        /// Se obtiene la lista de instalaciones historico
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Lista de cambios segun las instalaciones",
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
        [Route("api/install/getInstallsHistory/")]
        public StructureOutputInstallHistory getInstallsHistory([Range(1, int.MaxValue)] int id_instalacion)
        {
            StructureOutputInstallHistory estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Install_History(AE.Decrypt(_config["Sqlconnetion"])).getInstallsHistory(id_instalacion);
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
        /// Se obtiene la lista de instalaciones asignados a un usuario segun estado Pendiente, En proceso, Finalizado
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Lista de instalaciones",
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
        [Route("api/install/getInstallsByUser/")]
        public StructureOutputInstallsStatus getInstallsByUser([Required] [Range(1, int.MaxValue)] int fk_user, [Required][StringLength(25, MinimumLength =5)] string estado = "Pendiente", [Required][Range(1, 101)] int cantidad= 100)
        {
            StructureOutputInstallsStatus estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Install(AE.Decrypt(_config["Sqlconnetion"])).getInstallsByUser(fk_user, estado, cantidad);
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
        /// Permite modificar el estado de la instalacion         
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
        [Route("api/install/updateInstallStatus/")]
        public StructurePostBool updateInstallStatus(InstallUpdateStatus usuario)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Install(AE.Decrypt(_config["Sqlconnetion"])).updateInstallStatus(usuario);
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

        
         

    }
}
