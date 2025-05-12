using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Services.Interfaces;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace NajdiSpolubydliciRazor.Pages.Offer
{
    public class VerifyEmailModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Povinné")]
        public IFormFile? ProfilePhoto { get; set; }

        [BindProperty]
        public List<IFormFile> Photos { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly IImageManipulator _imageManipulator;
        private readonly IImageDirectory _directoryManipulator;
        private readonly IAuth _auth;

        public VerifyEmailModel(ApplicationDbContext context, IImageManipulator imageResizer, IImageDirectory directoryManipulator, IAuth auth)
        {
            _context = context;
            _imageManipulator = imageResizer;
            _directoryManipulator = directoryManipulator;
            _auth = auth;
            Photos = [];
        }

        public async Task<IActionResult> OnGet()
        {
            var offerEntity = await _context.Offers.FindAsync(_auth.GetIdFromClaim(User.Identity as ClaimsIdentity));

            if (offerEntity is null)
            {
                return Redirect("~/Error");
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid || ProfilePhoto is null)
            {
                return Page();
            }

            var id = _auth.GetIdFromClaim(User.Identity as ClaimsIdentity);

            var offerEntity = await _context.Offers.FindAsync(id);

            if (offerEntity is null)
            {
                return Redirect("~/NotFound");
            }
            if (_directoryManipulator.ImageDirectoryExists(offerEntity.ImagesDirectoryId))
            {
                await Task.Delay(15000);
                return Redirect($"~/Offer/Offer/{offerEntity.Id}");
            }


            if (!_imageManipulator.HasPermitedImageFileSiqnature(ProfilePhoto.OpenReadStream(), ProfilePhoto.ContentType))
            {
                ModelState.AddModelError(nameof(ProfilePhoto), $"{ProfilePhoto.FileName} - Tento soubor nepøíjmeme \t");
            }

            for (int i = 0; i < Photos.Count; i++)
            {
                if (!_imageManipulator.HasPermitedImageFileSiqnature(Photos[i].OpenReadStream(), Photos[i].ContentType))
                {
                    ModelState.AddModelError(nameof(Photos), $"{Photos[i].FileName} - Tento soubor nepøíjmeme \t");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var path = _directoryManipulator.GetPath(offerEntity.ImagesDirectoryId);

                Directory.CreateDirectory(path);

                using Image thumbnail = await Image.LoadAsync(ProfilePhoto.OpenReadStream());
                {
                    _imageManipulator.Resize(thumbnail, ImageFunctionOnWeb.Carousel);
                    await _imageManipulator.SaveImageAsync(thumbnail, path, "0");

                    _imageManipulator.Resize(thumbnail, ImageFunctionOnWeb.Thumbnail);
                    await _imageManipulator.SaveImageAsync(thumbnail, path, "thumbnail");
                }

                for (int i = 0; i < Photos.Count; i++)
                {
                    using Image image = await Image.LoadAsync(Photos[i].OpenReadStream());
                    {
                        _imageManipulator.Resize(image, ImageFunctionOnWeb.Carousel);
                        await _imageManipulator.SaveImageAsync(image, path, (i + 1).ToString());
                    }
                }
            }
            catch (Exception)
            {
                if (_directoryManipulator.ImageDirectoryExists(offerEntity.ImagesDirectoryId))
                {
                    _directoryManipulator.DeleteDirectoryWithContents(offerEntity.ImagesDirectoryId);
                }
                return Redirect("~/Error");
                throw;
            }

            await _auth.SignOutAsync(HttpContext);

            offerEntity.Active = true;
            offerEntity.LastChanged = DateTime.UtcNow;
            offerEntity.ImageCount = Photos.Count;

            _context.Update(offerEntity);
            await _context.SaveChangesAsync();

            return Redirect($"~/Offer/Offer/{offerEntity.Id}");
        }
    }
}
