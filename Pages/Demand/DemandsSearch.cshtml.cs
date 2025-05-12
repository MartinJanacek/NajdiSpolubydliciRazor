using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Enums;

namespace NajdiSpolubydliciRazor.Pages.Demand
{
    public class DemandSearchModel : PageModel
    {
        [BindProperty]
        public District District { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                return RedirectToPage($"../Demand/Demands", new { district = District });
            }
            return Page();
        }
    }
}
