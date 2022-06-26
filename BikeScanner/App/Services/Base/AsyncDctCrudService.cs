using System;
using BikeScanner.DAL;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.App.Services
{
	public class AsyncDctCrudService<T> where T : DctBase, new()
    {
        private readonly BikeScannerContext _ctx;

        public AsyncDctCrudService(BikeScannerContext ctx)
        {
            _ctx = ctx;
        }

        public Task<string> GetDctValue(string code)
        {
            return _ctx
                .Set<T>()
                .WithCode(code)
                .Select(d => d.Value)
                .FirstOrDefaultAsync();
        }

        public Task<T> GetDct(string code)
        {
            return _ctx
                .Set<T>()
                .GetByCodeAsync(code);
        }

        public async Task<T> AddOrUpdateDctValue(string code, string value)
        {
            var dct = await _ctx
                .Set<T>()
                .GetByCodeAsync(code);

            if (dct == null)
            {
                dct = new T()
                {
                    Code = code,
                    Value = value
                };
                _ctx.Set<T>().Add(dct);
                await _ctx.SaveChangesAsync();
            }

            return dct;
        }
    }
}

