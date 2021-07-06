using Microsoft.AspNetCore.Authentication.Cookies;
using Sushi.Room.Application.Services.DataModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Sushi.Room.Web.Infrastructure.Identity
{
    public class IdentityConfig
    {
        public static ClaimsIdentity CreateClaimsIdentity(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim("UserFullName", $"{user.FirstName} {user.LastName}"),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

            return identity;
        }
    }
}
