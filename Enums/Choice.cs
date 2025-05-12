using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Enums
{
    public enum Choice
    {
        [Display(Name = "Ano")]
        Yes = 0,

        [Display(Name = "Ne")]
        No = 1,
    }
}
