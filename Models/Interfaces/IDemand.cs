using NajdiSpolubydliciRazor.Enums;

namespace NajdiSpolubydliciRazor.Models.Interfaces
{
    public interface IDemand
    {
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string Text { get; set; }

        public District District { get; set; }

        public string? Location { get; set; }
    }
}
