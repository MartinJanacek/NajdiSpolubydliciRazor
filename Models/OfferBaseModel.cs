using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Models
{
    public class OfferBaseModel : IOffer
    {
        [Required(ErrorMessage = "Toto pole je povinné")]
        [EmailAddress(ErrorMessage = "Zadejte email ve správném formátu, př.: info@najdispolubydlici.cz")]
        public string Email { get; set; } = null!;

        [RegularExpression("^\\+[1-9]{1}[0-9]{3,14}$|^$", ErrorMessage = "Číslo zadejte ve vzoru +420 123 123 123 nebo nechejte prázdné")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné")]
        [MaxLength(2000, ErrorMessage = "Maximální počet znaků: 2000")]
        public string Text { get; set; } = null!;

        [Required(ErrorMessage = "Toto pole je povinné")]
        public District District { get; set; }

        [MaxLength(100, ErrorMessage = "Maximální počet znaků: 100")]
        public string? Location { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Toto pole je povinné")]
        public int? RentalPrice { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné")]
        public BuildingType Type { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné"), Range(1, 30, ErrorMessage = "Neplatná hodnota")]
        public int NumberOfRooms { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné"), Range(1, 500, ErrorMessage = "Neplatná hodnota")]
        public int TotalArea { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné"), Range(1, 500, ErrorMessage = "Neplatná hodnota")]
        public int AreaForTenant { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné")]
        public Choice Furnished { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné")]
        public Choice Internet { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné")]
        public Choice Wifi { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné")]
        public Choice AreAnimalsInPlace { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné")]
        public AnimalOwnersChoice AnimalOwners { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné"), Range(-100, 500)]
        public int Floor { get; set; }

        [Required(ErrorMessage = "Toto pole je povinné")]
        public DateTime MoveInDate { get; set; }

        public OfferBaseModel() { }

        public OfferBaseModel(OfferBaseModel model)
        {
            Email = model.Email;
            PhoneNumber = model.PhoneNumber;
            Text = model.Text;
            District = model.District;
            Location = model.Location;
            RentalPrice = model.RentalPrice;
            Type = model.Type;
            NumberOfRooms = model.NumberOfRooms;    
            TotalArea = model.TotalArea;
            AreaForTenant = model.AreaForTenant;
            Furnished = model.Furnished;    
            Internet = model.Internet;
            Wifi = model.Wifi;
            AreAnimalsInPlace = model.AreAnimalsInPlace;
            AnimalOwners = model.AnimalOwners;
            Floor = model.Floor;
            MoveInDate = model.MoveInDate;
        }
    }
}
