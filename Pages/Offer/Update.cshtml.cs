using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Models;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace NajdiSpolubydliciRazor.Pages.Offer
{
    [BindProperties]
    public class UpdateModel : PageModel
    {
        [Required]
        public OfferBaseModel OfferForm { get; set; } = null!;

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

            var offerEntity = await _context.FindAsync<Entities.Offer>(id);

            if (offerEntity is null)
            {
                return Redirect("~/NotFound");
            }

            OfferForm = offerEntity;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var id = _auth.GetIdFromClaim(User.Identity as ClaimsIdentity);

            var offerEntity = await _context.FindAsync<Entities.Offer>(id);

            if (offerEntity is null)
            {
                return Redirect("~/NotFound");
            }

            offerEntity.PhoneNumber = OfferForm.PhoneNumber;
            offerEntity.Text = OfferForm.Text;
            offerEntity.District = OfferForm.District;
            offerEntity.Location = OfferForm.Location;
            offerEntity.RentalPrice = OfferForm.RentalPrice;
            offerEntity.Type = OfferForm.Type;
            offerEntity.NumberOfRooms = OfferForm.NumberOfRooms;
            offerEntity.TotalArea = OfferForm.TotalArea;
            offerEntity.AreaForTenant = OfferForm.AreaForTenant;
            offerEntity.Furnished = OfferForm.Furnished;
            offerEntity.Internet = OfferForm.Internet;
            offerEntity.Wifi = OfferForm.Wifi;
            offerEntity.AreAnimalsInPlace = OfferForm.AreAnimalsInPlace;       
            offerEntity.AnimalOwners = OfferForm.AnimalOwners;
            offerEntity.Floor = OfferForm.Floor;
            offerEntity.MoveInDate = OfferForm.MoveInDate;

            await _context.SaveChangesAsync();

            return Redirect("~/Offer/UpdateImages");
        }
    }
}
