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
    public  class Db_Region
    {
        private readonly string conexioAlent;
        public Db_Region(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructurePostLong crear(Region region)
        {
            StructurePostLong structurePostInt = new StructurePostLong();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   insert into [SistemaWeb].[web].[Region] (nombre) output inserted.id_region values (@nombre) ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", region.nombre);                     
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);
                    
                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool Update(Region region)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    update [SistemaWeb].[web].[Region] set nombre=@nombre where id_region=@id_region ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", region.nombre);
                    param.Add("@id_region", region.id_region);
                   

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

        public StructurePostBool delete(int id_region)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = " delete from  [SistemaWeb].[web].[Region]  where id_region=@id_region ";
                    var param = new DynamicParameters();
                    param.Add("@id_region", id_region);
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
