using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ServicioWeb_Entidades.Usuarios;
using ServicioWeb_Entidades.Clientes;

namespace ServicioWeb_BD
{
    public  class DbUsuarios
    {
        private readonly string conexioAlent;
        public DbUsuarios(string sqlconexion)
        {

            conexioAlent = sqlconexion;
            //conexioAlent = Convertconexion.descifrar(sqlconexion);

        }

        public StructurePostInt GetUserByDni(string dni)
        {
            StructurePostInt structurePostInt = new StructurePostInt();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  select ISNULL( Pk_user,0) from [SistemaWeb].[web].[Users] where Dni like @dni";
                    var param = new DynamicParameters();
                    param.Add("@dni", dni);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);
                }
                
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostInt GetUserByCorreo(string correo)
        {
            StructurePostInt structurePostInt = new StructurePostInt();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  select ISNULL( Pk_user,0) from [SistemaWeb].[web].[Users] where Correo like @Correo ";
                    var param = new DynamicParameters();
                    param.Add("@Correo", correo);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);
                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostInt AddUser(UserInput usuario)
        {
            StructurePostInt structurePostInt = new StructurePostInt();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  insert into [SistemaWeb].[web].[Users] (Nombre, Apellidos, Dni, Correo, Clave) output inserted.Pk_user values (@Nombre, @Apellidos, @Dni, @Correo, @Clave) ";
                    var param = new DynamicParameters();
                    param.Add("@Nombre", usuario.Nombre);
                    param.Add("@Apellidos", usuario.Apellidos);
                    param.Add("@Dni", usuario.Dni);
                    param.Add("@Correo", usuario.Correo);
                    param.Add("@Clave", usuario.Clave);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);
                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        
        public StructureOutputUsers ObtenerUsuarios()
        {
            StructureOutputUsers structurePostInt = new StructureOutputUsers();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   select *, FORMAT( Fecha_creado,'dd/MM/yyyy HH:mm') Fecha_creado , FORMAT(UltimaConexion,'dd/MM/yyyy HH:mm' ) UltimaConexion  from [SistemaWeb].[web].[Users] ";
                    
                    structurePostInt.Data = cn.Query<OutputUsers>(squery).ToList();
                }
                if (structurePostInt.Data==null)
                {
                    structurePostInt.Data = new List<OutputUsers>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputUserLogin Login(string correo, string clave)
        {
            StructureOutputUserLogin structurePostInt = new StructureOutputUserLogin();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "     select * from [SistemaWeb].[web].[Users] where Correo like @correo and Clave like @clave ";
                    var param = new DynamicParameters();
                    param.Add("@correo", correo);
                    param.Add("@clave", clave);
                    structurePostInt.Data = cn.QueryFirstOrDefault<OutputUsers>(squery,param);
                   
                    if (structurePostInt.Data!=null)
                    {
                        if (structurePostInt.Data.Habilitado)
                        {
                            string squery1 = "update   [SistemaWeb].[web].[Users] set UltimaConexion=@UltimaConexion where Pk_user=@Pk_user";
                            var param2 = new DynamicParameters();
                            param2.Add("@UltimaConexion", DateTime.Now);
                            param2.Add("@Pk_user", structurePostInt.Data.Pk_user);
                            cn.Execute(squery1, param2);
                        }
                       
                    }
                }
                
            }
            catch (Exception ex)
            {
                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

       

        public StructurePostBool UpdateUser(UserUpdate usuario)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   update [SistemaWeb].[web].[Users] set Habilitado=@habilitado , Administrador=@Administrador,Tecnico=@Tecnico where Pk_user=@Pk_user ";
                    var param = new DynamicParameters();
                    param.Add("@habilitado", usuario.Habilitado);
                    param.Add("@Administrador", usuario.Administrador);
                    param.Add("@Pk_user", usuario.Pk_user);
                    param.Add("@Tecnico", usuario.Tecnico);
                   
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

        public StructurePostBool deleteUser(int pk_user)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    delete from [SistemaWeb].[web].[Users] where Pk_user=@Pk_user ";
                    var param = new DynamicParameters();
                   
                    param.Add("@Pk_user", pk_user);

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
