using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Clientes;
using ServicioWeb_Entidades.Deduccion;
using ServicioWeb_Entidades.Solicitud_Atencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class Db_deduccion
    {
        private readonly string conexioAlent;
        public Db_deduccion(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructurePostInt crear(deduccionInput deduccion)
        {
            StructurePostInt structurePostInt = new StructurePostInt();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   insert into [SistemaWeb].[web].[Deduccion]  (id_servicio,mes,nameMonth, nameYear,Fk_user,monto ) " +
                        "  output inserted.id_deduccion "+
                        "  values(@id_servicio, @mes, @nameMonth, @nameYear, @Fk_user,@monto)";
                    var param = new DynamicParameters();
                    param.Add("@id_servicio", deduccion.id_servicio);
                    param.Add("@mes", deduccion.mes);
                    param.Add("@nameMonth", deduccion.nameMonth);
                    param.Add("@nameYear", deduccion.nameYear);
                    param.Add("@Fk_user", deduccion.Fk_user);
                    param.Add("@monto", deduccion.monto);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);
                     
                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool deleteDeduccion(int id_deduccion)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   delete from [SistemaWeb].[web].[Deduccion] where id_deduccion=@id_deduccion ";
                    var param = new DynamicParameters();
                    param.Add("@id_deduccion", id_deduccion);
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

        public StructureOutputDeduccion getDeducciones(int id_servicio)
        {
            StructureOutputDeduccion structurePostInt = new StructureOutputDeduccion();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {

                    string squery = string.Format("   select top 50 id_deduccion, nameMonth,mes, nameYear,monto, SistemaWeb.web.getUserName(Fk_user) as usuario, FORMAT(dateinclude,'dd-MM-yyyy HH:mm:ss') fecha\r\n  " +
                        "from [SistemaWeb].[web].[Deduccion] where id_servicio=@id_servicio \r\n  order by id_deduccion desc ");
                    var param = new DynamicParameters();

                    param.Add("@id_servicio", id_servicio);
                     
                    structurePostInt.Data = cn.Query<OutputDeduccion>(squery, param).ToList();


                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OutputDeduccion>();
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
