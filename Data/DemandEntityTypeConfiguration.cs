using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NajdiSpolubydliciRazor.Entities;

namespace NajdiSpolubydliciRazor.Data
{
    public class DemandEntityTypeConfiguration : IEntityTypeConfiguration<Demand>
    {
        public void Configure(EntityTypeBuilder<Demand> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Active, "IX_Active");

            builder.HasIndex(x => x.CreationDate, "IX_CreationDate");

            builder.HasIndex(x => x.District, "IX_District");
        }
    }
}
