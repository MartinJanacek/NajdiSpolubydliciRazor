using Microsoft.Extensions.Diagnostics.HealthChecks;
using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace NajdiSpolubydliciRazor.Services
{
    public class ImageManipulator : IImageManipulator
    {
        private static readonly Dictionary<string, List<byte[]>> _permitedFileSignatures = new Dictionary<string, List<byte[]>>
        {
            { "image/png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { "image/jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8}
                }
            },
            { "image/jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8}
                }
            }
        };

        public bool HasPermitedImageFileSiqnature(Stream data, string? extension)
        {
            if (string.IsNullOrEmpty(extension)
                || data is null
                || data.Length == 0
                || !_permitedFileSignatures.ContainsKey(extension))
            {
                return false;
            }

            using var reader = new BinaryReader(data);
            var signatures = _permitedFileSignatures[extension];
            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

            return signatures.Any(signature =>
                headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
        
        public void Resize(Image image, ImageFunctionOnWeb imageFunction)
        {
            int maxPixelCount = imageFunction switch
            {
                ImageFunctionOnWeb.Carousel => 1080,
                ImageFunctionOnWeb.Thumbnail => 240,
                _ => throw new NotImplementedException(),
            };

            if (image.Width >= image.Height) image.Mutate(x => x.Resize(maxPixelCount, 0));
            if (image.Height >= image.Width) image.Mutate(x => x.Resize(0, maxPixelCount));
        }

        public async Task SaveImageAsync(Image image, string path, string name)
        {
            await image.SaveAsWebpAsync($"{path}/{name}.webp");
        }
    }
}
