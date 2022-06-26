using System;
using BikeScanner.DAL;
using BikeScanner.Domain.Models;

namespace BikeScanner.App.Services
{
	public class JobExecInfoService : AsyncDctCrudService<JobExecInfo>
	{
        private readonly string _indexingStampKey = "LastCrawlingTime";
        private readonly string _autoSearchStampKey = "LastAutoSearchTime";

        public JobExecInfoService(BikeScannerContext ctx)
			: base(ctx)
		{ }

        public async Task<DateTime?> GetLastCrawlingTime()
        {
            var value = await GetDctValue(_indexingStampKey);
            return value == null
                ? null
                : DateTime.Parse(value);
        }

        public Task SetLastCrawlingTime(DateTime time) =>
            AddOrUpdateDctValue(_indexingStampKey, time.ToString());

        public async Task<DateTime?> GetLastAutoSearchTime()
        {
            var value = await GetDctValue(_autoSearchStampKey);
            return value == null
                ? null
                : DateTime.Parse(value);
        }

        public Task SetLastAutoSearchTime(DateTime time) =>
            AddOrUpdateDctValue(_autoSearchStampKey, time.ToString());
    }
}

