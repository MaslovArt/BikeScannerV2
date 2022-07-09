using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.DAL.Configurations
{
    internal class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIdColumn();
            builder.HasCrudColumns();
            builder.HasAlternateKey(e => e.UserId);
            builder.HasIndex(e => e.State);
        }
    }
}

