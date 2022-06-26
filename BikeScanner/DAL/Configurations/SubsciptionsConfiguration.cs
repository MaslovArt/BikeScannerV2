using System;
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
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => new { e.UserId, e.SearchQuery }).IsUnique();
            builder.Property(e => e.SearchQuery).IsRequired();
        }
    }
}

