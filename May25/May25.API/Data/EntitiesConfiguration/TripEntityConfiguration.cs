using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class TripEntityConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.Property(x => x.DriverId).IsRequired();
            builder.Property(x => x.CarId).IsRequired();
            builder.Property(x => x.OriginId).IsRequired();
            builder.Property(x => x.DestinationId).IsRequired();
            builder.Property(x => x.Departure).IsRequired();
            builder.Property(x => x.MaxPassengers).IsRequired().HasDefaultValueSql("1");
            builder.Property(x => x.Description).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Distance).IsRequired(false);
            builder.Property(x => x.Duration).IsRequired(false);
            builder.Property(x => x.SuggestedCost).IsRequired();
            builder.Property(x => x.Cost).IsRequired();
            builder.Property(x => x.CostPerPassenger).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.IsCanceled).IsRequired();
            builder.Property(x => x.CanceledAt).IsRequired(false);

            builder
                .HasOne(x => x.Driver)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Car)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Origin)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Destination)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
