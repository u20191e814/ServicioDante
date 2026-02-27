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
   
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Usuarios : ControllerBase
    {
        private readonly IConfiguration _config;
       
        public Usuarios(IConfiguration config)
        {
           
            _config = config;
        }



        ///<remarks>
        ///
        /// Se agregará un nuevo usuario a la base de datos
        ///
        ///Ejemplo:
        ///
        ///     {
        ///        "Nombre":"Juan",
        ///        "Apellidos": "Tenorio Galvez",
        ///        "Dni": "00000012",
        ///        "Correo": "ejemplo@hotmail.com",
        ///        "Clave": "****"        
        ///     }
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
        /// |200|400| 0 |U1|The Dni exists| |
        /// |200|400| 0 |U2|The email exists| |        
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/create/")]
        public StructurePostInt crear(UserInput usuario)
        {
            StructurePostInt estructura = new();
            estructura.Status = "400";

            var pusuario = new DbUsuarios(AE.Decrypt(_config["Sqlconnetion"])).GetUserByDni(usuario.Dni);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage=pusuario.InternalMessage;               
                return estructura;
            }
            if (pusuario.Data>0)
            {
                estructura.StatusCode = "U1";
                estructura.StatusMessage = "The Dni exists";
                return estructura;
            }
            pusuario = new DbUsuarios(AE.Decrypt(_config["Sqlconnetion"])).GetUserByCorreo(usuario.Correo);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (pusuario.Data >0)
            {
                estructura.StatusCode = "U2";
                estructura.StatusMessage = "The email exists";
                return estructura;
            }

            
            pusuario = new DbUsuarios(AE.Decrypt(_config["Sqlconnetion"])).AddUser(usuario);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";  
                estructura.InternalMessage= pusuario.InternalMessage;
                return estructura;
            }
            
           
            return pusuario;

        }


        ///<remarks>
        ///
        /// Se loguea con los credenciales el usuario        
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Retorna el usuario",
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
        /// |200|400| 0 |U4|User does not exist| |       
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/login/")]
        public StructureOutputUserLogin Login(UserLogin usuario)
        {
            StructureOutputUserLogin estructura = new();
            estructura.Status = "400";

            StructureOutputUserLogin pusuario = new DbUsuarios(AE.Decrypt(_config["Sqlconnetion"])).Login(usuario.Correo, usuario.Clave);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (pusuario.Data ==null)
            {
                estructura.StatusCode = "U4";
                estructura.StatusMessage = "user does not exist";
                return estructura;
            }
            if (pusuario.Data.Habilitado)
            {
                var claimsIdentity = new[] { new Claim(ClaimTypes.Name, usuario.Correo) };

                var token = new JwtSecurityToken(
                    //issuer: _config["Jwt:Issuer"],
                    //audience: _config["Jwt:Audience"],
                    claims: claimsIdentity,
                    expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:TokenValidityInMinutes"])),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])), SecurityAlgorithms.HmacSha256));
                pusuario.TokenBearer = new JwtSecurityTokenHandler().WriteToken(token);
            }
           
            return pusuario;

        }



        ///<remarks>
        ///
        /// Permite modificar al usuario para habilitar el inicio de sesión y administrador         
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
        /// |200|400| 0 |U5|Could not update| |           
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("api/user/updateUser/")]
        public StructurePostBool updateUser(UserUpdate usuario)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new DbUsuarios(AE.Decrypt(_config["Sqlconnetion"])).UpdateUser(usuario);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (!pusuario.Data)
            {
                estructura.StatusCode = "U5";
                estructura.StatusMessage = "Could not update";
                return estructura;
            }

            return pusuario;

        }




        ///<remarks>
        ///
        /// Se obtiene la lista de usuarios
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
        /// |200|400| 0 |U3|There are no registered users| |               
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>
        
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("api/user/getUsers/")]
        public StructureOutputUsers ObtenerUsuarios()
        {
            StructureOutputUsers estructura = new();
            estructura.Status = "400";

            var pusuario = new DbUsuarios(AE.Decrypt(_config["Sqlconnetion"])).ObtenerUsuarios();
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (pusuario.Data==null || pusuario.Data.Count==0)
            {
                estructura.StatusCode = "U3";
                estructura.StatusMessage = "There are no registered users";
                estructura.Data=pusuario.Data;
                return estructura;
            }
            
           

            return pusuario;

        }

        ///<remarks>
        ///
        /// Permite eliminar el usuario        
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
        /// |200|400| 0 |U6|Could not be deleted| |           
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        [Route("api/user/deleteUser/")]
        public StructurePostBool deleteUser([Required] [Range(1,int.MaxValue)] int Pk_user)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new DbUsuarios(AE.Decrypt(_config["Sqlconnetion"])).deleteUser(Pk_user);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (!pusuario.Data)
            {
                estructura.StatusCode = "U6";
                estructura.StatusMessage = "Could not be deleted";
                return estructura;
            }

            return pusuario;

        }



    }




}
