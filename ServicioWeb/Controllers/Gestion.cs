using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicioWeb_BD;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Pagos;
using ServicioWeb_Entidades.Servicios;
using ServicioWeb_Entidades.Solicitud_Atencion;
using System.ComponentModel.DataAnnotations;

namespace ServicioWeb.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class Gestion : ControllerBase
    {
        private readonly IConfiguration _config;
        public Gestion(IConfiguration config)
        {

            _config = config;
        }

        ///<remarks>
        ///
        /// Se agregará una nueva region
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
        [Route("api/gestion/createRegion/")]
        public StructurePostLong createRegion(Region region)
        {
            StructurePostLong estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Region(AE.Decrypt(_config["Sqlconnetion"])).crear(region);
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
        /// Se va a modificar la region
        ///    
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "True",
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
        [Route("api/gestion/UpdateRegion/")]
        public StructurePostBool UpdateRegion(Region region)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Region(AE.Decrypt(_config["Sqlconnetion"])).Update(region);
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
        /// Permite eliminar la region         
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
        [Route("api/gestion/deleteRegion/")]
        public StructurePostBool deleteRegion([Required][Range(1, int.MaxValue)] int id_region)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Region(AE.Decrypt(_config["Sqlconnetion"])).delete(id_region);
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

        //Provincia


        ///<remarks>
        ///
        /// Se agregará una nueva provincia
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
        [Route("api/gestion/createProvince/")]
        public StructurePostLong createProvince(Provinces provincia)
        {
            StructurePostLong estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Provincia(AE.Decrypt(_config["Sqlconnetion"])).crear(provincia);
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
        /// Se va a modificar la provincia
        ///    
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "True",
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
        [Route("api/gestion/updateProvince/")]
        public StructurePostBool updateProvince(Provinces province)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Provincia(AE.Decrypt(_config["Sqlconnetion"])).Update(province);
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
        /// Permite eliminar la provincia         
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
        [Route("api/gestion/deleteProvince/")]
        public StructurePostBool deleteProvince([Required][Range(1, int.MaxValue)] int id_provincia)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Provincia(AE.Decrypt(_config["Sqlconnetion"])).delete(id_provincia);
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

        //Distrito

        ///<remarks>
        ///
        /// Se agregará un nuevo distrito
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
        [Route("api/gestion/createDistrict/")]
        public StructurePostLong createDistrict(Districts district)
        {
            StructurePostLong estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Distrito(AE.Decrypt(_config["Sqlconnetion"])).crear(district);
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
        /// Se va a modificar el distrito 
        ///    
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "True",
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
        [Route("api/gestion/updateDistrict/")]
        public StructurePostBool updateDistrict(Districts district)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Distrito(AE.Decrypt(_config["Sqlconnetion"])).Update(district);
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
        /// Permite eliminar el distrito         
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
        [Route("api/gestion/deleteDistrict/")]
        public StructurePostBool deleteDistrict([Required][Range(1, int.MaxValue)] int id_district)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Distrito(AE.Decrypt(_config["Sqlconnetion"])).delete(id_district);
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

        //Comunidad

        ///<remarks>
        ///
        /// Se agregará una nueva comunidad
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
        [Route("api/gestion/createComunidad/")]
        public StructurePostLong createComunidad(Comunities comunidad)
        {
            StructurePostLong estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Comunidad(AE.Decrypt(_config["Sqlconnetion"])).crear(comunidad);
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
        /// Se va a modificar la comunidad 
        ///    
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "True",
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
        [Route("api/gestion/updateComunidad/")]
        public StructurePostBool updateComunidad(Comunities comunidad)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Comunidad(AE.Decrypt(_config["Sqlconnetion"])).Update(comunidad);
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
        /// Permite eliminar la comunidad         
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
        [Route("api/gestion/deleteComunidad/")]
        public StructurePostBool deleteComunidad([Required][Range(1, int.MaxValue)] int id_comunidad)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Comunidad(AE.Decrypt(_config["Sqlconnetion"])).delete(id_comunidad);
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


        //years


        ///<remarks>
        ///
        /// Se agregará un nuevo año
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
        [Route("api/gestion/createYear/")]
        public StructurePostLong createYear(Years year)
        {
            StructurePostLong estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Years(AE.Decrypt(_config["Sqlconnetion"])).crear(year);
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
        /// Se va a modificar el año 
        ///    
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "True",
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
        [Route("api/gestion/updateYear/")]
        public StructurePostBool updateYear(Years year)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Years(AE.Decrypt(_config["Sqlconnetion"])).Update(year);
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
        /// Permite eliminar el año         
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
        [Route("api/gestion/deleteYear/")]
        public StructurePostBool deleteYear([Required][Range(1, int.MaxValue)] int pk_Anio)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new Db_Years(AE.Decrypt(_config["Sqlconnetion"])).delete(pk_Anio);
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
        /// Se obtiene la lista de los años
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Lista de años",
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
        /// |200|400| 0 |P3|There are no registered| |               
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpGet]
        [Route("api/gestion/getYears/")]
        public StructureOutputYears getYears()
        {
            StructureOutputYears estructura = new();
            estructura.Status = "400";

            var pusuario = new Db_Years(AE.Decrypt(_config["Sqlconnetion"])).getYears();
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            
            return pusuario;

        }

    }
}
