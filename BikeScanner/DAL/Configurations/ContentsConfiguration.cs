using BikeScanner.DAL.Constants;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.DAL.Configurations
{
    internal class ContentsConfiguration : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.HasIdColumn();
            builder.HasCrudColumns();
            builder.UseExcludeDeleteStateFilter();
            builder.HasAlternateKey(e => e.Url);
            builder.HasIndex(e => e.Published);
            builder
                .HasIndex(e => e.Text)
                .HasMethod("GIN")
                .IsTsVectorExpressionIndex(PostgreVectorLangs.Rus);
            builder.Property(e => e.Text).IsRequired();
            builder.Property(e => e.Url).IsRequired();
        }
    }
}

