using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Helpers.Interfaces;
using NajdiSpolubydliciRazor.Models;
using NajdiSpolubydliciRazor.Services.Interfaces;

namespace NajdiSpolubydliciRazor.Pages.Offer
{
    public class CreateOfferModel : PageModel
    {
        [BindProperty]
        public OfferBaseModel OfferForm { get; set; } = null!;

        private readonly ApplicationDbContext _context;
        private readonly ISequentialGuid _sequentialGuid;
        private readonly IEmailSender _emailSender;
        private readonly IOneTimeCode _codeGenerator;
        private readonly IHasher _hasher;

        public CreateOfferModel(ApplicationDbContext context, ISequentialGuid sequentialGuid, IEmailSender emailSender, IOneTimeCode codeGenerator, IHasher hasher)
        {
            _context = context;
            _sequentialGuid = sequentialGuid;
            _emailSender = emailSender;
            _codeGenerator = codeGenerator;
            _hasher = hasher;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _context.Offers.AnyAsync(o => o.Email == OfferForm.Email))
            {
                ModelState.AddModelError("OfferForm.Email", $"Email {OfferForm.Email} se již pro nabídku užívá. Pokud je váš, mùžete nabídku smazat");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var oneTimeCode = _codeGenerator.GenerateCode();

            Entities.Offer offerEntity = new(
                _sequentialGuid.CreateSequentialGuidForNewEntity(),
                _hasher.Hash(oneTimeCode, out byte[] salt),
                salt,
                OfferForm
            );

            await _context.AddAsync(offerEntity);
            await _context.SaveChangesAsync();

            await _emailSender.SendEmailAsync(offerEntity.Email, "Ovìøení emailu", $"Kód: {oneTimeCode} <br>Kód je aktivní 15 minut");
            
            return RedirectToPage($"../Offer/VerifyEmail", new { id = offerEntity.Id, after = AfterVerification.Create });
        }
    }
}
