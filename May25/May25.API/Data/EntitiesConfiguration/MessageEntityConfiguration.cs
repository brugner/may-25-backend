using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(x => x.TripId).IsRequired();
            builder.Property(x => x.FromUserId).IsRequired();
            builder.Property(x => x.ToUserId).IsRequired();
            builder.Property(x => x.Text).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CreatedAt).IsRequired();

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
