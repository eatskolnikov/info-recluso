using Datos.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Datos.Utilities;

public class UtilServices
{
    public static async Task<String> TokenGenerator(UserDto userDto, UserManager<IdentityUser> userManager = default, List<Claim> listClaims = default)
    {
        try
        {
            var config = Injector.GetService<IConfiguration>();
            var repo = Injector.GetService<AccountServices>(true);
            var keys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            var creds = new SigningCredentials(keys, algorithm: SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddDays(365);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,userDto.Email),
                new Claim(ClaimTypes.Expiration, exp.ToString("dd/MM/yyyy")),
            };

            if (userManager is not null)
            {
                var usuario = await userManager?.FindByEmailAsync(userDto?.Email);
                var claimsdb = await userManager?.GetClaimsAsync(usuario);
                var roles = await userManager?.GetRolesAsync(usuario);

                roles.ToList().ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x.ToString())));
                claims?.AddRange(claimsdb);
            }

            if (listClaims is not null && listClaims.Count >= 1)
                claims.AddRange(listClaims);

            var SecurityTokens = new JwtSecurityToken(
                                        issuer: null,
                                        audience: null,
                                        claims: claims,
                                        signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(SecurityTokens);
        }
        catch (Exception)
        {
            throw new Exception("Error Generando Token");
        }
    }
}