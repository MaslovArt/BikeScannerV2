using System;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.DAL.Configurations
{
	public class JobsExecInfoConfiguration : IEntityTypeConfiguration<JobExecInfo>
    {
        public void Configure(EntityTypeBuilder<JobExecInfo> builder)
        {
            builder.HasDctColumns();
        }
    }
}

