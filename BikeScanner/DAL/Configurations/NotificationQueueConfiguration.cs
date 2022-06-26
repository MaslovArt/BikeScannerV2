using System;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.DAL.Configurations
{
    internal class NotificationQueueConfiguration : IEntityTypeConfiguration<NotificationQueue>
    {
        public void Configure(EntityTypeBuilder<NotificationQueue> builder)
        {
            builder.HasIdColumn();
            builder.HasCrudColumns();
            builder.HasIndex(e => e.State);
            builder.Property(e => e.Text).IsRequired();
        }
    }
}

