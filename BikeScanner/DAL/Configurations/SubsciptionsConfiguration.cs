using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.DAL.Configurations
{
    internal class SubsciptionsConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasIdColumn();
            builder.HasCrudColumns();
            builder.UseExcludeDeleteStateFilter();
            builder.HasIndex(e => e.UserId);
            builder.Property(e => e.SearchQuery).IsRequired();
        }
    }
}

