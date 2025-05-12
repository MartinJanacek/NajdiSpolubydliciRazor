using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Enums;

namespace NajdiSpolubydliciRazor.Pages.Offer
{
    public class OffersSearchModel : PageModel
    {
        [BindProperty]
        public District District { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                return RedirectToPage($"../Offer/Offers", new { district = District});
            }
            return Page();
        }
    }
}
