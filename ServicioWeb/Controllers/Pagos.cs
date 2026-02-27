using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicioWeb_BD;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Pagos;
using System.ComponentModel.DataAnnotations;

namespace ServicioWeb.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   // [AllowAnonymous]
    [ApiController]
    public class Pagos : ControllerBase
    {

        private readonly IConfiguration _config;
        public Pagos(IConfiguration config)
        {

            _config = config;
        }

        ///<remarks>
        ///
        /// Permite ingresar los datos de la facturacion         
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
        /// |200|400| 0 |P2|Could not ingresar| |           
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>


        [HttpPost]
        [Route("api/pagos/insertBilling/")]
        public StructurePostBool insertBilling(BillInput usuario)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";
            //List<Facturacion> listaFacturacion = new DbPagos(AE.Decrypt(_config["Sqlconnetion"])).GetFacturacion(usuario.id_servicio, usuario.mes, usuario.nameYear);
            //if (listaFacturacion != null && listaFacturacion.Count > 0)
            //{
            //    var primero = listaFacturacion.First();
            //    float sumaPrecio = listaFacturacion.Sum(v => v.monto);
            //    float nuevamonto = sumaPrecio + usuario.monto;
            //    if (nuevamonto > primero.precio)
            //    {
            //        estructura.StatusCode = "P5";
            //        estructura.StatusMessage = "El monto ingresado supera al precio establecido en el servicio ";
            //        estructura.InternalMessage = "Se supera por " + (nuevamonto - primero.precio) + " soles";
            //        return estructura;
            //    }


            //}
            StructurePostBool pusuario = new DbPagos(AE.Decrypt(_config["Sqlconnetion"])).InsertBilling(usuario);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (!pusuario.Data)
            {
                estructura.StatusCode = "P2";
                estructura.StatusMessage = "Could not insert";
                return estructura;
            }

            return pusuario;

        }



        ///<remarks>
        ///
        /// Se obtiene la lista de los ultimos 12 meses de la facturación
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "Lista de facturacion",
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
        [Route("api/pagos/getFacturacion/")]
        public StructureOutputPagos getFacturacion(int id_servicio)
        {
            StructureOutputPagos estructura = new();
            estructura.Status = "400";

            var pusuario = new DbPagos(AE.Decrypt(_config["Sqlconnetion"])).getFacturacion(id_servicio);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            //if (pusuario.Data == null || pusuario.Data.Count == 0)
            //{
            //    estructura.StatusCode = "P1";
            //    estructura.StatusMessage = "There are no registered billing";
            //    estructura.Data = pusuario.Data;
            //    return estructura;
            //}



            return pusuario;

        }


        ///<remarks>
        ///
        /// Se obtiene la lista de los años activos
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
        [Route("api/pagos/getYears/")]
        public StructureOutputYears getYears()
        {
            StructureOutputYears estructura = new();
            estructura.Status = "400";

            var pusuario = new DbPagos(AE.Decrypt(_config["Sqlconnetion"])).getYears();
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (pusuario.Data == null || pusuario.Data.Count == 0)
            {
                estructura.StatusCode = "P3";
                estructura.StatusMessage = "There are no registered";
                estructura.Data = pusuario.Data;
                return estructura;
            }



            return pusuario;

        }

        ///<remarks>
        ///
        /// Permite eliminar el pago realizado        
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
        /// |200|400| 0 |P4|Could not be deleted| |           
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>

        [HttpDelete]
        [Route("api/pagos/delete/")]
        public StructurePostBool delete([Required][Range(1, int.MaxValue)] int id_facturacion)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";

            StructurePostBool pusuario = new DbPagos(AE.Decrypt(_config["Sqlconnetion"])).delete(id_facturacion);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (!pusuario.Data)
            {
                estructura.StatusCode = "P4";
                estructura.StatusMessage = "Could not be deleted";
                return estructura;
            }

            return pusuario;

        }


        ///<remarks>
        ///
        /// Se obtiene el nro de recibo 
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "nro de recibo",
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
        [Route("api/pagos/getNroRecibo")]
        public StructurePostLong getNroRecibo()
        {
            StructurePostLong estructura = new();
            estructura.Status = "400";

            var pusuario = new DbPagos(AE.Decrypt(_config["Sqlconnetion"])).getNroRecibo();
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
        /// Se obtiene la lista de pagos pendientes de aprobacion 
        ///       
        ///     
        /// RESPUESTAS          
        /// 
        /// Correcto:
        /// 
        ///     {
        ///         "Status" : "OK",
        ///         "Data" : "lista de pendientes",
        ///         "StatusCode":"0",
        ///         "StatusMessage" : "OK",
        ///         "InternalMessage":""
        ///     }             
        /// 
        /// Incorrecto:
        ///  
        ///     {
        ///         "Status" : "400",
        ///         "Data" : [],
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
        [Route("api/pagos/getPagosPendientes")]
        public StructureOutputPagosPendientes getPagosPendientes(int cantidad =100)
        {
            StructureOutputPagosPendientes estructura = new();
            estructura.Status = "400";

            var pusuario = new DbPagos(AE.Decrypt(_config["Sqlconnetion"])).getPagosPendientes(cantidad);
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
        /// Permite aprobar el pago realizado por el cliente        
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
        /// |200|400| 0 |P2|Could not update| |           
        /// 
        ///</remarks>
        ///<response code="401">It's not authorized</response>
        ///<response code="200">Success</response>
        ///<response code="400">Required fields</response>
        ///<response code="404">Route not found</response>


        [HttpPost]
        [Route("api/pagos/updateAprobacion/")]
        public StructurePostBool updateAprobacion(UpdatePagosAct usuario)
        {
            StructurePostBool estructura = new();
            estructura.Status = "400";
             
            StructurePostBool pusuario = new DbPagos(AE.Decrypt(_config["Sqlconnetion"])).updateAprobacion(usuario);
            if (!string.IsNullOrEmpty(pusuario.InternalMessage))
            {
                estructura.StatusCode = "D1";
                estructura.StatusMessage = "Database error ";
                estructura.InternalMessage = pusuario.InternalMessage;
                return estructura;
            }
            if (!pusuario.Data)
            {
                estructura.StatusCode = "P2";
                estructura.StatusMessage = "Could not update";
                return estructura;
            }

            return pusuario;

        }

    }
}
