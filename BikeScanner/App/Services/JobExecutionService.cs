using System;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.DAL;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.App.Services
{
	public class JobExecutionService
	{
        private readonly DbSet<JobExecutionInfo>  _repository;
        private readonly BikeScannerContext       _ctx;
        private readonly string                   _indexingStampKey;
        private readonly string                   _autoSearchStampKey;

        public JobExecutionService(BikeScannerContext ctx)
		{
            _ctx = ctx;
            _repository = ctx.JobExecutionInfo;
            _indexingStampKey = "LastCrawlingTime";
            _autoSearchStampKey = "LastAutoSearchTime";
        }

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


        private Task<string> GetDctValue(string code) =>
            _repository
                .Where(d => d.Code == code)
                .Select(d => d.Value)
                .FirstOrDefaultAsync();

        public Task<JobExecutionInfo> GetDct(string code) =>
            _repository.FirstOrDefaultAsync(x => x.Code == code);

        private async Task<JobExecutionInfo> AddOrUpdateDctValue(string code, string value)
        {
            var dct = await GetDct(code);
            if (dct == null)
            {
                dct = new JobExecutionInfo()
                {
                    Code = code,
                };
                _repository.Add(dct);
            }

            dct.Value = value;
            await _ctx.SaveChangesAsync();

            return dct;
        }
    }
}

