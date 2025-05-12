using NajdiSpolubydliciRazor.Enums;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Entities
{
    public class ContactReceiver
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public TypeOfAdvertising TypeOfAdvertising { get; set; }

        public Guid AdvertismentId { get; set; }

        // Hash Salt not needed
        public string Code { get; set; }

        public DateTime CodeCreated { get; set; }

        public bool UrlOpened { get; set; }

        public ContactReceiver(string email, TypeOfAdvertising typeOfAdvertising, Guid advertismentId, string code)
        {
            Email = email;
            TypeOfAdvertising = typeOfAdvertising;
            AdvertismentId = advertismentId;
            UrlOpened = false;
            Code = code;
            CodeCreated = DateTime.UtcNow;
        }
    }
}
