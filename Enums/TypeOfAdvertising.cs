using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Enums
{
    public enum TypeOfAdvertising
    {
        [Display(Name = "Nabídka")]
        Offer = 0,

        [Display(Name = "Poptávka")]
        Demand = 1
    }
}
