using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.PasswordHash).IsRequired(false);
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.Gender).HasMaxLength(1);
            builder.Property(x => x.Bio).HasMaxLength(300);
            builder.Property(x => x.Talk).HasDefaultValueSql("5");
            builder.Property(x => x.Music).HasDefaultValueSql("5");
            builder.Property(x => x.Pets).HasDefaultValueSql("5");
            builder.Property(x => x.Smoking).HasDefaultValueSql("5");
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.EmailConfirmationToken).IsRequired(false);
            builder.Property(x => x.IsEmailConfirmed).IsRequired();
            builder.Property(x => x.IsSubscribed).IsRequired();
        }
    }
}
