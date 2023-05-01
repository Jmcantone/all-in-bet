using Microsoft.IdentityModel.Tokens;
using AllinBetApp.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Firebase.Database;
using Firebase.Database.Query;

namespace AllinBetApp.Api.Services
{
    public class AuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly FirebaseClient _firebaseClient;
        private readonly ILogger<FirebaseService> _logger;

        public AuthenticationService(IConfiguration configuration, ILogger<FirebaseService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var firebaseUrl = configuration.GetSection("Firebase:DatabaseUrl").Value;

            _firebaseClient = new FirebaseClient(firebaseUrl);
        }

        public bool Register(User user)
        {
            var userId = Guid.NewGuid().ToString();

            var newUser = new User
            {
                Id = userId,
                Name = user.Name,
                Password = user.Password
            };

            try
            {
                var response = _firebaseClient.Child("users").Child(userId).PutAsync(newUser);

                return response.IsCompletedSuccessfully;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                return false;
            }
        }

        public string Login(string userName, string password)
        {
            if (IsValidUser(userName, password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, userName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            return null;
        }

        private bool IsValidUser(string userName, string password)
        {
            var userQuery = _firebaseClient
                .Child("users")
                .OrderBy("Name")
                .EqualTo(userName)
                .OnceAsync<User>().Result.FirstOrDefault();

            if (userQuery != null)
            {
                var user = userQuery.Object;

                if (user.Password == password)
                {
                    return true;
                }
            }

            return false;
        }
    }
}