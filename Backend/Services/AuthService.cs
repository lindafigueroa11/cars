using Backend.DTOs.Auth;
using Backend.Models;
using Backend.Repository.Interfaces;
using Backend.Services.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<AuthResponseDTO> LoginWithGoogle(LoginGoogleDTO dto)
        {
            // 1️⃣ Validar token con Google
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                dto.IdToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _config["GoogleAuth:ClientId"] }
                }
            );

            // 2️⃣ Buscar usuario
            var user = await _userRepo.GetByEmailAsync(payload.Email);

            // 3️⃣ Crear usuario si no existe
            if (user == null)
            {
                user = new User
                {
                    Name = payload.Name,
                    UserName = payload.Email.Split('@')[0],
                    Email = payload.Email,
                    IsActive = true,
                    SaleCar = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepo.AddAsync(user);
            }

            // 4️⃣ Generar JWT
            var token = GenerateJwt(user);

            return new AuthResponseDTO
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Name = user.Name
            };
        }

        private string GenerateJwt(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", user.Name)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:ExpireMinutes"]!)
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}