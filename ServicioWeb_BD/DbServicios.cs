using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using ServicioWeb_Entidades;
using ServicioWeb_Entidades.Deduccion;
using ServicioWeb_Entidades.Pagos;
using ServicioWeb_Entidades.Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public  class DbServicios
    {
        private readonly string sqlconexion;
        public DbServicios(string sqlconexion)
        {
            this.sqlconexion = sqlconexion;          

        }

        public StructureOutputRegion getRegions()
        {
            StructureOutputRegion structurePostInt = new StructureOutputRegion();
            try
            {
                using (SqlConnection cn = new SqlConnection(sqlconexion))
                {
                    string squery = "     select * from  [SistemaWeb].[web].[Region] ";
                    structurePostInt.Data = cn.Query<Region>(squery).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<Region>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputProvinces getProvinces(int id_region)
        {
            StructureOutputProvinces structurePostInt = new StructureOutputProvinces();
            try
            {
                using (SqlConnection cn = new SqlConnection(sqlconexion))
                {
                    string squery = "     select * from [SistemaWeb].[web].[Provincia] where id_region =@id_region";
                    var param = new DynamicParameters();
                    param.Add("@id_region", id_region);
                    structurePostInt.Data = cn.Query<Provinces>(squery,param).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<Provinces>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputDistricts getDistricts(int id_province)
        {
            StructureOutputDistricts structurePostInt = new StructureOutputDistricts();
            try
            {
                using (SqlConnection cn = new SqlConnection(sqlconexion))
                {
                    string squery = "    select * from  [SistemaWeb].[web].[Distrito] where id_provincia =@id_provincia";
                    var param = new DynamicParameters();
                    param.Add("@id_provincia", id_province);
                    structurePostInt.Data = cn.Query<Districts>(squery,param).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<Districts>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostInt AddService(ServiceInput servicio)
        {
            StructurePostInt structurePostInt = new StructurePostInt();
            try
            {
                using (SqlConnection cn = new SqlConnection(sqlconexion))
                {
                    string squery = " insert into  [SistemaWeb].[web].[Servicio] (servicio, id_comunidad, estado, precio, fechaInstalacion, direccion, referencia, id_client,latitud,longitud,informacion_adicional, fechaEstado,suministro) " +
                        "  output inserted.id_servicio " +
                        " values(@servicio, @id_comunidad, @estado, @precio, @fechaInstalacion, @direccion, @referencia, @id_client,@latitud, @longitud,@informacion_adicional, @fechaEstado,@suministro);" +
                        "";
                    var param = new DynamicParameters();
                    param.Add("@servicio", servicio.servicio);
                    param.Add("@id_comunidad", servicio.id_comunidad);
                    param.Add("@estado", servicio.estado_servicio);
                    param.Add("@precio", servicio.precio_servicio);
                    param.Add("@fechaInstalacion", servicio.fechainstalacion); 
                    param.Add("@direccion", servicio.direccion_servicio);
                    param.Add("@referencia", servicio.referencia_servicio);
                    param.Add("@id_client", servicio.id_client);
                    param.Add("@latitud", servicio.latitud);
                    param.Add("@longitud", servicio.longitud);
                    param.Add("@fk_user", servicio.fk_user);
                    param.Add("@informacion_adicional", servicio.informacion_adicional);
                    param.Add("@fechaEstado", servicio.fechainstalacion);
                    param.Add("@suministro", servicio.suministro);

                    structurePostInt.Data = cn.QueryFirstOrDefault<int>(squery, param);
                    if (structurePostInt.Data>0)
                    {
                        string squery2 = "insert into [SistemaWeb].[web].[Servicio_History] (id_servicio,estado,precio,direccion,referencia,latitud,longitud, servicio,fk_user, informacion_adicional,fechaEstado, suministro,id_comunidad) " +
                            "values (@id_servicio,@estado,@precio,@direccion,@referencia,@latitud,@longitud,@servicio,@fk_user,@informacion_adicional ,@fechaEstado,@suministro, @id_comunidad) ";
                        param.Add("@id_servicio", structurePostInt.Data);
                        cn.Execute(squery2, param);
                    }
                }

            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool UpdateService(ServiceUpdate service)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(sqlconexion))
                {
                    string squery = "  update [SistemaWeb].[web].[Servicio] set estado=@estado, precio=@precio, direccion=@direccion, referencia=@referencia, latitud=@latitud, longitud=@longitud,servicio=@servicio,informacion_adicional=@informacion_adicional, fechaEstado=@fechaEstado, suministro=@suministro,id_comunidad=@id_comunidad  where id_servicio=@id_servicio;" +
                        "  insert into [SistemaWeb].[web].[Servicio_History] (id_servicio,estado,precio,direccion,referencia,latitud,longitud,servicio,fk_user, informacion_adicional,fechaEstado,suministro,id_comunidad) " +
                        "values (@id_servicio,@estado,@precio,@direccion,@referencia,@latitud,@longitud,@servicio,@fk_user, @informacion_adicional,@fechaEstado,@suministro, @id_comunidad) ";
                    var param = new DynamicParameters();
                    param.Add("@estado", service.estado);
                    param.Add("@precio", service.precio);
                    param.Add("@direccion", service.direccion);
                    param.Add("@referencia", service.referencia);
                    param.Add("@id_servicio", service.id_servicio);
                    param.Add("@latitud", service.latitud);
                    param.Add("@longitud", service.longitud);
                    param.Add("@servicio", service.servicio);
                    param.Add("@fk_user", service.fk_user);
                    param.Add("@informacion_adicional", service.informacion_adicional);
                    param.Add("@fechaEstado", service.fechaEstado);
                    param.Add("@suministro", service.suministro);
                    param.Add("@id_comunidad", service.id_comunidad);
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

        public StructureOutputCommunities getCommunities(int id_districts)
        {
            StructureOutputCommunities structurePostInt = new StructureOutputCommunities();
            try
            {
                using (SqlConnection cn = new SqlConnection(sqlconexion))
                {
                    string squery = "    select * from  [SistemaWeb].[web].[Comunidad] where id_distrito =@id_distrito";
                    var param = new DynamicParameters();
                    param.Add("@id_distrito", id_districts);
                    structurePostInt.Data = cn.Query<Comunities>(squery,param).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<Comunities>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructureOutputServices getServices2(int id_client )
        {
            StructureOutputServices structurePostInt = new StructureOutputServices();
            try
            {
                using (SqlConnection cn = new SqlConnection(sqlconexion))
                {
                    string squery = "  select cl.Telefono, cl.Dni, cl.Nombre_completo, id_servicio, servicio, estado, precio, direccion,id_client, referencia, FORMAT(fechaInstalacion,'dd-MM-yyyy') fechaInstalacion,  FORMAT(fechaEstado,'dd-MM-yyyy') fechaEstado, r.nombre + ' - '+ p.nombre+ ' - '+ d.nombre + ' - '+ c.nombre as 'ubicacion', s.latitud, s.longitud, s.informacion_adicional, FORMAT(fechaEstado,'yyyy-MM-dd') fechaEstado2, suministro, " +
                        " r.id_region,p.id_provincia,c.id_distrito ,c.id_comunidad  from [SistemaWeb].[web].[Servicio] s " +
                        " inner join SistemaWeb.web.Clients cl on (cl.Pk_clients = s.id_client) inner join SistemaWeb.web.Comunidad c on(s.id_comunidad = c.id_comunidad) " +
                        "  inner join SistemaWeb.web.Distrito d on(d.id_distrito = c.id_distrito) "+
                        "  inner join SistemaWeb.web.Provincia p on(d.id_provincia = p.id_provincia) "+
                        "  inner join SistemaWeb.web.Region r on(p.id_region = r.id_region) "+
                        "  where id_client = @id_client; ";
                    var param = new DynamicParameters();
                    param.Add("@id_client", id_client);
                    List<OutputService> services = cn.Query<OutputService>(squery, param).ToList();
                    if (services!=null  && services.Count>0)
                    {
                        foreach (var item in services)
                        {
                            try
                            {
                               
                                //try
                                //{
                                //    inicio = DateTime.ParseExact(item.fechainstalacion, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                //}
                                //catch (Exception ex)
                                //{

                                //}
                                DateTime fact = DateTime.Now;
                                List<OutputFechas> fechasOutputs = new Db_B1(sqlconexion).GetHistoryFechasMulti(item.id_servicio);
                                List<string> mesPendientes = new List<string>();
                                List<string> mesPagados = new List<string>();
                                List<string> mesPendientesJson = new List<string>();
                                List<string> listaPagados = new List<string>();
                                List< DeudaPendientes > deudasPendientes  = new List<DeudaPendientes>();
                                string anios = fact.Year - 1 + "," + fact.Year;
                                List<Facturacion> listaFacturacion = new Db_B1(sqlconexion).GetServiciosPagadosTwoYears(item.id_servicio, anios);
                                if (listaFacturacion == null)
                                {
                                    listaFacturacion = new List<Facturacion>();
                                }
                                //else
                                //{
                                //    if (listaFacturacion.Count > 0)
                                //    {
                                //        int yearmax = listaFacturacion.Max(b => int.Parse(b.nameYear));
                                //        if (fechacorte.Year < yearmax)
                                //        {
                                //            var rgs = listaFacturacion.Where(v => int.Parse(v.nameYear) == yearmax).ToList();
                                //            int mesmax = rgs.Max(b => b.mes);
                                //            fechacorte = new DateTime(yearmax, mesmax, 1);
                                //        }
                                //    }

                                //}
                                StructureOutputDeduccion deduccion = new Db_deduccion(sqlconexion).getDeducciones(item.id_servicio);
                                if (deduccion == null)
                                {
                                    deduccion = new StructureOutputDeduccion();
                                }
                                if (deduccion.Data == null)
                                {
                                    deduccion.Data = new List<OutputDeduccion>();
                                }
                                List<OutputDeduccion> listaDeduccion = deduccion.Data;

                                if (fechasOutputs != null)
                                {
                                    fechasOutputs = fechasOutputs.OrderBy(v => v.fechaEstado).ToList();
                                    
                                    bool estado = false;
                                    while (!estado)
                                    {
                                        if (fechasOutputs.Count==1)
                                        {
                                            if (fechasOutputs[0].estado != "Activo")
                                            {
                                                estado = true;
                                                continue;
                                            }
                                        }
                                        DateTime inicio = DateTime.MinValue;
                                        DateTime fechacorte = DateTime.MinValue;
                                        if (fechasOutputs.Count > 0)
                                        {
                                            inicio = fechasOutputs[0].fechaEstado;
                                            float precioActual = fechasOutputs[0].precio;
                                            string servicioActual = fechasOutputs[0].servicio;
                                            fechasOutputs.RemoveAt(0);
                                            
                                            if (fechasOutputs .Count==0)
                                            {
                                                DateTime fechacortePosible = new DateTime(fact.Year, fact.Month, 1);
                                                fechacorte = fechacortePosible.AddDays(-1);
                                            }
                                            else
                                            {
                                                DateTime nextActive = fechasOutputs.Where(v => v.fechaEstado > inicio && v.estado == "Activo").Select(v => v.fechaEstado).FirstOrDefault();
                                                DateTime nextDifActive = fechasOutputs.Where(v => v.fechaEstado > inicio && v.estado != "Activo").Select(v => v.fechaEstado).FirstOrDefault();
                                                if (nextActive!= DateTime.MinValue && nextDifActive != DateTime.MinValue)
                                                {
                                                    if (nextDifActive < nextActive)
                                                    {
                                                        fechacorte = nextDifActive;
                                                        fechasOutputs.RemoveAll(v=> v.fechaEstado <= fechacorte);
                                                    }
                                                    else
                                                    {
                                                        fechacorte = nextActive;
                                                    }
                                                }
                                                else
                                                {
                                                    if (nextActive != DateTime.MinValue)
                                                    {
                                                        fechacorte = nextActive;
                                                    }
                                                    if (nextDifActive != DateTime.MinValue)
                                                    {
                                                        fechacorte = nextDifActive;
                                                        fechasOutputs.RemoveAll(v => v.fechaEstado <= fechacorte);
                                                    }
                                                }
                                            }


                                            if (inicio != DateTime.MinValue)
                                            {
                                                int diaMaximo1 = DateTime.DaysInMonth(inicio.Year, inicio.Month);
                                                int diferenciadias = diaMaximo1 - inicio.Day;
                                                if (diferenciadias > 0)
                                                {
                                                    item.yearPagoInicio = inicio.Year;
                                                    item.mesPagoInicio = inicio.Month;
                                                    item.precioPagoInicio = (int)Math.Ceiling((precioActual * diferenciadias) / diaMaximo1);

                                                }
                                            }

                                            if (fechacorte != DateTime.MinValue)
                                            {
                                                if (inicio.Year == fechacorte.Year && inicio.Month == fechacorte.Month)
                                                {
                                                    int diaMaximo2 = DateTime.DaysInMonth(fechacorte.Year, fechacorte.Month);
                                                    int diferenciadias = fechacorte.Day - inicio.Day;
                                                    if (diferenciadias > 0)
                                                    {
                                                        item.yearPagoFin = fechacorte.Year;
                                                        item.mesPagoFin = fechacorte.Month;
                                                        item.precioPagoFin = (int)Math.Ceiling((precioActual * diferenciadias) / diaMaximo2);

                                                    }
                                                }
                                                else
                                                {
                                                    int diaMaximo2 = DateTime.DaysInMonth(fechacorte.Year, fechacorte.Month);
                                                    int diferenciadias = fechacorte.Day;
                                                    if (diferenciadias > 0)
                                                    {
                                                        item.yearPagoFin = fechacorte.Year;
                                                        item.mesPagoFin = fechacorte.Month;
                                                        item.precioPagoFin = (int)Math.Ceiling((precioActual * diferenciadias) / diaMaximo2);

                                                    }
                                                } 
                                            }
                                            else
                                            {

                                                DateTime fechacortePosible = new DateTime(fact.Year, fact.Month, 1);
                                                fechacorte = fechacortePosible.AddDays(-1);

                                                item.yearPagoFin = fechacorte.Year;
                                                item.mesPagoFin = fechacorte.Month;
                                                item.precioPagoFin = precioActual;

                                            }

                                            int diferenciaMeses = ((fechacorte.Year - inicio.Year) * 12) + fechacorte.Month - inicio.Month;
                                            if (diferenciaMeses >= 0)
                                            {
                                                for (int i = 0; i <= diferenciaMeses; i++)
                                                {
                                                    DateTime actual = inicio.AddMonths(i);
                                                    if (actual.Year < (fact.Year - 1))
                                                    {
                                                        continue;
                                                    }
                                                    var precioPagar = precioActual;
                                                    if (actual.Month == item.mesPagoInicio && actual.Year == item.yearPagoInicio)
                                                    {
                                                        precioPagar = item.precioPagoInicio;

                                                    }
                                                    if (actual.Month == item.mesPagoFin && actual.Year == item.yearPagoFin)
                                                    {
                                                        precioPagar = item.precioPagoFin;

                                                    }
                                                    var resultados = listaFacturacion.Where(v => v.nameYear == actual.Year.ToString() && v.mes == actual.Month).ToList();
                                                    if (resultados != null && resultados.Count > 0)
                                                    {
                                                        var mespag = resultados[0];
                                                        var sumatotal = resultados.Sum(v => v.monto);
                                                        if (sumatotal == precioPagar)
                                                        {
                                                            listaPagados.Add(mespag.nameMonth + "/" + mespag.nameYear);
                                                            mesPagados.Add(mespag.nameMonth + "/" + mespag.nameYear);

                                                        }
                                                        else
                                                        {
                                                             
                                                            float diferenciaPrecio = (precioPagar - sumatotal);
                                                            var listaDeduccionesFiltro = listaDeduccion.Where(v => v.nameYear == mespag.nameYear && v.mes == mespag.mes).ToList();
                                                            if (listaDeduccionesFiltro != null && listaDeduccionesFiltro.Count > 0)
                                                            {
                                                                var sumadeduciones = listaDeduccionesFiltro.Sum(b => b.monto);
                                                                diferenciaPrecio = diferenciaPrecio - sumadeduciones;

                                                            }

                                                             
                                                            if (diferenciaPrecio > 0)
                                                            {
                                                                deudasPendientes.Add(new DeudaPendientes {
                                                                    mes = mespag.mes, 
                                                                    nameMes = mespag.nameMonth,
                                                                    nameYear = mespag.nameYear,
                                                                    precio = diferenciaPrecio , 
                                                                    servicio = servicioActual
                                                                });
                                                                //mesPendientes.Add("S/." + diferenciaPrecio + " " + mespag.nameMonth + "/" + mespag.nameYear);
                                                                //mesPendientesJson.Add(diferenciaPrecio + "-" + mespag.mes + "-" + mespag.nameMonth + "-" + mespag.nameYear);
                                                            }

                                                        }
                                                    }
                                                    else
                                                    {
                                                        var listaDeduccionesFiltro = listaDeduccion.Where(v => v.nameYear == actual.Year.ToString() && v.mes == actual.Month).ToList();
                                                        if (listaDeduccionesFiltro != null && listaDeduccionesFiltro.Count > 0)
                                                        {
                                                            var sumadeduciones = listaDeduccionesFiltro.Sum(b => b.monto);
                                                            precioPagar = precioPagar - sumadeduciones;

                                                        }
                                                        CultureInfo culturaEspañol = new CultureInfo("es-ES");

                                                        string nombreMes = culturaEspañol.DateTimeFormat.GetMonthName(actual.Month);
                                                        if (precioPagar > 0)
                                                        {
                                                            deudasPendientes.Add(new DeudaPendientes
                                                            {
                                                                mes = actual.Month,
                                                                nameMes = nombreMes,
                                                                nameYear = actual.Year.ToString(),
                                                                precio = precioPagar,
                                                                servicio = servicioActual
                                                            });
                                                            //mesPendientes.Add(nombreMes + "/" + actual.Year);
                                                            //mesPendientesJson.Add(precioPagar + "-" + actual.Month + "-" + nombreMes + "-" + actual.Year);

                                                        }
                                                        else
                                                        {
                                                            listaPagados.Add(nombreMes + "/" + actual.Year);
                                                            mesPagados.Add("S/." + 0 + " por deducion " + nombreMes + "/" + actual.Year);
                                                        }

                                                    }

                                                }
                                            }

                                        }
                                        else 
                                        { 
                                            estado = true;
                                        }
                                        
                                    }

                                }

                                var agrupaciones = deudasPendientes.GroupBy(v => new { v.mes, v.nameYear }).ToList();
                                foreach (var agrupacion in agrupaciones)
                                {
                                    var sumatoria = agrupacion.Sum(v => v.precio);
                                    var agr = agrupacion.FirstOrDefault();
                                    var listaDeduccionesFiltro = listaDeduccion.Where(v => v.nameYear == agr.nameYear && v.mes == agr.mes).ToList();
                                    var nombresservicios = string.Join(", ", agrupacion.Select(v => v.servicio).Distinct());
                                    if (listaDeduccionesFiltro != null && listaDeduccionesFiltro.Count > 0)
                                    {
                                        var sumadeduciones = listaDeduccionesFiltro.Sum(b => b.monto);
                                        sumatoria = sumatoria - sumadeduciones;

                                    }
                                    if (sumatoria > 0)
                                    {
                                        mesPendientes.Add("S/." + sumatoria + " " + agr.nameMes + "/" + agr.nameYear);
                                        mesPendientesJson.Add(sumatoria + "-" + agr.mes + "-" + agr.nameMes + "-" + agr.nameYear + "-"+ nombresservicios);
                                    }
                                    else
                                    {
                                        listaPagados.Add(agr.nameMes + "/" + agr.nameYear);
                                        mesPagados.Add("S/." + 0 + " por deducion " + agr.nameMes + "/" + agr.nameYear);
                                    }
                                }
                                if (listaPagados != null)
                                {
                                    foreach (var item2 in listaFacturacion)
                                    {
                                        string nombre = item2.nameMonth + "/" + item2.nameYear;
                                        if (!listaPagados.Exists(v=>v== nombre))
                                        {
                                            listaPagados.Add(nombre);
                                        }
                                    }
                                }

                                item.pagado = string.Join("; ", listaPagados.Distinct().ToList());
                                //item.pagado = string.Join("; ", mesPagados.TakeLast(12));
                                
                                item.pendiente = string.Join("; ", mesPendientes);

                                item.pendienteJson = JsonConvert.SerializeObject(mesPendientesJson);
                                if (string.IsNullOrEmpty(item.pendiente))
                                {
                                    item.pendiente = "-";
                                }
                                if (string.IsNullOrEmpty(item.pagado))
                                {
                                    item.pagado = "-";
                                }
                            }
                            catch (Exception ex)
                            {
 
                            }
                            
                        }
                    }
                    structurePostInt.Data = services;
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OutputService>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public StructurePostBool deleteService(int id_service)
        {
            StructurePostBool structurePostInt = new StructurePostBool();
            try
            {
                using (SqlConnection cn = new SqlConnection(sqlconexion))
                {
                    string squery = "    delete from [SistemaWeb].[web].[Servicio] where id_servicio=@id_servicio ";
                    var param = new DynamicParameters();
                    param.Add("@id_servicio", id_service);
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

        public StructureHistoryOuput getHistory(int id_servicio)
        {
            StructureHistoryOuput structurePostInt = new StructureHistoryOuput();
            try
            {
                using (SqlConnection cn = new SqlConnection(sqlconexion))
                {
                    //string squery = "  select  estado,precio,direccion,referencia,latitud,longitud, FORMAT(updateService, 'dd-MM-yyyy HH:mm:ss') updateService, servicio, u.Correo as usuario, h.informacion_adicional,  FORMAT(fechaEstado,'dd-MM-yyyy') fechaEstado,  FORMAT(fechaEstado,'yyyy-MM-dd') fechaEstado2, suministro " +
                    //    "  from [SistemaWeb].[web].[Servicio_History] h left join SistemaWeb.web.Users u on (u.Pk_user= h.fk_user) where id_servicio=@id_servicio  order by PK_Servicio_History desc ";
                    string squery2 = "  select  estado,precio,direccion,referencia,latitud,longitud, FORMAT(updateService, 'dd-MM-yyyy HH:mm:ss') updateService, servicio, u.Correo as usuario, h.informacion_adicional,  FORMAT(fechaEstado,'dd-MM-yyyy') fechaEstado,  FORMAT(fechaEstado,'yyyy-MM-dd') fechaEstado2, suministro  , r.nombre + ' - '+ p.nombre+ ' - '+ d.nombre + ' - '+ c.nombre as 'ubicacion'\r\n  " +
                        "                    from [SistemaWeb].[web].[Servicio_History] h left join SistemaWeb.web.Users u on (u.Pk_user= h.fk_user)\r\n\t\t\t\t\t  left join SistemaWeb.web.Comunidad c on(h.id_comunidad = c.id_comunidad)  \r\n                           left join SistemaWeb.web.Distrito d on(d.id_distrito = c.id_distrito)  \r\n                    " +
                        "       left join SistemaWeb.web.Provincia p on(d.id_provincia = p.id_provincia)  \r\n                           left join SistemaWeb.web.Region r on(p.id_region = r.id_region)\r\n\t\t\t\t\t  where id_servicio=@id_servicio  order by PK_Servicio_History desc";
                    var param = new DynamicParameters();
                    param.Add("@id_servicio", id_servicio);
                    structurePostInt.Data = cn.Query<HistoryOuput>(squery2, param).ToList();
                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<HistoryOuput>();
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
