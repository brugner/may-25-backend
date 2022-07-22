using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class NotificationTokenEntityConfiguration : IEntityTypeConfiguration<NotificationToken>
    {
        public void Configure(EntityTypeBuilder<NotificationToken> builder)
        {
            builder.ToTable("NotificationTokens");
            builder.Property(x => x.UserId).IsRequired();
            builder.HasIndex(x => x.UserId).IsUnique();
            builder.Property(x => x.Token).IsRequired();
        }
    }
}
