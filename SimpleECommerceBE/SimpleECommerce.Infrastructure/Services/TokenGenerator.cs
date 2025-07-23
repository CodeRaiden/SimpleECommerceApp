//using Microsoft.IdentityModel.Tokens;
//using SharedLibPackage.Helper;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Reflection;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace SimpleECommerce.Infrastructure.Services
//{
//    public static class TokenGenerator<T> where T : class
//    {
//        public static string GenerateToken(T user, CancellationToken cancellationToken = default)
//        {
//            if (user == null) throw new ArgumentNullException(nameof(user));

//            // Get values using reflection
//            string firstName = GetPropertyValue(user, "FirstName");
//            string lastName = GetPropertyValue(user, "LastName");
//            string BusinessName = GetPropertyValue(user, "BusinessName");
//            string email = GetPropertyValue(user, "Email");
//            string role = GetPropertyValue(user, "Role");

//            var key = Encoding.UTF8.GetBytes(EnvConfig.AuthKey); // Use appropriate JWT key here
//            var issuer = EnvConfig.AuthIssuer;
//            var lifeTime = EnvConfig.AuthLifeTime;

//            var securityKey = new SymmetricSecurityKey(key);
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var claims = new List<Claim>();

//            if (string.IsNullOrEmpty(BusinessName))
//            {
//                if (!string.IsNullOrWhiteSpace(firstName) || !string.IsNullOrWhiteSpace(lastName))
//                    claims.Add(new Claim(ClaimTypes.Name, $"{firstName} {lastName}".Trim()));
//            }
//            else
//            {
//                if (!string.IsNullOrWhiteSpace(BusinessName))
//                    claims.Add(new Claim(ClaimTypes.Name, BusinessName.Trim()));
//            }

//            if (!string.IsNullOrWhiteSpace(email))
//                claims.Add(new Claim(ClaimTypes.Email, email));

//            if (!string.IsNullOrWhiteSpace(role))
//                claims.Add(new Claim(ClaimTypes.Role, role));

//            var token = new JwtSecurityToken(
//                issuer: issuer,
//                audience: issuer,
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(Convert.ToDouble(lifeTime)),
//                signingCredentials: credentials
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//        private static string GetPropertyValue(T obj, string propertyName)
//        {
//            var prop = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
//            var value = prop?.GetValue(obj)?.ToString();
//            return value ?? string.Empty;
//        }
//    }
//}
