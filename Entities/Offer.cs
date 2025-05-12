using NajdiSpolubydliciRazor.Entities.Interfaces;
using NajdiSpolubydliciRazor.Helpers.Interfaces;
using NajdiSpolubydliciRazor.Models;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Entities
{
    public class Offer : OfferBaseModel, IAdvertismentEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ImagesDirectoryId { get; set; }

        public string Hash { get; set; } = null!;

        public byte[] Salt { get; set; } = null!;

        public bool Active { get; set; }

        public int ImageCount { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastChanged { get; set; }

        public Offer() : base() { } // for EF only

        public Offer(Guid sequentialGuid, string hash, byte[] salt, OfferBaseModel offerBaseModel) : base(offerBaseModel)
        {
            Id = sequentialGuid;

            ImagesDirectoryId = Guid.NewGuid();

            Hash = hash;

            Salt = salt;

            Active = false;

            ImageCount = 0;

            CreationDate = DateTime.UtcNow;

            LastChanged = DateTime.UtcNow;
        }
    }
}
