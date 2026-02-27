using Dapper;
using Microsoft.Data.SqlClient;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Installs;
using ServicioWeb_Entidades.Solicitud_Atencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServicioWeb_BD
{
    public  class Db_SolicitudAtencion
    {
        private readonly string conexioAlent;
        public Db_SolicitudAtencion(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructurePostLong crear(SolicitudInput servicio)
        {
            StructurePostLong structurePostInt = new StructurePostLong();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    insert into [SistemaWeb].[web].[SolicitudAtencion] (statusAtencion,id_servicio,fk_user,fk_userAsig,info_cliente, materials,comments    ) "+
                        "output inserted.id_SolicitudAtencion  values(@statusAtencion, @id_servicio, @fk_user, @fk_userAsig, @info_cliente, @materials, @comments) ";
                    var param = new DynamicParameters(); 
                    param.Add("@statusAtencion", servicio.statusAtencion);
                    param.Add("@fk_user", servicio.fk_user);
                    param.Add("@id_servicio", servicio.id_servicio);
                    param.Add("@info_cliente", servicio.info_cliente);
                    param.Add("@fk_userAsig", servicio.fk_userAsig);
                    param.Add("@materials", servicio.materials);
                    param.Add("@comments", servicio.comments);
                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);
                    if (structurePostInt.Data > 0)
                    {
                        new Db_SolicitudAtencion_History(conexioAlent).Insert(structurePostInt.Data, servicio.statusAtencion,  servicio.fk_userAsig, servicio.info_cliente, servicio.materials, servicio.comments);

                    }
                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        

        public StructurePostBool UpdateAtencion(SolicitudUpdate servicio)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "    update [SistemaWeb].[web].[SolicitudAtencion] set  statusAtencion=@statusAtencion, fk_userAsig=@fk_userAsig, info_cliente=@info_cliente, "+
                        "  materials = @materials, comments = @comments where id_SolicitudAtencion = @id_SolicitudAtencion";

                    var param = new DynamicParameters();
                    param.Add("@statusAtencion", servicio.statusAtencion);
                    param.Add("@info_cliente", servicio.info_cliente);
                    param.Add("@fk_userAsig", servicio.fk_userAsig);
                    param.Add("@materials", servicio.materials);
                    param.Add("@comments", servicio.comments);
                    param.Add("@id_SolicitudAtencion", servicio.id_SolicitudAtencion);
                    cn.Execute(squery, param);
                    structurePostInt.Data = true;

                    new Db_SolicitudAtencion_History(conexioAlent).Insert(servicio.id_SolicitudAtencion, servicio.statusAtencion, servicio.fk_userAsig, servicio.info_cliente,   servicio.materials, servicio.comments);

                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool deleteAtencion(int id_SolicitudAtencion)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "   delete from [SistemaWeb].[web].[SolicitudAtencion] where id_SolicitudAtencion =@id_SolicitudAtencion; " +
                        "  delete from [SistemaWeb].[web].[SolicitudAtencion_History] where id_SolicitudAtencion = @id_SolicitudAtencion; ";
                    var param = new DynamicParameters();
                    param.Add("@id_SolicitudAtencion", id_SolicitudAtencion);
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

        public StructureOutputSolicitud getSolicitudByUser(int fk_user, string estado, int cantidad)
        {
            StructureOutputSolicitud structurePostInt = new StructureOutputSolicitud();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {

                    string squery = string.Format("     select top {0} a.id_SolicitudAtencion,a. statusAtencion ,FORMAT( a.datecreated,'dd/MM/yyyy HH:mm') Fecha_creacion,a.info_cliente,a.materials,a.comments, fk_userAsig," +
                        "     SistemaWeb. web.getUserName(fk_user) as usernameOrig,SistemaWeb. web.getUserName(fk_userAsig) as usernameAsig,        s.servicio, s.direccion, c.Telefono, c.Dni, c.Nombre_completo, r.nombre + ' - ' + p.nombre + ' - ' + d.nombre + ' - ' + co.nombre  ubicacion " +
                        "  from[SistemaWeb].[web].[SolicitudAtencion] a with(nolock) inner join SistemaWeb.web.Servicio s with(nolock) on(s.id_servicio = a.id_servicio) "+
                        "  inner join SistemaWeb.web.Clients c with(nolock)  on(c.Pk_clients = s.id_client) inner join SistemaWeb.web.Comunidad co with(nolock) on(s.id_comunidad = co.id_comunidad) "+
                        "  inner join SistemaWeb.web.Distrito d with(nolock) on(d.id_distrito = co.id_distrito)  inner join SistemaWeb.web.Provincia p with(nolock) on(p.id_provincia = d.id_provincia) "+
                        "  inner join SistemaWeb.web.Region r with(nolock) on(r.id_region = p.id_region) where fk_userAsig = @fk_userAsig and statusAtencion like @statusAtencion order by datecreated desc ", cantidad);
                    var param = new DynamicParameters();

                    param.Add("@fk_userAsig", fk_user);
                    param.Add("@statusAtencion", estado);
                    structurePostInt.Data = cn.Query<SolicitudOutput>(squery, param).ToList();


                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<SolicitudOutput>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool updateSolicitudStatus(SolicitudUpdateStatus usuario)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  update [SistemaWeb].[web].[SolicitudAtencion] set  statusAtencion=@statusAtencion  " +
                        "     where id_SolicitudAtencion = @id_SolicitudAtencion";
                    var param = new DynamicParameters();

                    param.Add("@statusAtencion", usuario.statusAtencion);
                    param.Add("@id_SolicitudAtencion", usuario.id_SolicitudAtencion);
                    cn.Execute(squery, param);
                    structurePostInt.Data = true;

                    new Db_SolicitudAtencion_History(conexioAlent).Insert(usuario.id_SolicitudAtencion,  usuario.statusAtencion, 0, "", "", "");

                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }
        public StructureOutputSolicitud getAtencionByService(int id_servicio)
        {
            StructureOutputSolicitud structurePostInt = new StructureOutputSolicitud();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {

                    string squery = string.Format("     select a.id_SolicitudAtencion,a. statusAtencion ,FORMAT( a.datecreated,'dd/MM/yyyy HH:mm') Fecha_creacion,a.info_cliente,a.materials,a.comments, fk_userAsig," +
                        "      SistemaWeb.web.getUserName(fk_user) as usernameOrig, SistemaWeb.web.getUserName(fk_userAsig) as usernameAsig,        s.servicio, c.Nombre_completo, r.nombre + ' - ' + p.nombre + ' - ' + d.nombre + ' - ' + co.nombre  ubicacion " +
                        "  from[SistemaWeb].[web].[SolicitudAtencion] a with(nolock) inner join SistemaWeb.web.Servicio s with(nolock) on(s.id_servicio = a.id_servicio) " +
                        "  inner join SistemaWeb.web.Clients c with(nolock)  on(c.Pk_clients = s.id_client) inner join SistemaWeb.web.Comunidad co with(nolock) on(s.id_comunidad = co.id_comunidad) " +
                        "  inner join SistemaWeb.web.Distrito d with(nolock) on(d.id_distrito = co.id_distrito)  inner join SistemaWeb.web.Provincia p with(nolock) on(p.id_provincia = d.id_provincia) " +
                        "  inner join SistemaWeb.web.Region r with(nolock) on(r.id_region = p.id_region) where a.id_servicio=@id_servicio order by id_SolicitudAtencion desc ");
                    var param = new DynamicParameters();

                    param.Add("@id_servicio", id_servicio);

                    structurePostInt.Data = cn.Query<SolicitudOutput>(squery, param).ToList();


                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<SolicitudOutput>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputSolicitud getSolicitudAtencions(string dniOrNameclientStatus, int pagina, int registros)
        {
            StructureOutputSolicitud structurePostInt = new StructureOutputSolicitud();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    if (string.IsNullOrEmpty(dniOrNameclientStatus) || dniOrNameclientStatus == "null")
                    {
                        string squery = string.Format(" exec [SistemaWeb].[web].[ups_GetSolicitudAtencion] {0},{1} ", pagina, registros);

                        structurePostInt.Data = cn.Query<SolicitudOutput>(squery).ToList();
                    }
                    else
                    {
                        string squery = string.Format(" exec [SistemaWeb].[web].[ups_GetSolicitudAtencion2] @dniOrNameOrStatus,{0},{1} ", pagina, registros);
                        var param = new DynamicParameters();
                        dniOrNameclientStatus = "%" + dniOrNameclientStatus + "%";
                        param.Add("@dniOrNameOrStatus", dniOrNameclientStatus);
                        structurePostInt.Data = cn.Query<SolicitudOutput>(squery, param).ToList();
                    }

                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<SolicitudOutput>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool updateSolicitudStatusDetails(SolicitudUpdateStatusDetails usuario)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "  update [SistemaWeb].[web].[SolicitudAtencion] set  statusAtencion=@statusAtencion , materials=@materials, comments=@comments " +
                        "     where id_SolicitudAtencion = @id_SolicitudAtencion";
                    var param = new DynamicParameters();
                    param.Add("@materials", usuario.materials);
                    param.Add("@comments", usuario.comments);
                    param.Add("@statusAtencion", usuario.statusAtencion);
                    param.Add("@id_SolicitudAtencion", usuario.id_SolicitudAtencion);
                    cn.Execute(squery, param);
                    structurePostInt.Data = true;

                    new Db_SolicitudAtencion_History(conexioAlent).Insert(usuario.id_SolicitudAtencion, usuario.statusAtencion, 0, "", usuario.materials, usuario.comments);

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
