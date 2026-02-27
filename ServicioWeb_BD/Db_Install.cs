using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Clientes;
using ServicioWeb_Entidades.Installs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServicioWeb_BD
{
    public class Db_Install
    {
        private readonly string conexioAlent;
        public Db_Install(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructurePostInt crear(InstallInput servicio)
        {
            StructurePostInt structurePostInt = new StructurePostInt();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  insert into  [SistemaWeb].[web].[Instalacion] (servicio,statusInstalacion,fk_user,nombre_completo,telefono,info_adicional,fk_userAsig, materials, comments  ) " +
                        "  output inserted.id_instalacion "+
                       "  values(@servicio, @statusInstalacion, @fk_user, @nombre_completo, @telefono, @info_adicional, @fk_userAsig, @materials, @comments ) ";
                    var param = new DynamicParameters();
                    param.Add("@servicio", servicio.servicio);
                    param.Add("@statusInstalacion", servicio.statusInstalacion);
                    param.Add("@fk_user", servicio.fk_user);
                    param.Add("@nombre_completo", servicio.nombre_completo);
                    param.Add("@telefono", servicio.telefono);
                    param.Add("@info_adicional", servicio.info_adicional);
                    param.Add("@fk_userAsig", servicio.fk_userAsig);
                    param.Add("@materials", servicio.materials);
                    param.Add("@comments", servicio.comments);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);
                    if (structurePostInt.Data > 0)
                    {
                        new Db_Install_History(conexioAlent).Insert(structurePostInt.Data, servicio.servicio, servicio.statusInstalacion, servicio.fk_user, servicio.nombre_completo, servicio.telefono,servicio.info_adicional, servicio.fk_userAsig, servicio.materials, servicio.comments);

                    }
                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        

        public StructureOutputInstalls getInstalls(string telOrNameclient, int pagina, int registros)
        {
            StructureOutputInstalls structurePostInt = new StructureOutputInstalls();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    if (string.IsNullOrEmpty(telOrNameclient) || telOrNameclient == "null")
                    {
                        string squery = string.Format(" exec [SistemaWeb].[web].[ups_GetInstalls] {0},{1} ", pagina, registros);

                        structurePostInt.Data = cn.Query<OutputInstall>(squery).ToList();
                    }
                    else
                    {
                        string squery = string.Format(" exec [SistemaWeb].[web].[ups_GetInstalls2] @telOrName,{0},{1} ", pagina, registros);
                        var param = new DynamicParameters();
                        telOrNameclient = "%" + telOrNameclient + "%";
                        param.Add("@telOrName", telOrNameclient);
                        structurePostInt.Data = cn.Query<OutputInstall>(squery, param).ToList();
                    }

                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OutputInstall>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool deleteInstall(int id_instalacion)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   delete from [SistemaWeb].[web].[Instalacion] where id_instalacion =@id_instalacion; "+
                        "  delete from SistemaWeb.web.Instalacion_History where id_instalacion = @id_instalacion; ";
                    var param = new DynamicParameters();
                    param.Add("@id_instalacion", id_instalacion);
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

        public StructurePostBool UpdateInstall(InstallUpdate servicio)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  update [SistemaWeb].[web].[Instalacion] set servicio=@servicio, statusInstalacion=@statusInstalacion, nombre_completo=@nombre_completo, "+
                        "  telefono = @telefono, info_adicional = @info_adicional, fk_userAsig = @fk_userAsig , materials = @materials, comments = @comments  where id_instalacion = @id_instalacion";
                    var param = new DynamicParameters();
                    param.Add("@servicio", servicio.servicio);
                    param.Add("@statusInstalacion", servicio.statusInstalacion);
                    param.Add("@nombre_completo", servicio.nombre_completo);
                    param.Add("@telefono", servicio.telefono);
                    param.Add("@info_adicional", servicio.info_adicional);
                    param.Add("@fk_userAsig", servicio.fk_userAsig);
                    param.Add("@materials", servicio.materials);
                    param.Add("@comments", servicio.comments);
                    param.Add("@id_instalacion", servicio.id_instalacion);
                    cn.Execute(squery, param);
                    structurePostInt.Data = true;

                    new Db_Install_History(conexioAlent).Insert(servicio.id_instalacion, servicio.servicio, servicio.statusInstalacion, servicio.fk_user, servicio.nombre_completo, servicio.telefono, servicio.info_adicional, servicio.fk_userAsig, servicio.materials, servicio.comments);

                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputInstallsStatus getInstallsByUser(int fk_user, string estado, int cantidad)
        {
            StructureOutputInstallsStatus structurePostInt = new StructureOutputInstallsStatus();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    
                    string squery = string.Format("   select top {0} id_instalacion, servicio, nombre_completo, telefono, info_adicional, FORMAT( datecreated,'dd/MM/yyyy HH:mm') Fecha_creacion, materials, comments\r\n  " +
                        " from [SistemaWeb].[web].[Instalacion] where fk_userAsig = @fk_userAsig and statusInstalacion like @statusInstalacion order by datecreated desc ", cantidad);
                    var param = new DynamicParameters();
                    
                    param.Add("@fk_userAsig", fk_user);
                    param.Add("@statusInstalacion", estado);
                    structurePostInt.Data = cn.Query<OutputInstallStatus>(squery, param).ToList();
                    

                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OutputInstallStatus>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool updateInstallStatus(InstallUpdateStatus usuario)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  update [SistemaWeb].[web].[Instalacion] set  statusInstalacion=@statusInstalacion  " +
                        "     where id_instalacion = @id_instalacion";
                    var param = new DynamicParameters();
                    
                    param.Add("@statusInstalacion", usuario.statusInstalacion); 
                    param.Add("@id_instalacion", usuario.id_instalacion);
                    cn.Execute(squery, param);
                    structurePostInt.Data = true;

                    new Db_Install_History(conexioAlent).Insert(usuario.id_instalacion, "", usuario.statusInstalacion, 0, "", "", "", 0, "", "");

                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool updateInstallStatusDetails(InstallUpdateStatusDetails usuario)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  update [SistemaWeb].[web].[Instalacion] set   statusInstalacion=@statusInstalacion , " +
                        "     materials = @materials, comments = @comments  where id_instalacion = @id_instalacion";
                    var param = new DynamicParameters();
                    param.Add("@statusInstalacion", usuario.statusInstalacion);
                    param.Add("@materials", usuario.materials);
                    param.Add("@comments", usuario.comments);
                    param.Add("@id_instalacion", usuario.id_instalacion);
                    cn.Execute(squery, param);
                    structurePostInt.Data = true;

                    new Db_Install_History(conexioAlent).Insert(usuario.id_instalacion,"", usuario.statusInstalacion, 0, "", "", "", 0, usuario.materials, usuario.comments);

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
