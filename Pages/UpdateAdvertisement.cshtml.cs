using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Pages
{
    [BindProperties]
    public class UpdateAdvertismentModel : PageModel
    {
        public TypeOfAdvertising Type { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné")]
        [EmailAddress(ErrorMessage = "Zadejte email ve správném formátu, pø.: info@najdispolubydlici.cz")]
        public string Email { get; set; } = null!;

        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IOneTimeCode _oneTimeCode;
        private readonly IHasher _hasher;

        public UpdateAdvertismentModel(ApplicationDbContext context, IEmailSender emailSender, IOneTimeCode oneTimeCode, IHasher hasher)
        {
            _context = context;
            _emailSender = emailSender;
            _oneTimeCode = oneTimeCode;
            _hasher = hasher;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Type == TypeOfAdvertising.Offer)
            {
                var offerEntity = await _context.Offers
                                                .Where(o => o.Email == Email)
                                                .FirstOrDefaultAsync();

                if (offerEntity is null)
                {
                    ModelState.AddModelError(nameof(Email), "Nabídka nenalezena");
                    return Page();
                }

                var oneTimeCode = _oneTimeCode.GenerateCode();
                offerEntity.Hash = _hasher.Hash(oneTimeCode, out byte[] salt);
                offerEntity.Salt = salt;
                offerEntity.LastChanged = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _emailSender.SendEmailAsync(Email, "Ovìøení", $"Kód: {oneTimeCode} <br>Kód je aktivní 15 minut").ConfigureAwait(false);

                return RedirectToPage($"/Offer/VerifyEmail", new { id = offerEntity.Id, after = AfterVerification.Update });
            }
            if (Type == TypeOfAdvertising.Demand)
            {
                var demandEntity = await _context.Demands
                                                    .Where(d => d.Email == Email)
                                                    .FirstOrDefaultAsync();

                if (demandEntity is null)
                {
                    ModelState.AddModelError(nameof(Email), "Poptávka nenalezena");
                    return Page();
                }

                var oneTimeCode = _oneTimeCode.GenerateCode();
                demandEntity.Hash = _hasher.Hash(oneTimeCode, out byte[] salt);
                demandEntity.Salt = salt;
                demandEntity.LastChanged = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                await _emailSender.SendEmailAsync(Email, "Ovìøení", $"Kód: {oneTimeCode} <br>Kód je aktivní 15 minut");

                return RedirectToPage("/Demand/VerifyEmail", new { id = demandEntity.Id, after = AfterVerification.Update });
            }

            return Page();
        }
    }
}
