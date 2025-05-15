using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Pages.ContactReceiver
{
    public class VerifyEmailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IOneTimeCode _oneTimeCode;

        public VerifyEmailModel(ApplicationDbContext context, IEmailSender emailSender, IOneTimeCode oneTimeCode)
        {
            _context = context;
            _emailSender = emailSender;
            _oneTimeCode = oneTimeCode;
        }

        public async Task<IActionResult> OnGet(string email, TypeOfAdvertising type, Guid id)
        {
            if (!new EmailAddressAttribute().IsValid(email))
            {
                return Page();
            }

            var receiver = await _context.ContactReceivers
                                    .Where(r => r.Email == email)
                                    .OrderBy(r => r.CodeCreated)
                                    .LastOrDefaultAsync();

            if (receiver is null || _oneTimeCode.IsTooOld(receiver.CodeCreated) || receiver.UrlOpened)
            {
                var code = _oneTimeCode.GenerateCode();

                var newReceiver = new Entities.ContactReceiver(email, type, id, code);

                await _context.AddAsync(newReceiver);
                await _context.SaveChangesAsync();

                string url = $"{Request.Scheme}://{Request.Host}/ContactReceiver/Contact?id={newReceiver.Id}&code={code}";
                await _emailSender.SendEmailAsync(email, "Kontakt", $"Link: <a href='{url}'>{url}</a> <br>Link je aktivní 15 minut").ConfigureAwait(false);

                return Redirect("~/ContactReceiver/ContactSent");
            }
            if (!receiver.UrlOpened && !_oneTimeCode.IsTooOld(receiver.CodeCreated))
            {
                return Redirect("~/ContactReceiver/OpenLastEmail");
            }

            return Page();
        }
    }
}
