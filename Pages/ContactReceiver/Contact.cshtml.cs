using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Services.Interfaces;

namespace NajdiSpolubydliciRazor.Pages.ContactReceiver
{
    public class ContactModel : PageModel
    {
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Error { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly IOneTimeCode _oneTimeCode;

        public ContactModel(ApplicationDbContext context, IOneTimeCode oneTimeCode)
        {
            _context = context;
            _oneTimeCode = oneTimeCode;
        }

        public async Task<IActionResult> OnGet(int id, string code)
        {
            Error = string.Empty;

            var contactEntity = await _context.ContactReceivers
                                    .Where(c => c.Id == id && c.Code == code)
                                    .FirstOrDefaultAsync();

            if (contactEntity is null)
            {
                Error = "Nenalezeno";
                return Page();
            }

            contactEntity.UrlOpened = true;
            await _context.SaveChangesAsync();

            if (_oneTimeCode.IsTooOld(contactEntity.CodeCreated))
            {
                Error = "Vypršela platnost linku";
                return Page();
            }

            if (contactEntity.TypeOfAdvertising == Enums.TypeOfAdvertising.Offer)
            {
                var offer = await _context.FindAsync<Entities.Offer>(contactEntity.AdvertismentId);

                if (offer is null)
                {
                    Error = "Nabídka nenalezena";
                    return Page();
                }

                Email = offer.Email;
                PhoneNumber = offer.PhoneNumber;

                return Page();
            }

            if (contactEntity.TypeOfAdvertising == Enums.TypeOfAdvertising.Demand)
            {
                var demand = await _context.FindAsync<Entities.Demand>(contactEntity.AdvertismentId);

                if (demand is null)
                {
                    Error = "Poptávka nenalezena";
                    return Page();
                }

                Email = demand.Email;
                PhoneNumber = demand.PhoneNumber;

                return Page();
            }

            Error = "Nìco se pokazilo";
            return Page();
        }
    }
}
