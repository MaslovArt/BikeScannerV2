using System;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.DAL;
using BikeScanner.DAL.Constants;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.States;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.App.Services
{
    public class ContentService : AsyncCrudService<Content, ContentModel, ContentModel>
	{
		public ContentService(BikeScannerContext ctx)
            : base(ctx)
		{ }

        public async Task<Page<TModel>> Search<TModel>(
            string query,
            int skip = 0,
            int take = 10,
            DateTime? since = null)
        {
            var queryable = repository
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
            repository
                .Where(c => EF.Functions
                    .ToTsVector(PostgreVectorLangs.Eng, c.Text)
                    .Matches(query))
                .CountAsync();

        public Task<int> ArchiveContents(DateTime since) =>
            UpdateState(ContentStates.Archive, c => c.State == ContentStates.Active.ToString() &&
                                                    c.Published < since);
    }
}

