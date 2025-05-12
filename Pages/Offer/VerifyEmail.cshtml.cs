using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using NajdiSpolubydliciRazor.Enums;

namespace NajdiSpolubydliciRazor.Pages.Offer
{
    public class CreateOfferNextStepModel : PageModel
    {
        [BindProperty]
        [Required]
        public string? OneTimeCode { get; set; }

        [BindProperty]
        [Required]
        public Guid Id { get; set; }

        [BindProperty]
        [Required]
        public AfterVerification AfterVerification { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly IHasher _hasher;
        private readonly IAuth _auth;
        private readonly IOneTimeCode _oneTimeCode;

        public CreateOfferNextStepModel(ApplicationDbContext context, IHasher hasher, IAuth auth, IOneTimeCode oneTimeCode)
        {
            _context = context;
            _hasher = hasher;
            _auth = auth;
            _oneTimeCode = oneTimeCode;
        }

        public void OnGet(Guid id, AfterVerification after)
        {
            Id = id;
            AfterVerification = after;
        }

        public async Task<IActionResult> OnPost()
        {
            var offerEntity = await _context.FindAsync<Entities.Offer>(Id);

            if (offerEntity is null || OneTimeCode is null)
            {
                return Page();
            }

            if (_oneTimeCode.IsTooOld(offerEntity.LastChanged))
            {
                ModelState.AddModelError(nameof(OneTimeCode), "Èas na ovìøení vypršel");
                return Page();
            }

            if (_hasher.Verify(OneTimeCode, offerEntity.Hash, offerEntity.Salt))
            {
                offerEntity.Hash = string.Empty;

                _context.Update(offerEntity);
                await _context.SaveChangesAsync();

                if (AfterVerification == AfterVerification.Create)
                {
                    await _auth.SignInAsync(Id, HttpContext, offerEntity.LastChanged, TypeOfAdvertising.Offer, TypeOfAuthorization.Publish);
                    return Redirect($"~/Offer/PublishOffer");
                }
                else
                {
                    await _auth.SignInAsync(Id, HttpContext, offerEntity.LastChanged, TypeOfAdvertising.Offer, TypeOfAuthorization.Change);

                    if (AfterVerification == AfterVerification.Delete)
                    {
                        return RedirectToPage($"../Offer/Delete");
                    }
                    if (AfterVerification == AfterVerification.Update)
                    {
                        return RedirectToPage("../Offer/Update");
                    }
                }
            }

            return Page();
        }
    }
}
