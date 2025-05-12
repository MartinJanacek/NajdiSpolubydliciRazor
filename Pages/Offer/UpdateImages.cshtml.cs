using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NajdiSpolubydliciRazor.Data;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Migrations;
using NajdiSpolubydliciRazor.Models;
using NajdiSpolubydliciRazor.Services.Interfaces;
using SixLabors.ImageSharp;
using System.Security.Claims;

namespace NajdiSpolubydliciRazor.Pages.Offer
{
    [BindProperties]
    public class UpdateimagesAndSaveChangesModel : PageModel
    {
        public Guid ImagesDirectory { get; set; }

        public int ImageCount { get; set; }

        public IFormFile? ProfilePhoto { get; set; }

        public List<IFormFile> Photos { get; set; }

        public List<int>? ChangedImages { get; set; }

        public List<int>? DeletedImages { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly IImageManipulator _imageManipulator;
        private readonly IImageDirectory _directoryManipulator;
        private readonly IAuth _auth;

        public UpdateimagesAndSaveChangesModel(ApplicationDbContext context, IImageManipulator imageResizer, IImageDirectory directoryManipulator, IAuth auth)
        {
            _context = context;
            _imageManipulator = imageResizer;
            _directoryManipulator = directoryManipulator;
            _auth = auth;
            Photos = [];
            ChangedImages = [];
            ImageCount = 0;
        }

        public async Task<IActionResult> OnGet(OfferBaseModel offerForm)
        {
            Photos = [];
            ChangedImages = [];

            var id = _auth.GetIdFromClaim(User.Identity as ClaimsIdentity);

            var offerEntity = await _context.FindAsync<Entities.Offer>(id);

            if (offerEntity is null)
            {
                return Redirect("~/NotFound");
            }

            ImagesDirectory = offerEntity.ImagesDirectoryId;
            ImageCount = offerEntity.ImageCount;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var id = _auth.GetIdFromClaim(User.Identity as ClaimsIdentity);

            var offerEntity = await _context.Offers.FindAsync(id);

            if (offerEntity is null)
            {
                return Redirect("~/NotFound");
            }

            if(ProfilePhoto is not null)
            {
                if (!_imageManipulator.HasPermitedImageFileSiqnature(ProfilePhoto.OpenReadStream(), ProfilePhoto.ContentType))
                {
                    ModelState.AddModelError(nameof(ProfilePhoto), $"{ProfilePhoto.FileName} - Tento soubor nepøíjmeme \t");
                }
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
                Photos = [];
                ChangedImages = [];
                ImagesDirectory = offerEntity.ImagesDirectoryId;
                ImageCount = offerEntity.ImageCount;
                DeletedImages = [];
                return Page();
            }

            var path = _directoryManipulator.GetPath(offerEntity.ImagesDirectoryId);

            if (ProfilePhoto is not null)
            {
                _directoryManipulator.SafelyDeleteImageFile(offerEntity.ImagesDirectoryId, "thumbnail");
                _directoryManipulator.SafelyDeleteImageFile(offerEntity.ImagesDirectoryId, "0");

                using Image thumbnail = await Image.LoadAsync(ProfilePhoto.OpenReadStream());
                {
                    _imageManipulator.Resize(thumbnail, ImageFunctionOnWeb.Carousel);
                    await _imageManipulator.SaveImageAsync(thumbnail, path, "0");

                    _imageManipulator.Resize(thumbnail, ImageFunctionOnWeb.Thumbnail);
                    await _imageManipulator.SaveImageAsync(thumbnail, path, "thumbnail");
                }
            }

            if(DeletedImages is not null)
            {
                for (int i = 0; i < DeletedImages.Count; i++)
                {
                    _directoryManipulator.SafelyDeleteImageFile(offerEntity.ImagesDirectoryId, DeletedImages[i].ToString());
                }
            }

            if(ChangedImages is not null)
            {
                for (int i = 0; i < ChangedImages.Count; i++)
                {
                    _directoryManipulator.SafelyDeleteImageFile(offerEntity.ImagesDirectoryId, ChangedImages[i].ToString());
                }
                
                for (int i = 0; i < Photos.Count; i++)
                {
                    using Image image = await Image.LoadAsync(Photos[i].OpenReadStream());
                    {
                        _imageManipulator.Resize(image, ImageFunctionOnWeb.Carousel);

                        if (i < ChangedImages.Count)
                        {
                            await _imageManipulator.SaveImageAsync(image, path, ChangedImages[i].ToString());
                        }
                        else
                        {
                            await _imageManipulator.SaveImageAsync(image, path, (offerEntity.ImageCount + 1).ToString());
                            if (i < Photos.Count -1)
                            {
                                offerEntity.ImageCount++;
                            }
                        }
                    }
                }
            }

            var newDirectoryId = Guid.NewGuid();
            _directoryManipulator.MoveAndSortFiles(offerEntity.ImagesDirectoryId, newDirectoryId);
            _directoryManipulator.DeleteDirectoryWithContents(offerEntity.ImagesDirectoryId);

            offerEntity.ImagesDirectoryId = newDirectoryId;
            await _context.SaveChangesAsync();

            return Redirect($"~/Offer/Offer/{offerEntity.Id}");
        }
    }
}
