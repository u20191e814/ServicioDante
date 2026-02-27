using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Notificacion;
using ServicioWeb_Entidades.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class DbNotificacion
    {
        private readonly string conexioAlent;
        public DbNotificacion(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructurePostLong createNotificacion(inputNotificacion noti)
        {
            StructurePostLong structurePostInt = new StructurePostLong();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "     insert into [SistemaWeb].[web].[Notificacion] (titulo, mensaje, fecha, id_cliente, isready, type)\r\n" +
                        "  output inserted.id_notificacion \r\n  " +
                        "values (@titulo, @mensaje, @fecha, @id_cliente, 0, @type) ";
                    var param = new DynamicParameters();
                    param.Add("@titulo", noti.titulo);
                    param.Add("@mensaje", noti.mensaje);
                    param.Add("@fecha", noti.fecha);
                    param.Add("@id_cliente", noti.id_cliente);
                    param.Add("@type", noti.type);
                    structurePostInt.Data = cn.QueryFirstOrDefault<long>(squery, param);

                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool deleteNotificacion(int id_notificacion)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   delete from [SistemaWeb].[web].[Notificacion] where id_notificacion=@id_notificacion ";
                    var param = new DynamicParameters();
                    param.Add("@id_notificacion", id_notificacion);
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

        public StructureOutputNotification getNotifications(int id_client)
        {
            StructureOutputNotification structurePostInt = new StructureOutputNotification();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "     select top 6 * from [SistemaWeb].[web].[Notificacion] where id_cliente =@id_cliente order by id_notificacion desc ";
                    var param = new DynamicParameters();
                    param.Add("@id_cliente", id_client);
                    structurePostInt.Data = cn.Query<OutputNotification>(squery, param).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OutputNotification>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool updateNotificacion(updateNotificacion notificacion)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   update [SistemaWeb].[web].[Notificacion] set isready=@isready where id_notificacion=@id_notificacion ";
                    var param = new DynamicParameters();
                    param.Add("@id_notificacion", notificacion.id_notificacion);
                    param.Add("@isready", notificacion.isready);
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
