using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBS_Mantenimientos_Financial.Api.Controllers
{
    public class AuditLog
    {
        public string Accion { get; set; }
        public string NombreTabla { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public DateTime FechaMaquina { get; set; }
        public DateTime FechaSistema { get; set; }
        public string InterfazCliente { get; set; }
        public string CodigoUsuario { get; set; }
        public string NombreOficina { get; set; }
        public bool RegistroValido { get; set; }


    }
}
