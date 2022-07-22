using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class TripPassengerEntityConfiguration : IEntityTypeConfiguration<TripPassenger>
    {
        public void Configure(EntityTypeBuilder<TripPassenger> builder)
        {
            builder.ToTable("TripPassengers");

            builder.Property(x => x.PassengerId).IsRequired();
            builder.Property(x => x.TripId).IsRequired();

            builder
                .HasOne(x => x.Trip)
                .WithMany(x => x.Passengers)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Passenger)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
