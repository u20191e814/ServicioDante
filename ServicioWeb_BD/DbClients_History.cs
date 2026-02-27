using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class DbClients_History
    {
        private readonly string conexioAlent;
        public DbClients_History(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        internal void Insert(int Pk_clients, string Nombre_completo,string Dni, string Telefono, string Informacion_adicional,string Correo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  insert into [SistemaWeb].[web].[Clients_History] (Nombre_completo, Dni, Telefono, Informacion_adicional, Correo, FK_Client) " +

                        "  values(@Nombre_completo, @Dni, @Telefono, @Informacion_adicional, @Correo,@FK_Client) ";
                    var param = new DynamicParameters();
                    param.Add("@Nombre_completo", Nombre_completo);
                    param.Add("@Dni", Dni);
                    param.Add("@Telefono", Telefono);
                    param.Add("@Informacion_adicional", Informacion_adicional);
                    param.Add("@Correo",Correo);
                    param.Add("@FK_Client", Pk_clients);
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

    }
}
