namespace FBS_Mantenimientos_Financial.Domain.Modelos.Autenticar
{
    public class UsuarioLogin
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}