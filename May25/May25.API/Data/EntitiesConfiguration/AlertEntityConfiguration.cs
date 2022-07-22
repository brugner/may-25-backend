using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class AlertEntityConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.OriginFormattedAddress).IsRequired();
            builder.Property(x => x.OriginLat).IsRequired();
            builder.Property(x => x.OriginLng).IsRequired();
            builder.Property(x => x.DestinationFormattedAddress).IsRequired();
            builder.Property(x => x.DestinationLat).IsRequired();
            builder.Property(x => x.DestinationLng).IsRequired();
            builder.Property(x => x.ValidUntil).IsRequired();

            builder
              .HasOne(x => x.User)
              .WithMany()
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
