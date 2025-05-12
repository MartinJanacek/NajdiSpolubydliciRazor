using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;

namespace NajdiSpolubydliciRazor.Pages.Demand
{
    public class DemandsModel : PageModel
    {
        [BindProperty]
        public District District { get; set; }

        public List<Entities.Demand>? Demands { get; set; }

        private readonly ApplicationDbContext _context;

        public DemandsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(District district)
        {
            Demands = await _context.Demands
                                .Where(d => d.Active && d.District == district)
                                .AsNoTracking()
                                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPost(District district)
        {
            Demands = await _context.Demands
                                .Where(d => d.Active && d.District == district)
                                .AsNoTracking()
                                .ToListAsync();

            return Page();
        }
    }
}
