using System;
using System.Linq.Expressions;
using BikeScanner.Core.Exceptions;
using BikeScanner.DAL;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models.Base;
using BikeScanner.Domain.States;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace BikeScanner.App.Services
{
	public abstract class AsyncCrudService<T> where T : StatefulCrudBase
	{
        protected readonly BikeScannerContext ctx;
        protected readonly DbSet<T> repository;

        public AsyncCrudService(BikeScannerContext ctx)
		{
            this.ctx = ctx;
            repository = ctx.Set<T>();
		}

        public virtual Task ValidateBeforeInsert<TModel>(TModel model) =>
            Task.CompletedTask;

        public virtual Task ValidateBeforeUpdate<TModel>(T entity, TModel model) =>
            Task.CompletedTask;

        public Task<TModel> GetRecordAsync<TModel>(int id) =>
            repository
                .WithId(id)
                .ProjectToType<TModel>()
                .FirstOrDefaultAsync();

        public Task<TModel[]> GetPageAsync<TModel>(int page, int limit, Expression<Func<T, bool>> filter) =>
            repository
                .WhereIf(filter, filter != null)
                .OrderByDescending(x => x.Id)
                .Page(page, limit)
                .ProjectToType<TModel>()
                .ToArrayAsync();

        public async Task<T> CreateAsync<TModel>(TModel insertModel)
        {
            await ValidateBeforeInsert(insertModel);

            var record = insertModel.Adapt<T>();
            repository.Add(record);
            return await ctx.SaveChangesAsync() > 0
                ? record
                : throw ApiException.ServerError("Ошибка при вставке записи");
        }

        public async Task<T> UpdateAsync<TModel>(int id, TModel updateModel)
        {
            var record = await repository.GetByIdAsync(id)
                ?? throw ApiException.NotFound();
            return await UpdateRecordAsync(record, updateModel);
        }


        private async Task<T> UpdateRecordAsync<TModel>(T record, TModel updateModel)
        {
            await ValidateBeforeUpdate(record, updateModel);

            updateModel.Adapt(record);

            await ctx.SaveChangesAsync();
            return record;
        }

        public async Task DeleteLogicalAsync(int id, string comments)
        {
            var result = await repository
                .WithId(id)
                .UpdateFromQueryAsync(new Dictionary<string, object>
                {
                    [nameof(StatefulCrudBase.State)] = BaseStates.Deleted
                });
            if (result <= 0)
                throw ApiException.NotFound();
        }

        public async Task DeletePhysicalAsync(int id)
        {
            var result = await repository
                .WithId(id)
                .DeleteAsync();
            if (result <= 0)
                throw ApiException.NotFound();
        }


    }
}

