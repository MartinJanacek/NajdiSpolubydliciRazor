using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.Security.Claims;

namespace NajdiSpolubydliciRazor.Pages.Demand
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuth _auth;

        public DeleteModel(ApplicationDbContext context, IAuth auth)
        {
            _context = context;
            _auth = auth;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            var id = _auth.GetIdFromClaim(User.Identity as ClaimsIdentity);

            var demandEntity = await _context.FindAsync<Entities.Demand>(id);

            if (demandEntity is null)
            {
                return Redirect("~/NotFound");
            }

            _context.Remove(demandEntity);
            _context.SaveChanges();

            return Redirect("~/Deleted");
        }
    }
}
