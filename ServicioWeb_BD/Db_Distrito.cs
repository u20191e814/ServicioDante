using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class Db_Distrito
    {
        private readonly string conexioAlent;
        public Db_Distrito(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructurePostLong crear(Districts distrito)
        {
            StructurePostLong structurePostInt = new StructurePostLong();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "     insert into [SistemaWeb].[web].[Distrito] (nombre, id_provincia) output inserted.id_distrito values (@nombre, @id_provincia) ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", distrito.nombre);
                    param.Add("@id_provincia", distrito.id_provincia);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);

                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool Update(Districts distrito)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    update [SistemaWeb].[web].[Distrito] set nombre=@nombre where id_distrito=@id_distrito ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", distrito.nombre);
                    param.Add("@id_distrito", distrito.id_distrito);


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

        public StructurePostBool delete(int id_distrito)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   delete from [SistemaWeb].[web].[Distrito] where id_distrito=@id_distrito ";
                    var param = new DynamicParameters();
                    param.Add("@id_distrito", id_distrito);
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
