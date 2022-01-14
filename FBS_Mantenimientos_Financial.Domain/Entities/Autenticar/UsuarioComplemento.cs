using FBS_CRUD_Generico.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FBS_Mantenimientos_Financial.Domain.Entities.Autenticar
{
    [Table("USUARIO_COMPLEMENTO", Schema = "FBS_SEGURIDADES")]
    [GeneratedController("api/seguridades/usuariocomplemento")]
    public class UsuarioComplemento
    {
        [Key]
        [Column("CODIGOUSUARIO")]
        [StringLength(25)]
        public string CodigoUsuario { get; set; }

        [Column("SECUENCIALPERSONA")]
        public int SecuencialPersona { get; set; }

        [Column("FECHACREACION", TypeName = "datetime")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Column("CLAVE")]
        [StringLength(500)]
        public string Clave { get; set; }

        [Required]
        [Column("EMAILINTERNO")]
        [StringLength(100)]
        public string EmailInterno { get; set; }

        [Column("FECHAULTIMOCAMBIOCLAVE", TypeName = "datetime")]
        public DateTime FechaUltimoCambioClave { get; set; }

        [Column("CAMBIOCLAVEPROXIMOINGRESO")]
        public bool CambioClaveProximoIngreso { get; set; }

        [Column("PERIODICIDADCAMBIOCLAVE")]
        public int PeriodicidadCambioClave { get; set; }

        [Column("ESINTERNO")]
        public bool EsInterno { get; set; }

        [Column("NUMEROVERIFICADOR")]
        [ConcurrencyCheck]
        [JsonIgnore]
        public int NumeroVerificador { get; set; }

        [Column("PUEDECONSULTAROTROSUSUARIOS")]
        public bool PuedeConsultarOtrosUsuarios { get; set; }

        [Column("PUEDEVISUALIZAROTROSUSUARIOS")]
        public bool PuedeVisualizarOtrosUsuarios { get; set; }

        [Column("RECIBEMENSAJES")]
        public bool RecibeMensajes { get; set; }

        [Column("MINUTOSPARAREVISIONMENSAJES")]
        public int MinutosParaRevisionMensajes { get; set; }

        [Column("PUEDEINGRESARSISTEMA")]
        public bool PuedeIngresarSistema { get; set; }

        [Column("PRIORIDADACCESO")]
        public int PrioridadAcceso { get; set; }

        [ForeignKey(nameof(CodigoUsuario))]
        [InverseProperty(nameof(Usuario.UsuarioComplementoNavigation))]
        [JsonIgnore]
        public virtual Usuario CodigoUsuarioNavigation { get; set; }
    }
}
