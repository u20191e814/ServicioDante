using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServicioWeb_BD;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Clientes;
using ServicioWeb_Entidades.Usuarios;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServicioWeb.Controllers
{
    [ApiController]
    [AllowAnonymous]
    
    public class App : ControllerBase
    {
        private readonly IConfiguration _config;
        public App(IConfiguration config)
        {

            _config = config;
        }


        ///<remarks>
        ///
        /// Se loguea con los credenciales del cliente        
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Retorna el cliente",
        ///         "StatusCode":"0",
        ///         "StatusMessage" : "OK",
        ///         "InternalMessage":"",
        ///         "TokenBearer":"Valor admisible segun cada x minutos "
        ///     }             
        /// 
        /// Incorrecto:
        ///  
        ///     {
        ///         "Status" : "400",
        ///         "Data" : 0,
        ///         "StatusCode":"XX",
        ///         "StatusMessage" : "Mensaje del error",
        ///         "InternalMessage":"Detalle del error",
        ///         "TokenBearer":""
        ///     } 
        ///     
        /// |Http code |Status| Data | StatusCode| StatusMessage|InternalMessage|
        /// |----------|-------|------|------------|---------|------|        
        /// |200|400| 0 |D1|Database error| |
        /// |200|400| 0 |C4|Client does not exist| |       
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpPost]
        [Route("api/app/loginClient/")]
        public ClientOutput loginClient(ClientInputLogin usuario)
        {
            ClientOutput estructura = new();
            estructura.Status = "400";
            //return estructura;
            ClientOutput pusuario = new ServicioWeb_BD.Db_Clientes(AE.Decrypt(_config["Sqlconnetion"])).logincl(usuario.dni, usuario.clave);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (pusuario.Data == null)
            {
                estructura.StatusCode = "C4";
                estructura.StatusMessage = "Client does not exist";
                return estructura;
            }
            var claimsIdentity = new[] { new Claim(ClaimTypes.Name, usuario.dni) };

            var token = new JwtSecurityToken(
                //issuer: _config["Jwt:Issuer"],
                //audience: _config["Jwt:Audience"],
                claims: claimsIdentity,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:TokenValidityInMinutes"])),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])), SecurityAlgorithms.HmacSha256));
            pusuario.TokenBearer = new JwtSecurityTokenHandler().WriteToken(token);

            return pusuario;

        }


        ///<remarks>
        ///
        /// Se agrega la clave del cliente        
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
        ///         "InternalMessage":"",
        ///          
        ///     }             
        /// 
        /// Incorrecto:
        ///  
        ///     {
        ///         "Status" : "400",
        ///         "Data" : 0,
        ///         "StatusCode":"XX",
        ///         "StatusMessage" : "Mensaje del error",
        ///         "InternalMessage":"Detalle del error",
        ///         
        ///     } 
        ///     
        /// |Http code |Status| Data | StatusCode| StatusMessage|InternalMessage|
        /// |----------|-------|------|------------|---------|------|        
        /// |200|400| 0 |D1|Database error| |
        /// |200|400| 0 |C4|User does not exist| |       
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpPost]
        [Route("api/app/createClient/")]
        public StructurePostBool createClient(ClientInputLogin usuario)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";
            //return estructura;
            StructurePostBool pusuario = new Db_Clientes(AE.Decrypt(_config["Sqlconnetion"])).createLogin(usuario.dni, usuario.clave);
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
        /// Se cambia la clave del cliente        
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
        ///         "InternalMessage":"",
        ///          
        ///     }             
        /// 
        /// Incorrecto:
        ///  
        ///     {
        ///         "Status" : "400",
        ///         "Data" : 0,
        ///         "StatusCode":"XX",
        ///         "StatusMessage" : "Mensaje del error",
        ///         "InternalMessage":"Detalle del error",
        ///         
        ///     } 
        ///     
        /// |Http code |Status| Data | StatusCode| StatusMessage|InternalMessage|
        /// |----------|-------|------|------------|---------|------|        
        /// |200|400| 0 |D1|Database error| |
        /// |200|400| 0 |C4|User does not exist| |       
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpPost]
        [Route("api/app/updateclient/")]
        public StructurePostBool updateclient(ClientLoginUpdate usuario)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";
            //return estructura;
            StructurePostBool pusuario = new Db_Clientes(AE.Decrypt(_config["Sqlconnetion"])).updateloginClient(usuario.dni, usuario.clave, usuario.nuevaclave);
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
        /// Se resetea la clave del cliente para ingresar al app        
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
        ///         "InternalMessage":"",
        ///          
        ///     }             
        /// 
        /// Incorrecto:
        ///  
        ///     {
        ///         "Status" : "400",
        ///         "Data" : False,
        ///         "StatusCode":"XX",
        ///         "StatusMessage" : "Mensaje del error",
        ///         "InternalMessage":"Detalle del error",
        ///         
        ///     } 
        ///     
        /// |Http code |Status| Data | StatusCode| StatusMessage|InternalMessage|
        /// |----------|-------|------|------------|---------|------|        
        /// |200|400| 0 |D1|Database error| |
        /// |200|400| 0 |C4|User does not exist| |       
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("api/app/resetclient/")]
        public StructurePostBool resetclient([Required][Range(1, long.MaxValue)] long  id_client)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";
            //return estructura;
            StructurePostBool pusuario = new Db_Clientes(AE.Decrypt(_config["Sqlconnetion"])).ResetloginClient(id_client);
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
