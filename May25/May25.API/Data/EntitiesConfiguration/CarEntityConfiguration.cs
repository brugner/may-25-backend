using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class CarEntityConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.Property(x => x.PlateNumber).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Color).IsRequired();
            builder.Property(x => x.Year).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();

            builder
              .HasOne(x => x.Make)
              .WithMany()
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Model)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
