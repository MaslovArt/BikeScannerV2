using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.DAL.Configurations
{
    public class JobsExecInfoConfiguration : IEntityTypeConfiguration<JobExecutionInfo>
    {
        public void Configure(EntityTypeBuilder<JobExecutionInfo> builder)
        {
            builder.HasIdColumn();
            builder.HasAlternateKey(x => x.Code);
        }
    }
}

