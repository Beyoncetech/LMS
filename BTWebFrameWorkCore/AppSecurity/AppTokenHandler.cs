using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BTWebAppFrameWorkCore.AppSecurity
{
    public class AppTokenHandler
    {
        private readonly byte[] SecretKey =  Encoding.ASCII.GetBytes("732b89c6-cab9-4a10-b04c-4ecce6aee510");
        private readonly int TokenExpiryDays = 1;

        public AppTokenHandler()
        {

        }

        public string CreateJWTToken(Dictionary<string, string> Payload)
        {
            string Token = string.Empty;
            var tempPayload = new ClaimsIdentity();
            foreach (var item in Payload)
            {
                tempPayload.AddClaim(new Claim(item.Key, item.Value));
            }            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {                
                Subject = tempPayload,
                Expires = DateTime.UtcNow.AddDays(TokenExpiryDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            Token = tokenHandler.WriteToken(token);
            return Token;
        }

        public async Task<ClaimsPrincipal> ValidateJWTToken(string jwt)
        {            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                //Same Secret key will be used while creating the token
                IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                //Usually, this is your application base URL
                //ValidIssuer = "http://localhost:45092/",
                //ValidateAudience = true,
                //Here, we are creating and using JWT within the same application.
                //In this case, base URL is fine.
                //If the JWT is created using a web service, then this would be the consumer URL.
                //ValidAudience = "http://localhost:45092/",
                RequireExpirationTime = true,
                ValidateLifetime = true,
                //ClockSkew = TimeSpan.Zero
            };

            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler()
                    .ValidateToken(jwt, validationParameters, out var rawValidatedToken);

                await Task.Delay(1);
                return claimsPrincipal;
                // Or, you can return the ClaimsPrincipal
                // (which has the JWT properties automatically mapped to .NET claims)
            }
            catch (SecurityTokenValidationException)
            {
                // The token failed validation!
                // TODO: Log it or display an error.
                //throw new Exception($"Token failed validation: {stvex.Message}");
                return null;
            }
            catch (ArgumentException)
            {
                // The token was not well-formed or was invalid for some other reason.
                // TODO: Log it or display an error.
                //throw new Exception($"Token was invalid: {argex.Message}");
                return null;
            }
        }
    }
}
