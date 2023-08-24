using ConcordiaStation.WebApp.SecurityServices.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConcordiaStation.WebApp.SecurityServices
{
    public class ServiceToken : IServiceToken
    {
        private readonly string _key;
        private readonly byte[] _encodingKey;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public ServiceToken(string key)
        {
            _key = key;
            _tokenHandler = new JwtSecurityTokenHandler();
            _encodingKey = Encoding.ASCII.GetBytes(_key);
        }

        public string GenerateToken(string email, int? id, int expirationMinutes = 480)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("id", id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_encodingKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_encodingKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                _tokenHandler.ValidateToken(token, validationParameters, out _);
                return !IsTokenInExpiredList(token, "blacklisttoken.json");
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }

        public void AddExpiredToken(string token, string filePath)
        {
            List<string> expiredTokens = new();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                expiredTokens = JsonConvert.DeserializeObject<List<string>>(json);
            }

            expiredTokens.Add(token);

            string updatedJson = JsonConvert.SerializeObject(expiredTokens);
            File.WriteAllText(filePath, updatedJson);
        }

        private static bool IsTokenInExpiredList(string token, string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var expiredTokens = JsonConvert.DeserializeObject<List<string>>(json);
                return expiredTokens.Contains(token);
            }
            return false;
        }
    }
}
