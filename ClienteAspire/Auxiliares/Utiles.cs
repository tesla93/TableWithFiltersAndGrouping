using ClienteAspire.Modelos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteAspire.Auxiliares
{
    public static class Utiles
    {
        public static string BorrarTodosLosCaracteresDespuesDe(this string cadena, char caracter)
        {
            string cadenaDevolver = string.Empty;
            int index = cadena.IndexOf(caracter);
            if (index >= 0)
            {
                cadenaDevolver = cadena.Substring(0, index).Trim();
            }
            return cadenaDevolver;
        }

        public static string ExtraerCadenaEntreDosComas(this string cadena)
        {
            int sFrom = cadena.IndexOf(",") + 1;
            int sTo = cadena.LastIndexOf(",");
            return cadena.Substring(sFrom, sTo - sFrom).Trim();
        }

        public static List<AuditLog> AplicarFiltrar(this List<AuditLog> elementos, List<ModeloFiltro> modFiltroList)
        {
            List<AuditLog> listaElementosfiltrados = new List<AuditLog>(elementos);
            foreach (var modFil in modFiltroList)
            {
                if (!string.IsNullOrEmpty(modFil.ValorBusqueda.Trim()))
                {
                    switch (modFil.Condicion)
                    {
                        case "Contiene":
                            {
                                listaElementosfiltrados = listaElementosfiltrados
                                .Where(e => e.GetType().GetProperty(modFil.PropiedadLog).GetValue(e).ToString().ToLower()
                                .Contains(modFil.ValorBusqueda.ToLower(), StringComparison.Ordinal)).ToList();
                                break;
                            }
                        case "Es Igual":
                            {
                                listaElementosfiltrados = listaElementosfiltrados
                                .Where(e => e.GetType().GetProperty(modFil.PropiedadLog).GetValue(e).ToString().ToLower() == (modFil.ValorBusqueda.ToLower()))
                                .ToList();
                                break;
                            }
                        case "No Es Igual":
                            {
                                listaElementosfiltrados = listaElementosfiltrados
                                .Where(e => e.GetType().GetProperty(modFil.PropiedadLog).GetValue(e).ToString().ToLower() != (modFil.ValorBusqueda.ToLower()))
                                .ToList();
                                break;
                            }
                        case "Comienza Con":
                            {
                                listaElementosfiltrados = listaElementosfiltrados
                                .Where(x => x.GetType().GetProperty(modFil.PropiedadLog).GetValue(x).ToString().ToLower().StartsWith(modFil.ValorBusqueda.ToLower()))
                                .ToList();
                                break;
                            }
                        case "Termina Con":
                            {
                                listaElementosfiltrados = listaElementosfiltrados
                                .Where(x => x.GetType().GetProperty(modFil.PropiedadLog).GetValue(x).ToString().ToLower().EndsWith(modFil.ValorBusqueda.ToLower()))
                                .ToList();
                                break;
                            }
                        case "Igual":
                            {

                                var fechaAComparar = DateTime.ParseExact(modFil.ValorBusqueda, Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture).Date;
                                listaElementosfiltrados = listaElementosfiltrados
                                  .Where(x => DateTime.Equals(Convert.ToDateTime(x.GetType().GetProperty(modFil.PropiedadLog).GetValue(x).ToString()).Date, fechaAComparar))
                                  .ToList();
                                break;
                            }
                        case "Mayor Que":
                            {
                                var fechaAComparar = DateTime.ParseExact(modFil.ValorBusqueda, Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture).Date;
                                listaElementosfiltrados = listaElementosfiltrados
                                .Where(x => DateTime.Compare(DateTime.Parse(x.GetType().GetProperty(modFil.PropiedadLog).GetValue(x).ToString()), fechaAComparar) > 0)
                                .ToList();
                                break;
                            }
                        case "Menor Que":
                            {
                                var fechaAComparar = DateTime.ParseExact(modFil.ValorBusqueda, Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture).Date;
                                listaElementosfiltrados = listaElementosfiltrados
                                .Where(x => DateTime.Compare((DateTime)x.GetType().GetProperty(modFil.PropiedadLog).GetValue(x), fechaAComparar) < 0)
                                .ToList();
                                break;
                            }
                        case "Rango":
                            {
                                var dates = modFil.ValorBusqueda.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                                var startDate = DateTime.ParseExact(dates[0], Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                var endDate = DateTime.ParseExact(dates[1], Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                listaElementosfiltrados = listaElementosfiltrados
                                .Where(x => DateTime.Compare(DateTime.Parse(x.GetType().GetProperty(modFil.PropiedadLog).GetValue(x).ToString()), endDate) <= 0
                                && DateTime.Compare((DateTime)x.GetType().GetProperty(modFil.PropiedadLog).GetValue(x), startDate) >= 0)
                                .ToList();
                                break;
                            }
                        default:
                            break;

                    }
                }
            }
            return listaElementosfiltrados;
        }

    }
}
