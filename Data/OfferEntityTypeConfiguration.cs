using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NajdiSpolubydliciRazor.Entities;

namespace NajdiSpolubydliciRazor.Data
{
    public class OfferEntityTypeConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.ImagesDirectoryId, "UX_ImagesDirectoryId").IsUnique();

            builder.HasIndex(x => x.Email, "UX_Email").IsUnique();

            builder.HasIndex(x => x.Active, "IX_Active");

            builder.HasIndex(x => x.CreationDate, "IX_CreationDate");

            builder.HasIndex(x => x.District, "IX_District");

            builder.HasIndex(x => x.RentalPrice, "IX_RentalPrice");

            builder.HasIndex(x => x.MoveInDate, "IX_MoveInDate");
        }
    }
}
