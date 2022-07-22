using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class SeatRequestEntityConfiguration : IEntityTypeConfiguration<SeatRequest>
    {
        public void Configure(EntityTypeBuilder<SeatRequest> builder)
        {
            builder.ToTable("SeatRequests");

            builder.Property(x => x.TripId).IsRequired();
            builder.Property(x => x.PassengerId).IsRequired();

            builder
               .HasOne(x => x.Trip)
               .WithMany(x => x.SeatRequests)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Passenger)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
