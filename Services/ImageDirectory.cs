using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Services.Interfaces;
using SixLabors.ImageSharp;

namespace NajdiSpolubydliciRazor.Services
{
    public class ImageDirectory : IImageDirectory
    {
        private const string imagesRoot = "wwwroot/users_images";

        public string GetPath(Guid directoryId)
        {
            return $"{imagesRoot}/{directoryId}";
        }

        public bool ImageDirectoryExists(Guid directoryId)
        {
            return Directory.Exists(GetPath(directoryId));
        }

        public void DeleteDirectoryWithContents(Guid directoryId)
        {
            Directory.Delete(GetPath(directoryId), true);
        }

        public void SafelyDeleteImageFile(Guid directoryId, string name)
        {
            if(File.Exists($"{GetPath(directoryId)}/{name}.webp"))
            {
                File.Delete($"{GetPath(directoryId)}/{name}.webp");
            }
        }

        public void MoveAndSortFiles(Guid directoryId, Guid newDirectoryId)
        {
            var newpath = GetPath(newDirectoryId);
            Directory.CreateDirectory(newpath);

            var path = GetPath(directoryId);
            string[] files = [.. Directory.GetFiles(path).Order()];

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith("thumbnail.webp"))
                {
                    File.Move($"{path}/thumbnail.webp", $"{newpath}/thumbnail.webp");
                }
                else File.Move($"{files[i]}", $"{newpath}/{i}.webp");
            }
        }
    }
}
