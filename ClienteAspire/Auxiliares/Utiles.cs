using ClienteAspire.Modelos;
using FBS_ComponentesDinamicos.Sevices;
using Newtonsoft.Json.Linq;
using System.Globalization;

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

        public static string AplicarFiltrar(this IEnumerable<ModeloFiltro> modFiltroList)
        {
            string Filtros = string.Empty;
            
            foreach (var modFil in modFiltroList)
            {
                string filtroAgregar = string.Empty;
                if (!string.IsNullOrEmpty(modFil.ValorBusqueda.Trim()))
                {
                    switch (modFil.Condicion)
                    {
                        case "Contiene":
                            {
                                filtroAgregar = $"e.{modFil.PropiedadLog}.ToLower().Contains(\"{modFil.ValorBusqueda.ToLower()}\")";                              
                                break;
                            }
                        case "Es Igual":
                            {
                                filtroAgregar = $"e.{modFil.PropiedadLog}.ToLower() == (\"{modFil.ValorBusqueda.ToLower()}\")";
                                break;
                            }
                        case "No Es Igual":
                            {
                                filtroAgregar = $"e.{modFil.PropiedadLog}.ToLower() != (\"{modFil.ValorBusqueda.ToLower()}\")";

                                break;
                            }
                        case "Comienza Con":
                            {
                                filtroAgregar = $"e.{modFil.PropiedadLog}.ToLower().StartsWith(\"{modFil.ValorBusqueda.ToLower()}\")";

                                break;
                            }
                        case "Termina Con":
                            {
                                filtroAgregar = $"e.{modFil.PropiedadLog}.ToLower().EndsWith(\"{modFil.ValorBusqueda.ToLower()}\")";
                                break;
                            }
                        case "Igual":
                            {

                                var fechaAComparar = DateTime.ParseExact(modFil.ValorBusqueda, Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture).Date;
                                filtroAgregar = $"DateTime.Compare(e.{modFil.PropiedadLog}.Date, Convert.ToDateTime(\"{fechaAComparar}\")) ==0";
                                break;
                            }
                        case "Mayor Que":
                            {
                                var fechaAComparar = DateTime.ParseExact(modFil.ValorBusqueda, Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture).Date;
                                filtroAgregar = $"DateTime.Compare(e.{modFil.PropiedadLog}.Date, Convert.ToDateTime(\"{fechaAComparar}\")) >0";
                                break;
                            }
                        case "Menor Que":
                            {
                                var fechaAComparar = DateTime.ParseExact(modFil.ValorBusqueda, Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture);                             
                               
                                filtroAgregar = $"DateTime.Compare(e.{modFil.PropiedadLog}.Date, Convert.ToDateTime(\"{fechaAComparar}\")) < 0";
                                break;
                            }
                        case "Rango":
                            {
                                var dates = modFil.ValorBusqueda.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                                var startDate = DateTime.ParseExact(dates[0], Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                var endDate = DateTime.ParseExact(dates[1], Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                filtroAgregar = $"DateTime.Compare(e.{modFil.PropiedadLog}.Date, Convert.ToDateTime(\"{endDate}\")) <= 0 " +
                                    $"and DateTime.Compare(e.{modFil.PropiedadLog}.Date, Convert.ToDateTime(\"{startDate}\")) >= 0";
                                break;
                            }
                        default:
                            break;

                    }

                    
                    if (string.IsNullOrEmpty(Filtros))
                        Filtros = "e => "+ filtroAgregar;
                    else
                        Filtros += $" and {filtroAgregar}";
                }
               
            }
            return Filtros;
        }

        public static List<AuditLog> ModificarJson(List<AuditLog> elementos)
        {
            for (int i = 0; i < elementos.Count(); i++)
            {
                string valorAnteriorArreglado = "";
                string valorNuevoArreglado = "";
                var jsonObject1 = JObject.Parse(elementos[i].ValorAnterior);
                var jsonObject2 = JObject.Parse(elementos[i].ValorNuevo);
                IList<string> keys = jsonObject2.Properties().Select(p => p.Name).ToList();

                foreach (var key in keys)
                {
                    if (jsonObject1[key]?.ToString() != jsonObject2[key].ToString())
                    {
                        if (jsonObject1[key] != null)
                        {
                            valorAnteriorArreglado += $"{key}: {jsonObject1[key] ?? ""}, ";
                        }
                        else
                            valorAnteriorArreglado = String.Empty;
                        valorNuevoArreglado += $"{key}: {jsonObject2[key]}, ";
                    }
                }
                elementos[i].ValorAnterior = valorAnteriorArreglado;
                elementos[i].ValorNuevo = valorNuevoArreglado;
            }
            return elementos;
        }

    }
}
