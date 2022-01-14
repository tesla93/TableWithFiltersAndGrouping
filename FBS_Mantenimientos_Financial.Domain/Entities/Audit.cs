using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBS_Mantenimientos_Financial.Domain.Entities
{
    [Table("AUDITS", Schema = "FBS_NOMINA")]
    public class Audit
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("NOMBRESCHEMMA")]
        public string NombreSchemma { get; set; }
        [Column("NOMBRETABLA")]
        public string NombreTabla { get; set; }
        [Column("CODIGOUSUARIO")]
        public string CodigoUsuario { get; set; }
        [Column("LLAVES")]
        public string Llaves { get; set; }
        [Column("VIEJOSVALORES")]
        public string ViejosValores { get; set; }
        [Column("NUEVOSVALORES")]
        public string NuevosValores { get; set; }
        [Column("FECHAMAQUINA")]
        public DateTime FechaMaquina { get; set; }
        [Column("FECHASISTEMA")]
        public DateTime FechaSistema { get; set; }

    }
}



//public int Id { get; set; }
//public string NombreSchemma { get; set; }
//public string NombreTabla { get; set; }
//public string CodigoUsuario { get; set; }
//public string Llaves { get; set; }
//public string ViejosValores { get; set; }
//public string NuevosValores { get; set; }
//public DateTime FechaMaquina { get; set; }
//public DateTime FechaSistema { get; set; }