using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Pagos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class DbPagos
    {
        private readonly string conexionPagos;
        public DbPagos(string sqlconexion)
        {
            conexionPagos = sqlconexion;

        }

        public StructurePostBool delete(int id_facturacion)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexionPagos))
                {
                    string squery = "      delete from  [SistemaWeb].[web].[Facturacion]  where id_facturacion=@id_facturacion ";
                    var param = new DynamicParameters();
                    param.Add("@id_facturacion", id_facturacion);
                    cn.Execute(squery, param);
                }
                structurePostInt.Data = true;
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputPagos getFacturacion(int id_servicio)
        {
            StructureOutputPagos structurePostInt = new StructureOutputPagos();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexionPagos))
                {
                    string squery = "  select top 20 id_facturacion,  FORMAT(fechaPago, 'dd-MM-yyyy') fechaPago, comprobante, format(fechaIngresado, 'dd-MM-yyyy') fechaIngresado, FK_Anio, mes, tipoPago, monto, nameMonth, nameYear, nameTipoPago,details, nroRecibo, isApproved, servicio from [SistemaWeb].[web].[Facturacion] where id_servicio=@id_servicio  order by nameYear,mes desc ";
                    var param = new DynamicParameters();
                    param.Add("@id_servicio", id_servicio);
                    structurePostInt.Data = cn.Query<OutputPagos>(squery,param).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OutputPagos>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public List<Facturacion> GetFacturacion(int id_servicio, int mes, string nameYear)
        {
            try
            {
                List<Facturacion> lista = null;
                using (SqlConnection cn = new SqlConnection(conexionPagos))
                {
                    string squery =string.Format( "  select f.monto, s.precio from [SistemaWeb].[web].[Facturacion] f " +
                        "inner join SistemaWeb.web.Servicio s on (s.id_servicio = f.id_servicio) \r\n " +
                        " where f. id_servicio ={0} and mes = {1} and nameYear like '{2}'", id_servicio, mes, nameYear);
                    lista = cn.Query<Facturacion>(squery).ToList();
                }
                return lista;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public StructurePostLong getNroRecibo()
        {
            StructurePostLong structurePostInt = new StructurePostLong();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexionPagos))
                {
                    string squery = "   select  ISNULL( max (nroRecibo), 0) +1 from [SistemaWeb].[web].[Facturacion]  ";

                    structurePostInt.Data = cn.QueryFirstOrDefault<long>(squery);
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = 0;
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputPagosPendientes getPagosPendientes(int cantidad)
        {
            StructureOutputPagosPendientes structurePostInt = new StructureOutputPagosPendientes();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexionPagos))
                {
                    string squery = string.Format(" select top {0} ROW_NUMBER() OVER (ORDER BY nroRecibo DESC) AS rowNum, s.id_servicio, FORMAT(fechaPago, 'dd-MM-yyyy') fechaPago, nroRecibo, details, monto, direccion, c.Nombre_completo , r.nombre + ' - '+ p.nombre + ' - '+ d.nombre +' - '+ co.nombre ubicacion" +
                        " from (\r\n  select  id_servicio, fechaPago, nroRecibo, max( details) details, sum(monto) monto from [SistemaWeb].[web].[Facturacion] f with (nolock)\r\n  where f.isApproved = 0\r\n  group by id_servicio, fechaPago, nroRecibo ) fs inner join SistemaWeb.web.Servicio s with (nolock)\r\n " +
                        "  on (s.id_servicio= fs.id_servicio) inner join SistemaWeb.web.Clients c with (nolock)  on (s.id_client=c.Pk_clients)\r\n   inner join SistemaWeb.web.Comunidad co with (nolock) on (co.id_comunidad = s.id_comunidad)\r\n   inner join SistemaWeb.web.Distrito d with (nolock) on (d.id_distrito = co.id_distrito)\r\n " +
                        "  inner join SistemaWeb.web.Provincia p with (nolock) on (p.id_provincia = d.id_provincia)\r\n   inner join SistemaWeb.web.Region r with (nolock) on (r.id_region = p.id_region)\r\n   order by nroRecibo desc   ", cantidad);

                    structurePostInt.Data = cn.Query<PagosPendientes>(squery).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<PagosPendientes>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputYears getYears()
        {
            StructureOutputYears structurePostInt = new StructureOutputYears();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexionPagos))
                {
                    string squery = " select * from [SistemaWeb].[web].[Anio]  where status =1 ";
                     
                    structurePostInt.Data = cn.Query<Years>(squery).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<Years>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }
        public StructurePostBool InsertBilling(BillInput usuario)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexionPagos))
                {
                    string squery = "    insert into [SistemaWeb].[web].[Facturacion]  (id_servicio, fechaPago, comprobante, details, FK_Anio, mes, tipoPago, monto, nameMonth,nameYear,nameTipoPago,Fk_user, nroRecibo, isApproved, servicio)\r\n " +
                        " values (@id_servicio, @fechaPago, @comprobante, @details, @FK_Anio, @mes, @tipoPago, @monto, @nameMonth,@nameYear,@nameTipoPago, @Fk_user,@nroRecibo, @isApproved, @servicio) ";
                    var param = new DynamicParameters();
                    param.Add("@comprobante", usuario.comprobante);
                    param.Add("@id_servicio", usuario.id_servicio);
                    param.Add("@fechaPago", usuario.fechaPago);
                    param.Add("@details", usuario.details);
                    param.Add("@FK_Anio", usuario.FK_Anio);
                    param.Add("@mes", usuario.mes);
                    param.Add("@tipoPago", usuario.tipoPago);
                    param.Add("@monto", usuario.monto);
                    param.Add("@nameMonth", usuario.nameMonth);
                    param.Add("@nameYear", usuario.nameYear);
                    param.Add("@nameTipoPago", usuario.nameTipoPago);
                    param.Add("@Fk_user", usuario.Fk_user);
                    param.Add("@nroRecibo", usuario.nroRecibo);
                    param.Add("@isApproved", usuario.isApproved);
                    param.Add("@servicio", usuario.servicio);
                    cn.Execute(squery, param);
                }
                structurePostInt.Data = true;
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool updateAprobacion(UpdatePagosAct usuario)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexionPagos))
                {
                    string squery = "  update [SistemaWeb].[web].[Facturacion] set isApproved = @isApproved, Fk_userApproved = @Fk_userApproved where nroRecibo = @nroRecibo; ";
                    if (!usuario.isApproved)
                    {
                        squery = " declare @titulo nvarchar (50) = 'Pago rechazado';\r\n " +
                        "declare @mensaje nvarchar (max);\r\n declare @idclient int;\r\n " +
                        "select top 1 @idclient=s.id_client,  @mensaje = 'El pago fue rechazado de la fecha ' +FORMAT(fechaPago, 'dd-MM-yyyy') + ' con detalle ' +details + ' ubicado en: '+ s.direccion + '. Comunicate con nosotros. Preguntar por '+ SistemaWeb.web.getUserName(@Fk_userApproved) \r\n" +
                        " from [SistemaWeb].[web].[Facturacion] f with (nolock) inner join SistemaWeb.web.Servicio s with (nolock) on (f.id_servicio = s.id_servicio)\r\n where nroRecibo = @nroRecibo; \r\n insert into SistemaWeb.web.Notificacion (titulo, mensaje, fecha, id_cliente, type) \r\n" +
                        " values (@titulo, @mensaje, GETDATE(), @idclient ,  'Warning'); \r\n " +
                        "delete from [SistemaWeb].[web].[Facturacion] where nroRecibo =@nroRecibo;";
                    }
                    else
                    {
                        squery = squery + " declare @titulo nvarchar (50) = 'Pago aprobado';\r\n " +
                        "declare @mensaje nvarchar (max);\r\n declare @idclient int;\r\n " +
                        "select top 1 @idclient=s.id_client,  @mensaje = 'El pago fue aprobado de la fecha ' +FORMAT(fechaPago, 'dd-MM-yyyy') + ' con detalle ' +details + ' ubicado en: '+ s.direccion " +
                        " from [SistemaWeb].[web].[Facturacion] f with (nolock) inner join SistemaWeb.web.Servicio s with (nolock) on (f.id_servicio = s.id_servicio)\r\n where nroRecibo = @nroRecibo; \r\n insert into SistemaWeb.web.Notificacion (titulo, mensaje, fecha, id_cliente, type) \r\n" +
                        " values (@titulo, @mensaje, GETDATE(), @idclient ,  'Success'); \r\n ";
                         
                    }
                        var param = new DynamicParameters();
                    param.Add("@isApproved", usuario.isApproved);
                    param.Add("@Fk_userApproved", usuario.Fk_userApproved);
                    param.Add("@nroRecibo", usuario.nroRecibo);
                    
                    cn.Execute(squery, param);
                }
                structurePostInt.Data = true;
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }
    }
}
