using NajdiSpolubydliciRazor.Enums;
using System.Security.Claims;

namespace NajdiSpolubydliciRazor.Services.Interfaces
{
    public interface IAuth
    {
        public Task SignInAsync(Guid id, HttpContext httpContext, DateTime EntityLastchanged, TypeOfAdvertising typeOfAdvertising, TypeOfAuthorization typeOfAuthorization);

        public Task SignOutAsync(HttpContext httpContext);

        public Guid GetIdFromClaim(ClaimsIdentity? claimsIdentity);
    }
}
