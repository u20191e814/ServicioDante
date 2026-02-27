using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades.B1;
using ServicioWeb_Entidades.Pagos;
using ServicioWeb_Entidades.Reportes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class Db_Reportes
    {
        private readonly string conexioAlent;
        public Db_Reportes(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

         

        public StructureOuput_Cobros getCobrosByDates(long unixInDate, long unixOutDate, int fk_user)
        {
            StructureOuput_Cobros structurePostInt = new StructureOuput_Cobros();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = " select ROW_NUMBER() OVER (ORDER BY id_facturacion) AS nroFila, FORMAT( fechaPago,'dd/MM/yyyy') fecha_cobro, s.servicio,  c.Nombre_completo,r.nombre+ ' - '+ p.nombre +' - '+ d.nombre+ ' - '+ co.nombre ubicacion, monto , nameMonth, nameYear, nameTipoPago, nroRecibo, isnull( SistemaWeb.web.getUserName(f.Fk_user), 'Cliente') 'cobrador', f.Fk_user , isnull( SistemaWeb.web.getUserName(f.Fk_userApproved), '-') 'aprobado'  \r\n " +
                        " from [SistemaWeb].[web].[Facturacion] f inner join SistemaWeb.web.Servicio s on (f.id_servicio=s.id_servicio)\r\n  inner join SistemaWeb.web.Clients c on (s.id_client = c.Pk_clients)\r\n  inner join SistemaWeb.web.Comunidad co on (s.id_comunidad = co.id_comunidad)\r\n  inner join SistemaWeb.web.Distrito d on (d.id_distrito = co.id_distrito)\r\n  " +
                        "inner join SistemaWeb.web.Provincia p on (p.id_provincia = d.id_provincia)\r\n  inner join SistemaWeb.web.Region r on (p.id_region = p.id_region)\r\n  " +
                        "where f.fechaPago between @inicio and @fin ";
                    if (fk_user >= 0 )
                    {
                        squery = squery + " and f.Fk_user = " + fk_user;
                    }
                    DateTime start = FromUnixTimeMilliseconds(unixInDate);
                    DateTime end = FromUnixTimeMilliseconds(unixOutDate);
                    var param = new DynamicParameters();
                    param.Add("@inicio", start.Date);
                    param.Add("@fin", end.Date);

                    structurePostInt.Data = cn.Query<cobros>(squery, param).ToList();


                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<cobros>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }
        public static DateTime FromUnixTimeMilliseconds(long unixTimeMilliseconds)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds);
            return dateTimeOffset.UtcDateTime.AddHours(-5);
        }
        public StructureOuputRep_Global getGlobal ()
        {
            StructureOuputRep_Global structurePostInt = new StructureOuputRep_Global();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = " [SistemaWeb].web.[usp_getResportGeneral] ";
                     
                   
                    structurePostInt.Data = cn.QueryFirstOrDefault<Global>(squery);


                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new Global();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputGlobalChart getServices()
        {
            StructureOutputGlobalChart structurePostInt = new StructureOutputGlobalChart();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   select servicio 'serie' , COUNT(1) 'cantidad' from [SistemaWeb].[web].[Servicio]\r\n  group by servicio ";


                    structurePostInt.Data = cn.Query<SerieChart>(squery).ToList();


                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<SerieChart>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputService_Status getServicesAndStatus()
        {
            StructureOutputService_Status structurePostInt = new StructureOutputService_Status();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    select servicio, estado, COUNT(1) 'cantidad' from [SistemaWeb].[web].[Servicio] \r\n  group by servicio, estado ";


                    structurePostInt.Data = cn.Query<Service_Status>(squery).ToList();


                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<Service_Status>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputGlobalChart getStatus()
        {
            StructureOutputGlobalChart structurePostInt = new StructureOutputGlobalChart();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    select estado 'serie' , COUNT(1) 'cantidad' from [SistemaWeb].[web].[Servicio]\r\n  group by estado ";


                    structurePostInt.Data = cn.Query<SerieChart>(squery).ToList();


                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<SerieChart>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }
    }
}
