using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.Security.Claims;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Entities;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Entities.Interfaces;

namespace NajdiSpolubydliciRazor.Services
{
    public class Auth : IAuth
    {
        public async Task SignInAsync(Guid id, HttpContext httpContext, DateTime EntityLastchanged, TypeOfAdvertising typeOfAdvertising, TypeOfAuthorization typeOfAuthorization)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", id.ToString()),
                new Claim("Type", typeOfAdvertising.ToString()),
                new Claim("LastChanged", EntityLastchanged.ToString()),
                new Claim("Authorization", typeOfAuthorization.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,

                IsPersistent = true
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }

        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public Guid GetIdFromClaim(ClaimsIdentity? claimsIdentity)
        {
            if (claimsIdentity is null) return Guid.Empty;

            Claim? cookieId = claimsIdentity.FindFirst("Id");

            if (cookieId is null) return Guid.Empty;

            return new Guid(cookieId.Value);
        }
    }

    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly ApplicationDbContext _context;

        public CustomCookieAuthenticationEvents( ApplicationDbContext context )
        {
            _context = context;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext cookieValidatePrincipalContext)
        {
            bool reject = false;

            var userPrincipal = cookieValidatePrincipalContext.Principal;

            if (userPrincipal is null)
            {
                reject = true;
            }
            else
            {
                string? lastChanged = userPrincipal.FindFirstValue("LastChanged");

                string? type = userPrincipal.FindFirstValue("Type");

                string? id = userPrincipal.FindFirstValue("Id");

                string? authorization = userPrincipal.FindFirstValue("Authorization");

                if (lastChanged is null || type is null || id is null || authorization is null)
                {
                    reject = true;
                }
                else
                {
                    var entity = await GetAdvertismentEntity(type, id);

                    if (entity is null)
                    {
                        reject = true;
                    }
                    else {

                        if (entity.Active && authorization == TypeOfAuthorization.Publish.ToString()) reject = true;

                        if (entity.LastChanged.ToString() != lastChanged) reject = true;
                    }
                }
            }

            if (reject)
            {
                cookieValidatePrincipalContext.RejectPrincipal();

                await cookieValidatePrincipalContext.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        public async Task<IAdvertismentEntity?> GetAdvertismentEntity(string type, string id)
        {
            if (type == TypeOfAdvertising.Offer.ToString())
            {
                var offerEntity = await _context.FindAsync<Offer>(new Guid(id));

                if (offerEntity is not null) return offerEntity;
            }
            
            if (type == TypeOfAdvertising.Demand.ToString())
            {
                var demandEntity = await _context.FindAsync<Demand>(new Guid(id));

                if (demandEntity is not null) return demandEntity;
            }
            
            return null;
        }
    }
}
