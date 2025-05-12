using NajdiSpolubydliciRazor.Enums;
using SixLabors.ImageSharp;

namespace NajdiSpolubydliciRazor.Services.Interfaces
{
    public interface IImageDirectory
    {
        public string GetPath(Guid DirectoryId);

        public bool ImageDirectoryExists(Guid DirectoryId);

        public void DeleteDirectoryWithContents(Guid DirectoryId);

        public void SafelyDeleteImageFile(Guid DirectoryId, string name);

        public void MoveAndSortFiles(Guid DirectoryId, Guid newDirectoryId);
    }
}
