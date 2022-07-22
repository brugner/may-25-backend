using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class RatingEntityConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.Property(x => x.TripId).IsRequired();
            builder.Property(x => x.FromUserId).IsRequired();
            builder.Property(x => x.ToUserId).IsRequired();
            builder.Property(x => x.ToUserType).IsRequired();
            builder.Property(x => x.Stars).IsRequired();
            builder.Property(x => x.Comment).IsRequired();
            builder.Property(x => x.Reply).IsRequired(false);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.RepliedAt).IsRequired(false);

            builder
             .HasOne(x => x.Trip)
             .WithMany()
             .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasOne(x => x.FromUser)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);

            builder
              .HasOne(x => x.ToUser)
              .WithMany()
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
