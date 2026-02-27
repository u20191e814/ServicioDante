using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Pagos;
using ServicioWeb_Entidades.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class Db_Years
    {
        private readonly string conexioAlent;
        public Db_Years(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructurePostLong crear(Years region)
        {
            StructurePostLong structurePostInt = new StructurePostLong();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   insert into [SistemaWeb].[web].[Anio] (nombre,status) output inserted.PK_Anio values (@nombre,@status) ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", region.nombre);
                    param.Add("@status", region.status);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);

                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool Update(Years anio)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    update [SistemaWeb].[web].[Anio] set nombre=@nombre, status=@status where PK_Anio=@PK_Anio ";
                    var param = new DynamicParameters();
                    param.Add("@nombre", anio.nombre);
                    param.Add("@status", anio.status);
                    param.Add("@PK_Anio", anio.PK_Anio);


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

        public StructurePostBool delete(int PK_Anio)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = " delete from  [SistemaWeb].[web].[Anio]  where PK_Anio=@PK_Anio ";
                    var param = new DynamicParameters();
                    param.Add("@PK_Anio", PK_Anio);
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

        public StructureOutputYears getYears()
        {
            StructureOutputYears structurePostInt = new StructureOutputYears();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = " select * from [SistemaWeb].[web].[Anio]   ";

                    structurePostInt.Data = cn.Query<Years>(squery).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<Years>();
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
