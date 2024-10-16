using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherAPI.Models;
using WeatherAPI.Services.Interfaces;

namespace WeatherAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        private static readonly List<UserModel> _users = new List<UserModel>
        {
            new UserModel { Username = "randomuser", Password = "randompassword"}
        };

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = _users.FirstOrDefault(user => user.Username == username && user.Password == password);
            
            if (user == null)
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(UserModel user) 
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            var key = new SymmetricSecurityKey(secretKey);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"], 
                audience: jwtSettings["Audience"], 
                claims: claims, 
                expires: DateTime.Now.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"])), 
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);

        }
    }
}
