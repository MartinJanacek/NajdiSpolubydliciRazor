using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Helpers.Interfaces;
using NajdiSpolubydliciRazor.Models;
using NajdiSpolubydliciRazor.Services.Interfaces;

namespace NajdiSpolubydliciRazor.Pages.Demand
{
    public class CreateDemandModel : PageModel
    {
        [BindProperty]
        public DemandBaseModel DemandForm { get; set; } = null!;

        private readonly ApplicationDbContext _context;
        private readonly ISequentialGuid _sequentialGuid;
        private readonly IEmailSender _emailSender;
        private readonly IOneTimeCode _codeGenerator;
        private readonly IHasher _hasher;

        public CreateDemandModel(ApplicationDbContext context, ISequentialGuid sequentialGuid, IEmailSender emailSender, IOneTimeCode codeGenerator, IHasher hasher)
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

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var oneTimeCode = _codeGenerator.GenerateCode();

            Entities.Demand demandEntity = new(
                _sequentialGuid.CreateSequentialGuidForNewEntity(),
                _hasher.Hash(oneTimeCode, out byte[] salt),
                salt,
                DemandForm
            );

            await _context.AddAsync(demandEntity);
            await _context.SaveChangesAsync();

            await _emailSender.SendEmailAsync(demandEntity.Email, "Ovìøení emailu", $"Kód: {oneTimeCode} <br>Kód je aktivní 15 minut");

            return RedirectToPage($"../Demand/VerifyEmail", new { id = demandEntity.Id, AfterVerification.Create });
        }
    }
}
