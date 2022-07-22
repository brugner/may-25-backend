using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class PlaceEntityConfiguration : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.Property(x => x.GooglePlaceId).IsRequired();
            builder.Property(x => x.FormattedAddress).IsRequired(false);
            builder.Property(x => x.Lat).IsRequired(false);
            builder.Property(x => x.Lng).IsRequired(false);
            builder.Property(x => x.ValidUntil).IsRequired(false);
        }
    }
}