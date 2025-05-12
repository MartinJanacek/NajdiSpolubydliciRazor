using NajdiSpolubydliciRazor.Entities.Interfaces;
using NajdiSpolubydliciRazor.Models;
using System.ComponentModel.DataAnnotations;

namespace NajdiSpolubydliciRazor.Entities
{
    public class Demand : DemandBaseModel, IAdvertismentEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Hash { get; set; } = null!;

        public byte[] Salt { get; set; } = null!;

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastChanged { get; set; }

        public Demand() { } // for EF only

        public Demand(Guid id, string hash, byte[] salt, DemandBaseModel demandBaseModel) : base(demandBaseModel)
        {
            Id = id;
            Hash = hash;
            Salt = salt;
            Active = false;
            CreationDate = DateTime.UtcNow;
            LastChanged = CreationDate;
        }
    }
}
