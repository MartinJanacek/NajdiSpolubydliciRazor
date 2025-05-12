using NajdiSpolubydliciRazor.Enums;
using NajdiSpolubydliciRazor.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Models
{
    public class DemandBaseModel : IDemand
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

        public DemandBaseModel() { }

        public DemandBaseModel(DemandBaseModel model)
        {
            Email = model.Email;
            PhoneNumber = model.PhoneNumber;
            Text = model.Text;
            District = model.District;
            Location = model.Location;
        }
    }
}
