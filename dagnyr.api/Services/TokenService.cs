using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dagnyr.api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace dagnyr.api.Services;

public class TokenService
{
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
    public TokenService(UserManager<User> userManager, IConfiguration config)
    {
            _config = config;
            _userManager = userManager;

    }

    public async Task<string> CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Name, user.UserName),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["tokenSettings:tokenKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var options = new JwtSecurityToken
        (
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(5),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(options);

    }

}
