using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Services;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.Security.Claims;

namespace NajdiSpolubydliciRazor.Pages.Offer
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuth _auth;
        private readonly IImageDirectory _directoryManipulator;

        public DeleteModel(ApplicationDbContext context, IAuth auth, IImageDirectory directoryManipulator)
        {
            _context = context;
            _auth = auth;
            _directoryManipulator = directoryManipulator;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            var id = _auth.GetIdFromClaim(User.Identity as ClaimsIdentity);

            var offerEntity = await _context.FindAsync<Entities.Offer>(id);

            if (offerEntity is null)
            {
                return Redirect("~/NotFound");
            }

            _context.Remove(offerEntity);
            _context.SaveChanges();

            try
            {
                if (_directoryManipulator.ImageDirectoryExists(offerEntity.ImagesDirectoryId))
                {
                    _directoryManipulator.DeleteDirectoryWithContents(offerEntity.ImagesDirectoryId);
                }
            }
            catch 
            {
                Console.WriteLine($"{_directoryManipulator.GetPath(offerEntity.ImagesDirectoryId)} not deleted");
            }

            return Redirect("~/Deleted");
        }
    }
}
