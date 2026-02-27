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
    public  class Db_Comunidad
    {
        private readonly string conexioAlent;
        public Db_Comunidad(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructurePostLong crear(Comunities comunidad)
        {
            StructurePostLong structurePostInt = new StructurePostLong();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "     insert into [SistemaWeb].[web].[Comunidad] (nombre, id_distrito) output inserted.id_comunidad values (@nombre, @id_distrito) ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", comunidad.nombre);
                    param.Add("@id_distrito", comunidad.id_distrito);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);

                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool Update(Comunities comunidad)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    update [SistemaWeb].[web].[Comunidad] set nombre=@nombre where id_comunidad=@id_comunidad ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", comunidad.nombre);
                    param.Add("@id_comunidad", comunidad.id_comunidad);


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

        public StructurePostBool delete(int id_comunidad)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   delete from [SistemaWeb].[web].[Comunidad] where id_comunidad=@id_comunidad ";
                    var param = new DynamicParameters();
                    param.Add("@id_comunidad", id_comunidad);
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
