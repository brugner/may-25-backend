using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class NotificationEntityConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(x => x.GeneratorUserId).IsRequired();
            builder.Property(x => x.TargetUserId).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Body).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Read).IsRequired();
            builder.Property(x => x.FirebaseResponse).IsRequired(false);
            builder.Property(x => x.CreatedAt).IsRequired();

            builder
                .HasOne(x => x.GeneratorUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.TargetUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
