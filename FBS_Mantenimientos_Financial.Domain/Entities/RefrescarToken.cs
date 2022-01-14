using FBS_Mantenimientos_Financial.Domain.Entities.Autenticar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBS_Mantenimientos_Financial.Domain.Modelos.Seguridades.Asociados
{
    [Table("REFRESCARTOKEN", Schema = "FBS_SEGURIDADES")]
    public partial class RefrescarToken
    {
        [Key]
        [StringLength(1000)]
        public string Token { get; set; }

        [Column("EXPIRA", TypeName = "datetime")]
        public DateTime Expira { get; set; }

        [Column("CREADO", TypeName = "datetime")]
        public DateTime Creado { get; set; }

        [Column("REVOCADO", TypeName = "datetime")]
        public DateTime? Revocado { get; set; }

        [Required]
        [Column("CODIGOUSUARIO")]
        [StringLength(25)]
        public string CodigoUsuario { get; set; }
        [ForeignKey(nameof(CodigoUsuario))]
        [InverseProperty(nameof(Usuario.RefreshTokenNavigation))]
        public virtual Usuario CodigoUsuarioNavigation { get; set; }
    }
}