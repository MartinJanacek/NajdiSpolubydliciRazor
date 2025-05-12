using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Models;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace NajdiSpolubydliciRazor.Pages.Demand
{
    [BindProperties]
    public class UpdateModel : PageModel
    {
        [Required]
        public DemandBaseModel DemandForm { get; set; } = null!;

        private readonly ApplicationDbContext _context;
        private readonly IAuth _auth;

        public UpdateModel(ApplicationDbContext context, IAuth auth)
        {
            _context = context;
            _auth = auth;
        }

        public async Task<IActionResult> OnGet()
        {
            var id = _auth.GetIdFromClaim(User.Identity as ClaimsIdentity);

            var demandEntity = await _context.FindAsync<Entities.Demand>(id);

            if (demandEntity is null)
            {
                return Redirect("~/NotFound");
            }

            DemandForm = demandEntity;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var id = _auth.GetIdFromClaim(User.Identity as ClaimsIdentity);

            var demandEntity = await _context.Demands.FindAsync(id);

            if (demandEntity is null)
            {
                return Redirect("~/NotFound");
            }

            demandEntity.PhoneNumber = DemandForm.PhoneNumber;
            demandEntity.District = DemandForm.District;
            demandEntity.Location = DemandForm.Location;
            demandEntity.Text = DemandForm.Text;
            demandEntity.Salt = [];
            demandEntity.LastChanged = DateTime.UtcNow;

            _context.Update(demandEntity);
            _context.SaveChanges();

            return Redirect($"~/Demand/Demand/{demandEntity.Id}");
        }
    }
}

