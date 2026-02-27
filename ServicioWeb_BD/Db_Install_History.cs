using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades.Clientes;
using ServicioWeb_Entidades.Installs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServicioWeb_BD
{
    public  class Db_Install_History
    {
        private readonly string conexioAlent;
        public Db_Install_History(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        internal void Insert(long data, string servicio, string statusInstalacion, int fk_user, string nombre_completo, string telefono, string info_adicional, int fk_userAsig, string materials="",string comments="" )
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   insert into [SistemaWeb].[web].[Instalacion_History] (id_instalacion,servicio, statusInstalacion,fk_user,nombre_completo, telefono,info_adicional,fk_userAsig,materials, comments) "+
                        "  values(@id_instalacion, @servicio, @statusInstalacion, @fk_user, @nombre_completo, @telefono, @info_adicional, @fk_userAsig, @materials, @comments) ";
                    var param = new DynamicParameters();
                    param.Add("@nombre_completo", nombre_completo);                   
                    param.Add("@telefono", telefono);
                    param.Add("@info_adicional", info_adicional);
                    param.Add("@id_instalacion", data);
                    param.Add("@servicio", servicio);
                    param.Add("@statusInstalacion", statusInstalacion);
                    param.Add("@fk_user", fk_user); 
                    param.Add("@fk_userAsig", fk_userAsig);
                    param.Add("@materials", materials);
                    param.Add("@comments", comments);
                    cn.Execute(squery, param);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DbClients_History.Insert: " + ex.Message);
            }
        }
        public StructureOutputClients ObtenerClients(int Pk_clients)
        {
            StructureOutputClients structurePostInt = new StructureOutputClients();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {

                    string squery = "   select * from [SistemaWeb].[web].[Clients_History] where FK_Client=@FK_Client order by  Pk_Clients_History desc";
                    var param = new DynamicParameters();
                    param.Add("@FK_Client", Pk_clients);

                    structurePostInt.Data = cn.Query<OutputClient>(squery, param).ToList();

                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OutputClient>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputInstallHistory getInstallsHistory(int id_instalacion)
        {
            StructureOutputInstallHistory structurePostInt = new StructureOutputInstallHistory();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {

                    string squery = "     select servicio, statusInstalacion, nombre_completo, telefono,  FORMAT(dateinclude,'dd-MM-yyyy HH:mm:ss') Fecha_creacion, info_adicional, materials, comments, [SistemaWeb].web.getUserName(fk_userAsig) usernameAsig" +
                        " from [SistemaWeb].[web].[Instalacion_History]\r\n  where id_instalacion=@id_instalacion order by  id_instalacionHistory desc";
                    var param = new DynamicParameters();
                    param.Add("@id_instalacion", id_instalacion);

                    structurePostInt.Data = cn.Query<OutputInstallHistory>(squery, param).ToList();

                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OutputInstallHistory>();
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
