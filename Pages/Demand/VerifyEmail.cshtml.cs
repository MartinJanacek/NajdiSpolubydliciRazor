using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;

namespace NajdiSpolubydliciRazor.Pages.Demand
{
    public class VerifyEmailModel : PageModel
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

        public VerifyEmailModel(ApplicationDbContext context, IHasher hasher, IAuth auth, IOneTimeCode oneTimeCode)
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
            var demandEntity = await _context.FindAsync<Entities.Demand>(Id);

            if (demandEntity is null || OneTimeCode is null)
            {
                return Page();
            }

            if (_oneTimeCode.IsTooOld(demandEntity.LastChanged))
            {
                ModelState.AddModelError(nameof(OneTimeCode), "Èas na ovìøení vypršel");
                return Page();
            }

            if (_hasher.Verify(OneTimeCode, demandEntity.Hash, demandEntity.Salt))
            {
                demandEntity.Active = true;
                demandEntity.Hash = string.Empty;
                demandEntity.LastChanged = DateTime.UtcNow;

                _context.Update(demandEntity);
                await _context.SaveChangesAsync();

                if (AfterVerification == AfterVerification.Create)
                {
                    return Redirect($"~/Demand/Demand/{demandEntity.Id}");
                }
                else
                {
                    await _auth.SignInAsync(Id, HttpContext, demandEntity.LastChanged, TypeOfAdvertising.Demand, TypeOfAuthorization.Change);

                    if (AfterVerification == AfterVerification.Delete)
                    {
                        return RedirectToPage("../Demand/Delete");
                    }
                    if (AfterVerification == AfterVerification.Update)
                    {
                        return RedirectToPage("../Demand/Update");
                    }
                }
            }

            return Page();
        }
    }
}
