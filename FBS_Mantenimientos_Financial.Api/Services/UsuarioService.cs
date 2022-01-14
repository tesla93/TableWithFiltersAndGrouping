using FBS_Mantenimientos_Financial.Domain.DbContext;
using FBS_Mantenimientos_Financial.Domain.Entities;
using FBS_Mantenimientos_Financial.Domain.Entities.Autenticar;
using FBS_Mantenimientos_Financial.Domain.Modelos.Autenticar;
using FBS_Mantenimientos_Financial.Domain.Modelos.Seguridades.Asociados;
using Financial2_5.Encriptacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FBS_Mantenimientos_Financial.Api.Services
{
    public interface IUsuarioService
    {
        Task<AutenticacionModel> Autenticar(UsuarioLogin usuarioDto);

        Task<AutenticacionModel> RefrescarTokenAsync(string token);

        Task<bool> RevocarToken(string token);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly MantenimientoDbContext _contexto;
        private readonly IConfiguration _configuration;
        private readonly ITextHasher _textHasher;

        public UsuarioService(MantenimientoDbContext contexto, IConfiguration configuration, ITextHasher textHasher)
        {
            _contexto = contexto;
            _configuration = configuration;
            _textHasher = textHasher;
        }

        public async Task<AutenticacionModel> Autenticar(UsuarioLogin usuarioDto)
        {
            var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(x => x.Codigo.ToUpper() == usuarioDto.Codigo.ToUpper());
            if (usuario != null)
            {
                try
                {
                    var comprobacion = _textHasher.Check(usuario.UsuarioComplementoNavigation.Clave, usuarioDto.Clave);
                    if (comprobacion.Verified)
                    {
                        var refrescarToken = CrearRefrescarToken();
                        usuario.RefreshTokenNavigation.Add(refrescarToken);
                        _contexto.Update(usuario);
                        await _contexto.SaveChangesAsync();
                        return new AutenticacionModel()
                        {
                            EstaAutenticado = true,
                            Token = GenerateJwtToken(usuario),
                            Codigo = usuario.Codigo,
                            Nombre = usuario.Nombre,
                            RefrescarToken = refrescarToken.Token,
                            RefrescarTokenExpiracion = refrescarToken.Expira,
                        };
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"La clave para este usuario es incorrecta");
                }

            }
            return null;
        }

        public async Task<AutenticacionModel> RefrescarTokenAsync(string tokenToRefrescar)
        {
            var usuario = await _contexto.Usuarios.SingleOrDefaultAsync(u => u.RefreshTokenNavigation.Any(t => t.Token == tokenToRefrescar));
            if (usuario == null)
            {
                return new AutenticacionModel() { EstaAutenticado = false };
            }

            var refrescarToken = usuario.RefreshTokenNavigation.Single(x => x.Token == tokenToRefrescar);

            if (!(refrescarToken.Revocado == null && DateTime.Now <= refrescarToken.Expira))
            {
                return new AutenticacionModel() { EstaAutenticado = false };
            }

            //Revocar el refrescar token actual
            refrescarToken.Revocado = DateTime.Now;

            //Generar nuevo refrescar token y guardar en bd
            var nuevoTokenRefrescado = CrearRefrescarToken();
            usuario.RefreshTokenNavigation.Add(nuevoTokenRefrescado);
            _contexto.Update(usuario);
            await _contexto.SaveChangesAsync();

            //Generates new jwt
            return new AutenticacionModel()
            {
                EstaAutenticado = true,
                Token = GenerateJwtToken(usuario),
                Codigo = usuario.Codigo,
                Nombre = usuario.Nombre,
                RefrescarToken = nuevoTokenRefrescado.Token,
                RefrescarTokenExpiracion = nuevoTokenRefrescado.Expira
            };
        }

        public async Task<bool> RevocarToken(string token)
        {
            var usuario = await _contexto.Usuarios.SingleOrDefaultAsync(u => u.RefreshTokenNavigation.Any(t => t.Token == token));

            // return false if no user found with token
            if (usuario == null) return false;

            var refrescarToken = usuario.RefreshTokenNavigation.Single(x => x.Token == token);

            // return false if token is not active
            if (!(refrescarToken.Revocado == null && DateTime.Now <= refrescarToken.Expira))
                return false;

            // revoke token and save
            refrescarToken.Revocado = DateTime.Now;
            _contexto.Update(usuario);
            await _contexto.SaveChangesAsync();

            return true;
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtKey"]);
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, usuario.Codigo) };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefrescarToken CrearRefrescarToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return new RefrescarToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expira = DateTime.Now.AddMinutes(Double.Parse(_configuration["DuracionEnMinutos"])),
                    Creado = DateTime.Now
                };
            }
        }
    }
}