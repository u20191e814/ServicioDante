using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using ServicioWeb_Entidades.B1;
using ServicioWeb_Entidades.Clientes;
using ServicioWeb_Entidades.Deduccion;
using ServicioWeb_Entidades.Pagos;
using ServicioWeb_Entidades.Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_BD
{
    public class Db_B1
    {
        private readonly string conexioAlent;
        public Db_B1(string sqlconexion)
        {
            conexioAlent = sqlconexion;

        }

        public StructureOuputSearchB1 getSearch(int id_distrito, int id_comunidad, string servicio, string estado, int pagosB1,  int anio )
        {
            StructureOuputSearchB1 structurePostInt = new StructureOuputSearchB1();
            try
            {
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = "select s.id_servicio,  c.Dni, c.Nombre_completo, servicio, estado, FORMAT(fechaInstalacion,'dd-MM-yyyy') fechaInstalacion, direccion, referencia, d.nombre, s.precio, s.suministro, s.latitud, s.longitud, s.fechaEstado, c.Telefono \r\n " +
                        " from [SistemaWeb].[web].[Servicio] s inner join SistemaWeb.web.Clients c on (c.Pk_clients = s.id_client)\r\n  inner join SistemaWeb.web.Comunidad d on (d.id_comunidad = s.id_comunidad) ";
                    string condicion = string.Empty;
                    if (id_distrito >0)
                    {
                        if (string.IsNullOrEmpty(condicion))
                        {
                            condicion = " d.id_distrito =" + id_distrito;
                        }
                        else
                        {
                            condicion = condicion + " and d.id_distrito =" + id_distrito;
                        }
                    }
                    
                    if (id_comunidad > 0)
                    {
                        if (string.IsNullOrEmpty(condicion))
                        {
                            condicion = " s.id_comunidad =" + id_comunidad;
                        }
                        else
                        {
                            condicion = condicion + " and s.id_comunidad =" + id_comunidad;
                        }
                    }
                    if (servicio != "todos")
                    {
                        if (string.IsNullOrEmpty(condicion))
                        {
                            condicion = " s.servicio like '%" + servicio + "%'";
                        }
                        else
                        {
                            condicion = condicion + " and  s.servicio like '%" + servicio + "%'";
                        }
                    }
                    if (estado != "todos")
                    {
                        if (string.IsNullOrEmpty(condicion))
                        {
                            condicion = " s.estado like '%" + estado + "%'";
                        }
                        else
                        {
                            condicion = condicion + " and  s.estado like '%" + estado + "%'";
                        }
                    }
                    if (!string.IsNullOrEmpty(condicion))
                    {
                        squery = squery + " where " + condicion;
                    }
                    squery = squery + "   order by d.nombre, servicio, estado";
                    List<OuputSearchB1> lista = cn.Query<OuputSearchB1>(squery, null).ToList();
                    List<OuputSearchB1> listaFiltro = new List<OuputSearchB1>();
                    if (lista.Count > 0  )
                    {
                        foreach (var item in lista)
                        {
                            if (string.IsNullOrEmpty(item.pendiente))
                            {
                                item.pendiente = "-";
                            }
                            if (string.IsNullOrEmpty(item.pagado))
                            {
                                item.pagado = "-";
                            }
                            if (item.fechaEstado!=  DateTime.MinValue)
                            {
                                item.estado = item.estado + " desde " + item.fechaEstado.ToString("dd-MM-yyyy");
                            }
                            if (pagosB1 ==0)
                            {
                                listaFiltro.Add(item);
                                continue;
                            }
                            //DateTime inicio = DateTime.MinValue;
                            //DateTime fechacorte = DateTime.MinValue;
                            //try
                            //{
                            //    inicio = DateTime.ParseExact(item.fechaInstalacion, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //catch (Exception ex)
                            //{

                            //}
                            //List<DateTime> fechas = new Db_B1(conexioAlent).GetHistoryFechas(item.id_servicio);
                            //if (fechas != null)
                            //{
                            //    if (fechas.Count > 1)
                            //    {
                            //        inicio = fechas[0];
                            //        fechacorte = fechas[fechas.Count - 1];
                            //    }
                            //    else
                            //    {
                            //        inicio = fechas.FirstOrDefault();
                            //        if (fechas.Count == 1)
                            //        {
                            //            DateTime fact = DateTime.Now;
                            //            DateTime newdate = DateTime.MinValue;
                            //            if (fact.Year != anio)
                            //            {
                            //                DateTime dif = new DateTime(anio+1, 1, 1);
                            //                newdate = dif.AddDays(-1);
                            //            }
                            //            else
                            //            {
                            //                DateTime dif = new DateTime(fact.Year, fact.Month, 1);
                            //                newdate = dif.AddDays(-1);
                            //            }

                            //            fechacorte = newdate;
                            //        }
                            //    }
                            //}
                            //if (inicio != DateTime.MinValue)
                            //{
                            //    int diaMaximo1 = DateTime.DaysInMonth(inicio.Year, inicio.Month);
                            //    int diferenciadias = diaMaximo1 - inicio.Day;
                            //    if (diferenciadias > 0)
                            //    {
                            //        item.yearPagoInicio = inicio.Year;
                            //        item.mesPagoInicio = inicio.Month;
                            //        item.precioPagoInicio = (int)Math.Ceiling((item.precio * diferenciadias) / diaMaximo1);

                            //    }
                            //}
                            //if (fechacorte != DateTime.MinValue)
                            //{
                            //    if (inicio.Year == fechacorte.Year && inicio.Month == fechacorte.Month)
                            //    {
                            //        int diaMaximo2 = DateTime.DaysInMonth(fechacorte.Year, fechacorte.Month);
                            //        int diferenciadias = fechacorte.Day - inicio.Day;
                            //        if (diferenciadias > 0)
                            //        {
                            //            item.yearPagoFin = fechacorte.Year;
                            //            item.mesPagoFin = fechacorte.Month;
                            //            item.precioPagoFin = (int)Math.Ceiling((item.precio * diferenciadias) / diaMaximo2);

                            //        }
                            //    }
                            //    else
                            //    {
                            //        int diaMaximo2 = DateTime.DaysInMonth(fechacorte.Year, fechacorte.Month);
                            //        int diferenciadias = fechacorte.Day;
                            //        if (diferenciadias > 0)
                            //        {
                            //            item.yearPagoFin = fechacorte.Year;
                            //            item.mesPagoFin = fechacorte.Month;
                            //            item.precioPagoFin = (int)Math.Ceiling((item.precio * diferenciadias) / diaMaximo2);

                            //        }
                            //    }

                            //}


                            //List<Facturacion> listaFacturacion = GetServiciosPagados(item.id_servicio, anio);
                            //if (listaFacturacion == null) 
                            //{ 
                            //    listaFacturacion = new List<Facturacion>();
                            //}
                            //StructureOutputDeduccion deduccion = new Db_deduccion(conexioAlent).getDeducciones(item.id_servicio);
                            //if (deduccion == null)
                            //{
                            //    deduccion = new StructureOutputDeduccion();
                            //}
                            //if (deduccion.Data == null)
                            //{
                            //    deduccion.Data = new List<OutputDeduccion>();
                            //}
                            //List<OutputDeduccion> listaDeduccion = deduccion.Data;
                            //DateTime ahora = DateTime.Now;
                            //DateTime fechaInstalacion = item.fechaEstado;
                            //if (fechaInstalacion.Year> anio)
                            //{
                            //    continue;
                            //}
                            //if (fechaInstalacion==DateTime.MinValue)
                            //{
                            //    fechaInstalacion = DateTime.ParseExact(item.fechaInstalacion, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //int nromeses = 12;
                            //if (fechas!=null && fechas.Count>1)
                            //{
                            //    fechaInstalacion = fechas[0];
                            //    nromeses = fechas[fechas.Count -1].Month;

                            //}
                            //else
                            //{
                            //    nromeses = ahora.Month;
                            //}
                            //if (ahora.Year != anio)
                            //{
                            //    if (fechacorte!= DateTime.MinValue)
                            //    {
                            //        nromeses = fechacorte.Month;
                            //    }
                            //    else
                            //    {
                            //        nromeses = 12;
                            //    }

                            //}
                            //if (fechaInstalacion.Year != anio)
                            //{
                            //    fechaInstalacion = new DateTime(anio, 1, 1);
                            //}
                            //int diaMaximo = DateTime.DaysInMonth(fechaInstalacion.Year, fechaInstalacion.Month);
                            //int diferencia = diaMaximo - fechaInstalacion.Day;
                            //string pendiente =string.Empty;

                            //int diaMaximoActual = DateTime.DaysInMonth(ahora.Year, ahora.Month);
                            //List<string> mesPendientes = new List<string>();
                            //List<string> mespagados = new List<string>();
                            //if (listaFacturacion!= null  && listaFacturacion.Count>0)
                            //{
                            //    mespagados.AddRange(listaFacturacion.Select(v => v.texto));
                            //}
                            //if (pagosB1 ==1 || pagosB1 ==3)
                            //{
                            //    if (fechacorte != DateTime.MinValue)
                            //    {
                            //        if (fechacorte.Year != anio)
                            //        {
                            //            continue;
                            //        }
                            //    }
                            //    int mesInicial = fechaInstalacion.Month;
                            //    if (diferencia <= 2)
                            //    {
                            //        mesInicial = fechaInstalacion.Month + 1;                                    
                            //    }
                            //    if (mesInicial >12)
                            //    {
                            //        mesInicial = 12;
                            //    }
                            //    if ( mesInicial <= 12 && mesInicial <= nromeses)
                            //    {


                            //        for (int i = mesInicial; i <= nromeses; i++) 
                            //        {
                            //            if (ahora.Month ==i)
                            //            { 
                            //                if (ahora.Day < diaMaximoActual)
                            //                {
                            //                    continue;
                            //                }
                            //            }
                            //            if (!listaFacturacion.Exists(v=>v.mes == i))
                            //            {

                            //                CultureInfo culturaEspañol = new CultureInfo("es-ES");

                            //                string nombreMes = culturaEspañol.DateTimeFormat.GetMonthName(i);
                            //                nombreMes =char.ToUpper(nombreMes[0]) + nombreMes.Substring(1);
                            //                float pr = item.precio;
                            //                if (i == item.mesPagoInicio && item.yearPagoInicio == anio)
                            //                {
                            //                    pr = item.precioPagoInicio;
                            //                }
                            //                if (i == item.mesPagoFin && item.yearPagoFin == anio)
                            //                {
                            //                    pr = item.precioPagoFin;
                            //                }
                            //                var listaDeduccionesFiltro = listaDeduccion.Where(v => v.nameYear == anio.ToString() && v.mes == i).ToList();
                            //                if (listaDeduccionesFiltro != null && listaDeduccionesFiltro.Count > 0)
                            //                {
                            //                    var sumadeduciones = listaDeduccionesFiltro.Sum(b => b.monto);
                            //                    pr = pr - sumadeduciones;
                            //                    // sumatotal = sumatotal - sumadeduciones;   
                            //                }
                            //                if (pr > 0 )
                            //                {
                            //                    mesPendientes.Add("S/:" + pr + "-" + nombreMes + "/" + anio);

                            //                }
                            //                else
                            //                {
                            //                    mespagados.Add("S/." + 0 + " por deducion " + nombreMes + "/" + anio);
                            //                }

                            //            }
                            //            else
                            //            {
                            //                try
                            //                {
                            //                    var info = listaFacturacion.Exists(v => v.mes == i);

                            //                    if (info)
                            //                    {
                            //                        var pago = listaFacturacion.FirstOrDefault(v => v.mes == i);
                            //                        if (pago != null)
                            //                        {
                            //                            float pr = item.precio;
                            //                            if (i == item.mesPagoInicio && item.yearPagoInicio == int.Parse(pago.nameYear))
                            //                            {
                            //                                pr = item.precioPagoInicio;
                            //                            }
                            //                            if (i == item.mesPagoFin && item.yearPagoFin == int.Parse(pago.nameYear))
                            //                            {
                            //                                pr = item.precioPagoFin;
                            //                            }
                            //                            if (pr > pago.monto)
                            //                            {
                            //                                CultureInfo culturaEspañol = new CultureInfo("es-ES");

                            //                                string nombreMes = culturaEspañol.DateTimeFormat.GetMonthName(i);
                            //                                var diferenciasoles = pr - pago.monto;
                            //                                if (diferenciasoles>0)
                            //                                {
                            //                                    var listaDeduccionesFiltro = listaDeduccion.Where(v => v.nameYear == anio.ToString() && v.mes == i).ToList();
                            //                                    if (listaDeduccionesFiltro != null && listaDeduccionesFiltro.Count > 0)
                            //                                    {
                            //                                        var sumadeduciones = listaDeduccionesFiltro.Sum(b => b.monto);
                            //                                        diferenciasoles = diferenciasoles - sumadeduciones;
                            //                                        // sumatotal = sumatotal - sumadeduciones;   
                            //                                    }
                            //                                    if (diferenciasoles >0 )
                            //                                    {
                            //                                        nombreMes = char.ToUpper(nombreMes[0]) + nombreMes.Substring(1);
                            //                                        mesPendientes.Add("S/:" + diferenciasoles + "-" + nombreMes + "/" + anio);

                            //                                    }
                            //                                    else
                            //                                    {
                            //                                        mespagados.Add("S/." + 0 + " por deducion " + nombreMes + "/" + anio);
                            //                                    }
                            //                                }

                            //                            }
                            //                        }
                            //                    }
                            //                }
                            //                catch (Exception ex)
                            //                {

                            //                }
                            //            }

                            //        }
                            //        item.pendiente = string.Join("; " , mesPendientes);
                            //    }
                            //}
                            //if (pagosB1 == 2 || pagosB1 == 3)
                            //{
                            //    if (listaFacturacion != null)
                            //    {
                            //        item.pagado = string.Join("; "  , mespagados);
                            //    }

                            //}

                            ///**************
                            ///

                            DateTime fact = DateTime.Now;
                            List<OutputFechas> fechasOutputs = new Db_B1(conexioAlent).GetHistoryFechasMulti(item.id_servicio);
                            List<string> mesPendientes = new List<string>();
                            
                            List<string> mesPendientesJson = new List<string>();
                            List<string> listaPagados = new List<string>();
                            List<DeudaPendientes> deudasPendientes = new List<DeudaPendientes>();
                            string anios = fact.Year - 1 + "," + fact.Year;
                            List<Facturacion> listaFacturacion = new Db_B1(conexioAlent).GetServiciosPagadosTwoYears(item.id_servicio, anios);
                            if (listaFacturacion == null)
                            {
                                listaFacturacion = new List<Facturacion>();
                            }
                             
                            StructureOutputDeduccion deduccion = new Db_deduccion(conexioAlent).getDeducciones(item.id_servicio);
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

                               

                                bool estado2 = false;
                                while (!estado2)
                                {
                                    if (fechasOutputs.Count == 1)
                                    {
                                        if (fechasOutputs[0].estado != "Activo")
                                        {
                                            estado2 = true;
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
                                        if (fechasOutputs.Count == 0)
                                        {
                                            DateTime fechacortePosible = new DateTime(fact.Year, fact.Month, 1);
                                            fechacorte = fechacortePosible.AddDays(-1);
                                        }
                                        else
                                        {
                                            DateTime nextActive = fechasOutputs.Where(v => v.fechaEstado > inicio && v.estado == "Activo").Select(v => v.fechaEstado).FirstOrDefault();
                                            DateTime nextDifActive = fechasOutputs.Where(v => v.fechaEstado > inicio && v.estado != "Activo").Select(v => v.fechaEstado).FirstOrDefault();
                                            if (nextActive != DateTime.MinValue && nextDifActive != DateTime.MinValue)
                                            {
                                                if (nextDifActive < nextActive)
                                                {
                                                    fechacorte = nextDifActive;
                                                    fechasOutputs.RemoveAll(v => v.fechaEstado <= fechacorte);
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
                                                        if (mespag.nameYear == anio.ToString())
                                                        {
                                                            listaPagados.Add(mespag.nameMonth + "/" + mespag.nameYear);

                                                        }


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
                                                            if (mespag.nameYear == anio.ToString())
                                                            {
                                                                deudasPendientes.Add(new DeudaPendientes
                                                                {
                                                                    mes = mespag.mes,
                                                                    nameMes = mespag.nameMonth,
                                                                    nameYear = mespag.nameYear,
                                                                    precio = diferenciaPrecio,
                                                                    servicio = servicioActual
                                                                });
                                                            }
                                                                
                                                             
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
                                                        if (actual.Year == anio)
                                                        {
                                                            deudasPendientes.Add(new DeudaPendientes
                                                            {
                                                                mes = actual.Month,
                                                                nameMes = nombreMes,
                                                                nameYear = actual.Year.ToString(),
                                                                precio = precioPagar,
                                                                servicio = servicioActual
                                                            });

                                                        }

                                                    }
                                                    else
                                                    {
                                                        if (actual.Year == anio)
                                                        {
                                                            listaPagados.Add(nombreMes + "/" + actual.Year);
                                                        }
                                                           
                                                        
                                                    }

                                                }

                                            }
                                        }

                                    }
                                    else
                                    {
                                        estado2 = true;
                                    }

                                }

                            }

                            var agrupaciones = deudasPendientes.GroupBy(v => new { v.mes, v.nameYear }).ToList();
                            foreach (var agrupacion in agrupaciones)
                            {
                                var sumatoria = agrupacion.Sum(v => v.precio);
                                var agr = agrupacion.FirstOrDefault();
                                var listaDeduccionesFiltro = listaDeduccion.Where(v => v.nameYear == agr.nameYear && v.mes == agr.mes).ToList();
                                var nombresservicios = string.Join(",", agrupacion.Select(v => v.servicio).Distinct());
                                if (listaDeduccionesFiltro != null && listaDeduccionesFiltro.Count > 0)
                                {
                                    var sumadeduciones = listaDeduccionesFiltro.Sum(b => b.monto);
                                    sumatoria = sumatoria - sumadeduciones;

                                }
                                if (sumatoria > 0)
                                {
                                    if (agr.nameYear == anio.ToString())
                                    {
                                        mesPendientes.Add("S/." + sumatoria + " " + agr.nameMes + "/" + agr.nameYear);
                                        mesPendientesJson.Add(sumatoria + "-" + agr.mes + "-" + agr.nameMes + "-" + agr.nameYear + "-" + nombresservicios);
                                    }
                                        
                                }
                                else
                                {
                                    if (agr.nameYear == anio.ToString())
                                    {
                                        listaPagados.Add(agr.nameMes + "/" + agr.nameYear);
                                    }
                                       
                                     
                                }
                            }
                            if (listaPagados != null)
                            {
                                foreach (var item2 in listaFacturacion)
                                {
                                    if (item2.nameYear == anio.ToString())
                                    {
                                        string nombre = item2.nameMonth + "/" + item2.nameYear;
                                        if (!listaPagados.Exists(v => v == nombre))
                                        {
                                            listaPagados.Add(nombre);
                                        }
                                    }
                                        
                                }
                            }
                            if (listaPagados!=null && listaPagados.Count >0)
                            {
                                item.pagado = string.Join("; ", listaPagados.Distinct().ToList());

                            }
                            if (mesPendientes != null && mesPendientes.Count> 0)
                            {
                                item.pendiente = string.Join("; ", mesPendientes);
                            }

                            

                            if (string.IsNullOrEmpty(item.pendiente))
                            {
                                item.pendiente = "-";
                            }
                            if (string.IsNullOrEmpty(item.pagado))
                            {
                                item.pagado = "-";
                            }
                            if (pagosB1 ==1 && item.pendiente =="-")
                            {
                                continue;
                            }
                            if (pagosB1 == 2 && item.pagado == "-")
                            {
                                continue;
                            }
                            if(pagosB1 == 3 && item.pagado == "-" && item.pendiente == "-")
                            {
                                continue;
                            }
                            listaFiltro.Add(item);  
                        }
                    }
                    structurePostInt.Data = listaFiltro;
                    

                }
                if (structurePostInt.Data == null)
                {
                    structurePostInt.Data = new List<OuputSearchB1>();
                }
            }
            catch (Exception ex)
            {

                structurePostInt.InternalMessage = ex.Message;
            }
            return structurePostInt;
        }

        public List<DateTime> GetHistoryFechas(int id_servicio)
        {
            try
            {
                List<DateTime> registros = new List<DateTime>();
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = string.Format( "  select fechaEstado from [SistemaWeb].[web].[Servicio_History] where id_servicio ={0} and PK_Servicio_History >= " +
                        "(  select top 1 PK_Servicio_History from [SistemaWeb].[web].[Servicio_History] where id_servicio = {0} and estado = 'Activo' order by PK_Servicio_History desc )  \r\n  " +
                        "  " , id_servicio);
                    registros = cn.Query<DateTime>  (squery).ToList();
                }
                return registros;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetHistoryFechas: " + ex.Message);
                return null;
            }
        }
        public List<OutputFechas> GetHistoryFechasMulti(int id_servicio)
        {
            try
            {
                List<OutputFechas> registros = new List<OutputFechas>();
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = string.Format("select  fechaEstado ,estado, max (precio) precio, max(servicio) servicio from [SistemaWeb].[web].Servicio_History " +
                        " where id_servicio = {0} group by fechaEstado, estado ", id_servicio);
                    registros = cn.Query<OutputFechas>(squery).ToList();
                }
                return registros;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetHistoryFechas: " + ex.Message);
                return null;
            }
        }
        public List<Facturacion> GetServiciosPagados(int id_servicio, int anio)
        {
            try
            {
                List<Facturacion> lista = null;
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = string.Format(" select monto, nameMonth, nameYear, mes " +
                        "from [SistemaWeb].[web].[Facturacion] where id_servicio=@id_servicio and  nameYear like '{0}'  order by mes asc", anio);
                    var param = new DynamicParameters();
                    param.Add("@id_servicio", id_servicio);
                    lista = cn.Query<Facturacion>(squery, param).ToList();
                }
                return lista;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<Facturacion> GetServiciosPagadosTwoYears(int id_servicio, string anios)
        {
            try
            {
                List<Facturacion> lista = null;
                using (SqlConnection cn = new SqlConnection(conexioAlent))
                {
                    string squery = string.Format(" select monto, nameMonth, nameYear, mes " +
                        "from [SistemaWeb].[web].[Facturacion] where id_servicio=@id_servicio and  nameYear in ({0})  order by mes asc", anios);
                    var param = new DynamicParameters();
                    param.Add("@id_servicio", id_servicio);
                    lista = cn.Query<Facturacion>(squery, param).ToList();
                }
                return lista;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
