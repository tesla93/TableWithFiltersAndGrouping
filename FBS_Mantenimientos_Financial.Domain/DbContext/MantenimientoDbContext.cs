using FBS_CRUD_Generico.Contexts;
using FBS_Mantenimientos_Financial.Domain.Entities.Autenticar;
using FBS_Mantenimientos_Financial.Domain.Modelos.Seguridades.Asociados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBS_Mantenimientos_Financial.Domain.DbContext
{
    public class MantenimientoDbContext: ApplicationDbContext
    {
        public MantenimientoDbContext(DbContextOptions<MantenimientoDbContext> opciones, HistoryDbContext historyDbContext) : base(opciones, historyDbContext)
        {
        }

        public DbSet<RefrescarToken> RefreshTokens { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
