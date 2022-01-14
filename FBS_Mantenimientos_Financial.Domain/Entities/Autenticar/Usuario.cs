using FBS_CRUD_Generico.Entities;
using FBS_Mantenimientos_Financial.Domain.Modelos.Seguridades.Asociados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBS_Mantenimientos_Financial.Domain.Entities.Autenticar
{
    [Table("USUARIO", Schema = "FBS_SEGURIDADES")]
    [GeneratedController("api/seguridades/usuario")]
    public class Usuario
    {
        [Key]
        [Column("CODIGO")]
        [StringLength(25)]
        public string Codigo { get; set; }

        [Required]
        [Column("NOMBRE")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Column("SECUENCIALOFICINA")]
        public int SecuencialOficina { get; set; }

        [Column("ESTAACTIVO")]
        public bool EstaActivo { get; set; }

        [InverseProperty(nameof(UsuarioComplemento.CodigoUsuarioNavigation))]
        public virtual UsuarioComplemento UsuarioComplementoNavigation { get; set; }
        [InverseProperty(nameof(RefrescarToken.CodigoUsuarioNavigation))]
        public virtual ICollection<RefrescarToken> RefreshTokenNavigation { get; set; }
    }
}
