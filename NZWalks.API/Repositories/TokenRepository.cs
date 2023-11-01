using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace NZWalks.API.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;
    public TokenRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string CreateJWTToken(IdentityUser user, List<string> roles)
    {
        //Create claims
        var claims = new List<Claim>();
        //Add email to the claims
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        foreach (var role in roles)
        {
            //Add role to the claims
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        //Add user id to the claims
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        //Create credentials
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //Create token
        var token = new JwtSecurityToken(
            _configuration["JWT:Issuer"],
            _configuration["JWT:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
            );
        //Return token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}