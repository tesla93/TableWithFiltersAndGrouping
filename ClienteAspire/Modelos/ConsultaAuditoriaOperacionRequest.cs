using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteAspire.Modelos
{
    public class ConsultaAuditoriaOperacionRequest
    {
        public string Filtro { get; set; }
        public int Cantidad { get; set; }
        public int Omitir { get; set; }
    }
}
