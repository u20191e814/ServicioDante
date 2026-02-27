using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicioWeb_BD;
using ServicioWeb_Entidades.B1;
using ServicioWeb_Entidades.Clientes;
using ServicioWeb_Entidades.Pagos;
using System.ComponentModel.DataAnnotations;

namespace ServicioWeb.Controllers
{
    [ApiController]
   //[AllowAnonymous]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Busqueda1 : ControllerBase
    {
        private readonly IConfiguration _config;
        public Busqueda1(IConfiguration config)
        {

            _config = config;
        }

        ///<remarks>
        ///
        /// Se obtiene la lista de la busqueda
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Lista ",
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
        /// |200|400| 0 |B1|There are no results| |               
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpGet]
        [Route("api/busquedaB1/getSearch/")]
        public StructureOuputSearchB1 getSearch([Range(1, int.MaxValue)] int id_distrito,[Range(0, int.MaxValue)] int id_comunidad, [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El campo solo puede contener letras.")][StringLength(12, MinimumLength = 2)] string servicio = "todos", [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El campo solo puede contener letras.")][StringLength(12, MinimumLength = 2)] string estado = "todos",[Required] [Range(0,4)] int pagosB1 =0, [Required][Range(2020, 2150)] int Anio =2020)
        {
            StructureOuputSearchB1 estructura = new();
            estructura.Status = "400";
           

            var pusuario = new Db_B1(AE.Decrypt(_config["Sqlconnetion"])).getSearch(id_distrito,id_comunidad, servicio,estado, pagosB1 ,Anio  );
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (pusuario.Data == null || pusuario.Data.Count == 0)
            {
                estructura.StatusCode = "B1";
                estructura.StatusMessage = "There are no results";
                estructura.Data = pusuario.Data;
                return estructura;
            }



            return pusuario;

        }



    }
}
