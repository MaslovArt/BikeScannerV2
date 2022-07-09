using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.DAL.Configurations
{
    internal class DevMessagesConfiguration : IEntityTypeConfiguration<DevMessage>
    {
        public void Configure(EntityTypeBuilder<DevMessage> builder)
        {
            builder.HasIdColumn();
            builder.HasCrudColumns();
        }
    }
}

