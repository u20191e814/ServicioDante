using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades.Installs;
using ServicioWeb_Entidades.Solicitud_Atencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class Db_SolicitudAtencion_History
    {
        private readonly string conexioAlent;
        public Db_SolicitudAtencion_History(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

       

        internal void Insert(long id_SolicitudAtencion, string statusAtencion,  int fk_userAsig, string info_cliente, string materials = "", string comments = "")
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   insert into [SistemaWeb].[web].[SolicitudAtencion_History] (id_SolicitudAtencion, statusAtencion,fk_userAsig, info_cliente,materials,comments  ) "+ 
                        "values(@id_SolicitudAtencion, @statusAtencion, @fk_userAsig, @info_cliente, @materials, @comments) ";
                    var param = new DynamicParameters();
                    param.Add("@id_SolicitudAtencion", id_SolicitudAtencion);
                    param.Add("@statusAtencion", statusAtencion);
                    param.Add("@info_cliente", info_cliente);
                    param.Add("@fk_userAsig", fk_userAsig);
                    param.Add("@materials", materials);
                    param.Add("@comments", comments);
                    cn.Execute(squery, param);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Db_SolicitudAtencion_History.Insert: " + ex.Message);
            }
        }
        public StructureOutputSolicitudHistory getHistory(int id_SolicitudAtencion)
        {
            StructureOutputSolicitudHistory structurePostInt = new StructureOutputSolicitudHistory();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {

                    string squery = "     select statusAtencion, FORMAT( datecreated,'dd/MM/yyyy HH:mm') Fecha_creacion,[SistemaWeb]. web.getUserName(fk_userAsig) as usernameAsig, " +
                        "  info_cliente, materials, comments  from [SistemaWeb].[web].[SolicitudAtencion_History]  where id_SolicitudAtencion=@id_SolicitudAtencion order by  id_SolicitudAtencionHistory desc";
                    var param = new DynamicParameters();
                    param.Add("@id_SolicitudAtencion", id_SolicitudAtencion);

                    structurePostInt.Data = cn.Query<OutputSolicitudHistory>(squery, param).ToList();

                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OutputSolicitudHistory>();
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
