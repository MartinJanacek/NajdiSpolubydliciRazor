namespace NajdiSpolubydliciRazor.Entities.Interfaces
{
    public interface IAdvertismentEntity
    {
        public Guid Id { get; set; }

        public string Hash { get; set; }

        public byte[] Salt { get; set; }

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastChanged { get; set; }
    }
}
