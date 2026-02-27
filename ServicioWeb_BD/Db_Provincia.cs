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
    public class Db_Provincia
    {
        private readonly string conexioAlent;
        public Db_Provincia(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructurePostLong crear(Provinces provincia)
        {
            StructurePostLong structurePostInt = new StructurePostLong();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "     insert into [SistemaWeb].[web].[Provincia] (nombre, id_region) output inserted.id_provincia values (@nombre, @id_region) ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", provincia.nombre);
                    param.Add("@id_region", provincia.id_region);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);

                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool Update(Provinces provincia)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    update [SistemaWeb].[web].[Provincia] set nombre=@nombre where id_provincia=@id_provincia ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", provincia.nombre);
                    param.Add("@id_provincia", provincia.id_provincia);


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

        public StructurePostBool delete(int id_provincia)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   delete from [SistemaWeb].[web].[Provincia] where id_provincia=@id_provincia ";
                    var param = new DynamicParameters();
                    param.Add("@id_provincia", id_provincia);
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
