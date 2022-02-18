using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using teachersAndStudents.API.Helpers;

namespace teachersAndStudents.API.Services
{ 
    public interface IAuthService 
    {
        public Task<string> CreateJwtToken(IdentityUser user);
        public Task addRoles();
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Jwt _jwt;
        public AuthService(IOptions<Jwt> jwt ,  UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager) { 
            this._jwt = jwt.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task addRoles() {
            var roles = new string[] { "Student", "Teacher" };
            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                    return ;
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper(),
                });
            }
        }

        public async Task<string> CreateJwtToken(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(_jwt.Key());
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
