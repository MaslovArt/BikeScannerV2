using System;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.DAL;
using BikeScanner.DAL.Constants;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.App.Services
{
	public class SearchService
	{
        private readonly DbSet<Content> _repository;

		public SearchService(BikeScannerContext ctx)
		{
            _repository = ctx.Contents;
		}

        public async Task<Page<TModel>> Search<TModel>(
            string query,
            int skip = 0,
            int take = 10,
            DateTime? since = null)
        {
            var queryable = _repository
                .AsNoTracking()
                .WhereIf(c => c.CreateDate >= since.Value, since.HasValue)
                .Where(c => EF.Functions.ToTsVector(PostgreVectorLangs.Eng, c.Text).Matches(query))
                .OrderByDescending(c => c.Published);

            var entities = await queryable
                .Skip(skip)
                .Take(take)
                .ProjectToType<TModel>()
                .ToArrayAsync();

            var total = entities.Length == take
                ? await queryable.CountAsync()
                : skip + entities.Length;

            return new Page<TModel>()
            {
                Items = entities,
                Total = total,
                Offset = skip + entities.Length
            };
        }

        public Task<int> CountSearch(string query) =>
            _repository
                .Where(c => EF.Functions.ToTsVector(PostgreVectorLangs.Eng, c.Text).Matches(query))
                .CountAsync();
    }
}

