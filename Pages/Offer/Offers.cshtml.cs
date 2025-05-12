using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;

namespace NajdiSpolubydliciRazor.Pages.Offer
{
    public class OffersModel : PageModel
    {
        [BindProperty]
        public District District {  get; set; } 

        [BindProperty]
        public int? MaxPrice { get; set; }

        [BindProperty]
        public DateTime? MoveInDateBefore { get; set; }

        public List<Entities.Offer>? Offers { get; set; }

        private readonly ApplicationDbContext _context;

        public OffersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(District district, int? maxPrice, DateTime? moveInDateBefore)
        {
            Offers = await GetFilteredOffers(district, maxPrice, moveInDateBefore);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                Offers = await GetFilteredOffers(District, MaxPrice, MoveInDateBefore);
            }

            return Page();
        }

        private async Task<List<Entities.Offer>?> GetFilteredOffers(District district, int? maxPrice, DateTime? moveInDateBefore)
        {
            var offerQuery = _context.Offers.Where(o => o.District == district && o.Active);
            
            if (maxPrice is not null)
            {
                offerQuery = offerQuery.Where(o => o.RentalPrice <= maxPrice);
            }
            if (moveInDateBefore is not null)
            {
                offerQuery = offerQuery.Where(o => o.MoveInDate <= moveInDateBefore);
            }

            return await offerQuery.AsNoTracking().ToListAsync();
        }
    }
}
