using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MasterService.Services
{
    public static class JwtTokenAuthen
    {
        public static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var publicService = new PublicService();
            var key = Encoding.ASCII.GetBytes(publicService.GetConfiguration("JWT"));

            if (Convert.ToBoolean(publicService.GetConfiguration("JwtAuthenState")))
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // This is set to zero to account for clock differences between the token issuer and the server
                };

                try
                {
                    // Validate the token without throwing an exception for expiration
                    SecurityToken validatedToken;
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                    // Check if the token has expired
                    if (validatedToken.ValidTo < DateTime.UtcNow)
                    {
                        // Token has expired
                        // return true;
                        throw new Exception("Session has expired please login again.");
                    }
                }
                catch (SecurityTokenException)
                {
                    // Token validation failed (invalid token format, etc.)
                    // You may handle this case based on your requirements
                    // return true;
                    throw new Exception("Session has expired please login again.");
                }
            }
            // Token is valid and not expired
            return false;
        }

        public static string GenerateToken(string username, int expirationMinutes = 30)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var publicService = new PublicService();
            var key = Encoding.ASCII.GetBytes(publicService.GetConfiguration("JWT"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, username)
                    // Add additional claims as needed
                }),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string HashPassword(string password)
        {
            // Generate a random salt for hashing
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash the password using bcrypt with the salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        // Example usage during login verification
        public static bool VerifyPassword(string hashedPassword, string password)
        {
            // Verify the password against the hashed password stored in the database
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            return isPasswordValid;
        }
    }
}
