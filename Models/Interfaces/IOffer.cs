using NajdiSpolubydliciRazor.Enums;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Models.Interfaces
{
    public interface IOffer
    {
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string Text { get; set; }

        public District District { get; set; }

        public string? Location { get; set; }

        public int? RentalPrice { get; set; } 

        public BuildingType Type { get; set; }

        public int NumberOfRooms { get; set; }

        public int TotalArea { get; set; }

        public int AreaForTenant { get; set; }

        public Choice Furnished { get; set; }

        public Choice Internet { get; set; }

        public Choice Wifi { get; set; }

        public Choice AreAnimalsInPlace { get; set; }

        public AnimalOwnersChoice AnimalOwners { get; set; }

        public int Floor { get; set; }

        public DateTime MoveInDate { get; set; }
    }
}
