using NajdiSpolubydliciRazor.Enums;
using SixLabors.ImageSharp;

namespace NajdiSpolubydliciRazor.Services.Interfaces
{
    public interface IImageManipulator
    {
        public bool HasPermitedImageFileSiqnature(Stream data, string? extension);

        public void Resize(Image image, ImageFunctionOnWeb imageFunction);

        public Task SaveImageAsync(Image image, string path, string name);
    }
}
