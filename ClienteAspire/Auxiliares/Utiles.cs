using ClienteAspire.Modelos;
using FBS_ComponentesDinamicos.Sevices;
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

        public static string AplicarFiltrar(this List<ModeloFiltro> modFiltroList)
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

                                filtroAgregar = $"DateTime.Equals(Convert.ToDateTime(e.{modFil.PropiedadLog}.ToString()).Date, {fechaAComparar})";
                                break;
                            }
                        case "Mayor Que":
                            {
                                var fechaAComparar = DateTime.ParseExact(modFil.ValorBusqueda, Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture).Date;
                                filtroAgregar = $"DateTime.Compare(DateTime.Parse(e.{modFil.PropiedadLog}.ToString()), {fechaAComparar}) > 0";
                                break;
                            }
                        case "Menor Que":
                            {
                                var fechaAComparar = DateTime.ParseExact(modFil.ValorBusqueda, Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture).Date;

                                filtroAgregar = $"DateTime.Compare(DateTime.Parse(e.{modFil.PropiedadLog}.ToString({Constantes.FORMATO_FECHA})), {fechaAComparar}) < 0";
                                break;
                            }
                        case "Rango":
                            {
                                var dates = modFil.ValorBusqueda.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                                var startDate = DateTime.ParseExact(dates[0], Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                var endDate = DateTime.ParseExact(dates[1], Constantes.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                filtroAgregar = $"DateTime.Compare(DateTime.Parse(e.{modFil.PropiedadLog}.ToString()), {endDate}) <= 0 " +
                                    $"and DateTime.Compare(DateTime.Parse(e.{modFil.PropiedadLog}.ToString()), {startDate}) >= 0";
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

    }
}
