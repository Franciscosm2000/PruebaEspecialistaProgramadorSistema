using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos.Db_Context;
using Modelo.Seguridad;
using Web.Models.Seguridad;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador usuario
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsuariosController : ControllerBase
    {

        private readonly DbContextSistema _context;
        private readonly IConfiguration _config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public UsuariosController(DbContextSistema context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        /// <summary>
        /// METODO PARA CREAR UN USUARIO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] Web.Models.Seguridad.CrearUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usu = model.Usuario.ToLower();

            if (await _context.usuario.AnyAsync(u => u.Usuario == usu))
            {
                return BadRequest("El usuario ya existe");
            }

            CrearPasswordHash(model.password, out byte[] passwordHash, out byte[] passwordSalt);

            UsuarioModel usuario = new UsuarioModel
            {
                IdRol = model.IdRol,
                Usuario = model.Usuario.ToLower(),
                password_hash = passwordHash,
                password_salt = passwordSalt,
                EstadoFila = true
            };

            _context.usuario.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }
        /// <summary>
        /// METODO QUE PERMITE INICIAR SECION Y GENERA UN TOKEN POR TIEMPO LIMITADO PARA REALIZAR PETICIONES A LA API
        /// CREDENCIALES PARA HACER PRUEBA, Usuario: admin y Contraseña: 123
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var usu = model.usuario.ToLower();

            var usuario = await _context.usuario.
                Where(u => u.EstadoFila == true && u.Usuario == model.usuario).
                Include(u => u.rol).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            if (!VerificarPasswordHash(model.password, usuario.password_hash, usuario.password_salt))
            {
                return NotFound("Credenciales invalidas.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario.rol.Nombre ),
                new Claim("idusuario", usuario.IdUsuario.ToString() ),
                new Claim("rol", usuario.rol.Nombre ),
                new Claim("usuario", usuario.Usuario )
            };

            return Ok(
                    new { token = GenerarToken(claims) }
                );

        }

        //Encriptacion de clave
        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
       
        private bool VerificarPasswordHash(string password, byte[] passwordHashAlmacenado, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNuevo = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNuevo));
            }
        }
        
        private string GenerarToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var timeToken = _config.GetValue<string>("TimeToken");

            int time = timeToken.Trim() != null || timeToken != "" ? int.Parse(timeToken) : 2;

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(time),
              signingCredentials: creds,
              claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
