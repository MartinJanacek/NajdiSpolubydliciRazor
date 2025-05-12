using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Entities;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Models;
using NajdiSpolubydliciRazor.Pages.ContactReceiver;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Pages.Offer
{
    public class OfferModel : PageModel
    {
        [BindProperty]
        public Guid? Id { get; set; }

        public Guid? ImagesDirectory { get; set; }

        public OfferBaseModel? OfferBase { get; set; }

        public DateTime? CreationDate { get; set; }

        public int ImageCount { get; set; }

        [BindProperty]
        [EmailAddress(ErrorMessage = "Zadejte email ve správném formátu, pø.: info@najdispolubydlici.cz")]
        public string? ContactReceiverEmail { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public OfferModel(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
            ImageCount = 0;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            var offerEntity = await _context.FindAsync<Entities.Offer>(id);

            if (offerEntity is null || !offerEntity.Active)
            {
                return Redirect("~/NotFound");
            }

            OfferBase = offerEntity;
            CreationDate = offerEntity.CreationDate;
            Id = offerEntity.Id;
            ImagesDirectory = offerEntity.ImagesDirectoryId;
            ImageCount = offerEntity.ImageCount;

            return Page();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }    
            return RedirectToPage($"../ContactReceiver/SendContact", new {email = ContactReceiverEmail,  type = TypeOfAdvertising.Offer, id = Id});
        }
    }
}
