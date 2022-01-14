using System;
using System.Text.Json.Serialization;

namespace FBS_Mantenimientos_Financial.Domain.Entities
{
    public class AutenticacionModel
    {
        public bool EstaAutenticado { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Token { get; set; }

        [JsonIgnore]
        public string RefrescarToken { get; set; }

        public DateTime RefrescarTokenExpiracion { get; set; }
    }
}