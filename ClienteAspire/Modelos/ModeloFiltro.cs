using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteAspire.Modelos
{
    public class ModeloFiltro
    {

        public string PropiedadLog { get; set; }
        public string NombrePropiedad { get; set; }
        public string Condicion { get; set; }
        public string ValorBusqueda { get; set; }
        public bool EstaVisible { get; set; }
    }
}
