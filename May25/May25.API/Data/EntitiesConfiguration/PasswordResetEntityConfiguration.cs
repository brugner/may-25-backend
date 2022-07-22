using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class PasswordResetEntityConfiguration : IEntityTypeConfiguration<PasswordReset>
    {
        public void Configure(EntityTypeBuilder<PasswordReset> builder)
        {
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Token).IsRequired();
            builder.Property(x => x.ValidUntil).IsRequired();
        }
    }
}
