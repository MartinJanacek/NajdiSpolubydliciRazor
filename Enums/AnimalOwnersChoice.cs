using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Enums
{
    public enum AnimalOwnersChoice
    {
        [Display(Name = "Ano")]
        Yes = 0,

        [Display(Name = "Ne")]
        No = 1,

        [Display(Name = "Někteří")]
        Some = 2
    }
}
