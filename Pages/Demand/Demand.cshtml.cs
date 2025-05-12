using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Models;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Pages.Demand
{
    public class DemandModel : PageModel
    {
        [BindProperty]
        public Guid? Id { get; set; }

        public DemandBaseModel? DemandBase { get; set; }

        public DateTime? CreationDate { get; set; }

        [BindProperty]
        [EmailAddress(ErrorMessage = "Zadejte email ve správném formátu, pø.: info@najdispolubydlici.cz")]
        public string? ContactReceiverEmail { get; set; }

        private readonly ApplicationDbContext _context;

        public DemandModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            var demandEntity = await _context.FindAsync<Entities.Demand>(id);

            if (demandEntity is null || !demandEntity.Active)
            {
                return Redirect("~/NotFound");
            }

            DemandBase = demandEntity;
            CreationDate = demandEntity.CreationDate;
            Id = id;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage($"../ContactReceiver/SendContact", new { email = ContactReceiverEmail, type = TypeOfAdvertising.Demand, id = Id });
        }
    }
}
