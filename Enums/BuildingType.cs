using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Enums
{
    public enum BuildingType
    {
        [Display(Name = "Dům")]
        House = 0,

        [Display(Name = "Byt")]
        Apartment = 1,

        [Display(Name = "Jiné")]
        Else = 2,
    }
}
