using May25.API.Core.Models.Entities.Keyless;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class RecentlyCompletedTripEntityConfiguration : IEntityTypeConfiguration<RecentlyCompletedTrip>
    {
        public void Configure(EntityTypeBuilder<RecentlyCompletedTrip> builder)
        {
            builder.HasNoKey().ToView(null);
        }
    }
}
