using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using System.IdentityModel.Tokens.Jwt;

namespace TeachersAndStudents.Web.Helpers
{
    public class MyAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService local;
        private readonly ISessionStorageService session;

        public MyAuthenticationStateProvider(ILocalStorageService local,ISessionStorageService session) {
            this.local = local;
            this.session = session;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var claims= await GetClaimsAsync();
            return (claims is null)?Adduser(claims):Adduser(claims,"mySchema") ;
        }

        private AuthenticationState Adduser(IEnumerable<Claim> claims, string Schemea = null) {
            return new AuthenticationState(
                       new ClaimsPrincipal(
                           new ClaimsIdentity(claims, Schemea)));
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            string token;
            try
            {
                token = await session.GetItemAsync<string>("Token");
                return GetClaimsFromToken(token);
            }
            catch (Exception e)
            {
                try
                {
                    token = await local.GetItemAsync<string>("Token");
                    return GetClaimsFromToken(token);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        private IEnumerable<Claim> GetClaimsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken=tokenHandler.ReadJwtToken(token);
            return jwtSecurityToken.Claims;
        }
    }
}
