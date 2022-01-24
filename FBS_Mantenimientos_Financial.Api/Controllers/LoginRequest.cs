using System;

namespace FBS_Mantenimientos_Financial.Api.Controllers
{
    class LoginRequest
    {
        public string Usuario { get; set; }

        public string Password { get; set; }

        public bool UsaHuellaDigital { get; set; }

        public string[] Direcciones { get; set; }

        public string Maquina { get; set; }

        public int NumeroDeIntento { get; set; }

        public string IPMaquinaIngreso { get; set; }
        public DateTime? FechaSistemaCliente { get; set; }
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}

