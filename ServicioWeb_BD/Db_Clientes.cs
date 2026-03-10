using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Clientes;
using ServicioWeb_Entidades.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class Db_Clientes
    {
        private readonly string conexioAlent;
        public Db_Clientes(string sqlconexion)
        {
            conexioAlent = sqlconexion;           

        }

        public StructurePostInt AddClients(ClientInput usuario)
        {
            StructurePostInt structurePostInt = new StructurePostInt();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  insert into [SistemaWeb].[web].[Clients] (Nombre_completo, Dni, Telefono, Informacion_adicional, Correo) "+
                        "  output inserted.Pk_clients "+
                        "  values(@Nombre_completo, @Dni, @Telefono, @Informacion_adicional, @Correo) ";
                    var param = new DynamicParameters();
                    param.Add("@Nombre_completo", usuario.Nombre_completo);
                    param.Add("@Dni", usuario.Dni);
                    param.Add("@Telefono", usuario.Telefono);
                    param.Add("@Informacion_adicional", usuario.Informacion_adicional);
                    param.Add("@Correo", usuario.Correo);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);
                    if (structurePostInt.Data>0)
                    {
                        new DbClients_History(conexioAlent).Insert(structurePostInt.Data, usuario.Nombre_completo, usuario.Dni, usuario.Telefono, usuario.Informacion_adicional, usuario.Correo);

                    }
                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }
         
        public StructurePostInt GetClientsByDni(string dni)
        {
            StructurePostInt structurePostInt = new StructurePostInt();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  select ISNULL( Pk_clients,0) from  [SistemaWeb].[web].[Clients] where Dni like @dni";
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

        public StructurePostBool UpdateClient(ClientUpdate usuario)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  update [SistemaWeb].[web].[Clients] set Telefono=@Telefono, Informacion_adicional=@Informacion_adicional, Correo=@Correo,Nombre_completo=@Nombre_completo, Dni=@Dni  where Pk_clients=@Pk_clients ";
                    var param = new DynamicParameters();
                    param.Add("@Telefono", usuario.Telefono);
                    param.Add("@Informacion_adicional", usuario.Informacion_adicional);
                    param.Add("@Correo", usuario.Correo);
                    param.Add("@Pk_clients", usuario.Pk_clients);
                    param.Add("@Nombre_completo", usuario.Nombre_completo);
                    param.Add("@Dni", usuario.Dni);

                    cn.Execute(squery, param);
                    new DbClients_History(conexioAlent).Insert(usuario.Pk_clients, usuario.Nombre_completo, usuario.Dni, usuario.Telefono, usuario.Informacion_adicional, usuario.Correo);

                }
                structurePostInt.Data = true;
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputClients ObtenerClients( string dni, int pagina, int registros)
        {
            StructureOutputClients structurePostInt = new StructureOutputClients();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    if (string.IsNullOrEmpty(dni)|| dni =="null")
                    {
                        string squery = string.Format(" exec [SistemaWeb].web.[ups_GetListClients] {0},{1} ", pagina, registros);

                        structurePostInt.Data = cn.Query<OutputClient>(squery).ToList();
                    }
                    else
                    {
                        string squery = string.Format(" exec [SistemaWeb].web.[ups_GetListClients2] @dni,{0},{1} ", pagina, registros);
                        var param = new DynamicParameters();
                        dni = "%" + dni + "%";
                        param.Add("@dni", dni);
                        structurePostInt.Data = cn.Query<OutputClient>(squery,param).ToList();
                    }
                    
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

        public StructurePostBool deleteClient(int id_client)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = " delete from SistemaWeb.web.Facturacion where id_servicio  in (select id_servicio from [SistemaWeb].[web].[Servicio] where  id_client=@id_client);\r\n" +
                        "  delete from [SistemaWeb].[web].[Servicio] where  id_client=@id_client\r\n ;" +
                        " delete from [SistemaWeb].[web].[Clients] where Pk_clients = @id_client ";
                    var param = new DynamicParameters();
                    param.Add("@id_client", id_client);
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



        public ServicioWeb_Entidades.Clientes. ClientOutput logincl(string Dni, string password)
        {
            ClientOutput structurePostInt = new ClientOutput();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "     select * from [SistemaWeb].[web].[Clients] where Dni like @Dni and password like @password ";
                    var param = new DynamicParameters();
                    param.Add("@Dni", Dni);
                    param.Add("@password", password);
                    structurePostInt.Data = cn.QueryFirstOrDefault<ClientUpdate>(squery, param);

                     
                }

            }
            catch (Exception ex)
            {
                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool createLogin(string dni, string clave)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                int cantidad = GetLoginCount(dni, conexioAlent);
                if (cantidad == 0)
                {
                    structurePostInt.InternalMessage = "El cliente no tiene servicios registrados.";
                    return structurePostInt;
                }
                cantidad = GetLoginCountContrasena(dni, conexioAlent);
                if (cantidad == 0)
                {
                    structurePostInt.InternalMessage = "El cliente ya esta registrado y tiene una contraseña creada";
                    return structurePostInt;
                }
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   update [SistemaWeb].[web].[Clients] set password=@password where Dni=@Dni  ";
                    var param = new DynamicParameters();
                    param.Add("@password", clave);
                    param.Add("@Dni", dni);
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

        public int GetLoginCount(string dni, string conexioAlent)
        {
            try
            {
                int contad = 0;
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   select ISNULL( count(1), 0) from [SistemaWeb].[web].[Clients]  where Dni=@Dni   ";
                    var param = new DynamicParameters();
                    param.Add("@Dni", dni);
                    contad = cn.QueryFirstOrDefault<int>(squery, param);
                }

                return contad;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public int GetLoginCountContrasena(string dni, string conexioAlent)
        {
            try
            {
                int contad = 0;
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   select ISNULL( count(1), 0) from [SistemaWeb].[web].[Clients]  where Dni=@Dni and password is null  ";
                    var param = new DynamicParameters();
                    param.Add("@Dni", dni);
                    contad = cn.QueryFirstOrDefault<int>(squery, param);
                }

                return contad;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public StructurePostBool updateloginClient(string dni, string clave, string nuevaclave)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                int cantidad = GetLoginCount(dni, conexioAlent);
                if (cantidad == 0)
                {
                    structurePostInt.InternalMessage = "El cliente no tiene servicios registrados.";
                    return structurePostInt;
                }
                cantidad = GetLoginCountContrasenaActual(dni, clave, conexioAlent);
                if (cantidad == 0)
                {
                    structurePostInt.InternalMessage = "La contraseña actual no coincide";
                    return structurePostInt;
                }
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   update [SistemaWeb].[web].[Clients] set password=@password where Dni=@Dni  ";
                    var param = new DynamicParameters();
                    param.Add("@password", nuevaclave);
                    param.Add("@Dni", dni);
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

        public int GetLoginCountContrasenaActual(string dni, string clave, string conexioAlent)
        {
            try
            {
                int contad = 0;
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   select ISNULL( count(1), 0) from [SistemaWeb].[web].[Clients]  where Dni=@Dni and password =@password  ";
                    var param = new DynamicParameters();
                    param.Add("@Dni", dni);
                    param.Add("@password", clave);
                    contad = cn.QueryFirstOrDefault<int>(squery, param);
                }

                return contad;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public StructurePostBool ResetloginClient(long id_client)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = string.Format("  update [SistemaWeb].[web].[Clients] set password=null where Pk_clients ={0}", id_client);
                    cn.Execute(squery);
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
