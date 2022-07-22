using May25.API.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace May25.API.Data.EntitiesConfiguration
{
    public class MakeEntityConfiguration : IEntityTypeConfiguration<Make>
    {
        public void Configure(EntityTypeBuilder<Make> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(15);

            builder
                .HasMany(x => x.Models)
                .WithOne(x => x.Make)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
