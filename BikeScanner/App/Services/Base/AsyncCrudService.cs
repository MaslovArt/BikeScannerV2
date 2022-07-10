using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BikeScanner.Core.Exceptions;
using BikeScanner.DAL;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.Models.Base;
using BikeScanner.Domain.States;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace BikeScanner.App.Services
{
	public abstract class AsyncCrudService<TEntity, TCreateModel, TUpdateModel> where TEntity : StatefulCrudBase
	{
        protected readonly BikeScannerContext ctx;
        protected readonly DbSet<TEntity> repository;

        public AsyncCrudService(BikeScannerContext ctx)
		{
            this.ctx = ctx;
            repository = ctx.Set<TEntity>();
		}

        public virtual Task ValidateBeforeInsert(TCreateModel model) =>
            Task.CompletedTask;

        public virtual Task ValidateBeforeUpdate(TEntity entity, TUpdateModel model) =>
            Task.CompletedTask;

        public Task<TModel> GetRecordAsync<TModel>(int id) =>
            repository
                .WithId(id)
                .ProjectToType<TModel>()
                .FirstOrDefaultAsync();

        public Task<TModel[]> GetAll<TModel>() =>
            repository
                .ProjectToType<TModel>()
                .ToArrayAsync();

        public async Task<Page<TModel>> GetPageAsync<TModel>(
            int page,
            int limit,
            Expression<Func<TEntity, bool>> filter
            )
        {
            var pageItems = await repository
                .WhereIf(filter, filter != null)
                .OrderByDescending(x => x.Id)
                .Page(page, limit)
                .ProjectToType<TModel>()
                .ToArrayAsync();
            var total = pageItems.Length < limit
                ? pageItems.Length
                : await repository.WhereIf(filter, filter != null).CountAsync();

            return new Page<TModel>()
            {
                Items = pageItems,
                Total = total
            };
        }

        public async Task<TEntity> CreateAsync(
            TCreateModel insertModel,
            string initialState = null
            )
        {
            await ValidateBeforeInsert(insertModel);

            var record = insertModel.Adapt<TEntity>();
            record.State = initialState ?? BaseStates.Active.ToString();
            record.MarkCreated();
            repository.Add(record);

            return await ctx.SaveChangesAsync() > 0
                ? record
                : throw ApiException.ServerError("Ошибка при вставке записи");
        }

        public async Task<TEntity[]> CreateManyAsync(
            IEnumerable<TCreateModel> insertModels,
            string initialState = null
            )
        {
            if (insertModels.Count() == 0)
                return Array.Empty<TEntity>();

            foreach (var insertModel in insertModels)
                await ValidateBeforeInsert(insertModel);

            var records = insertModels.Adapt<List<TEntity>>();
            records.ForEach(r =>
            {
                r.State = initialState ?? BaseStates.Active.ToString();
                r.MarkCreated();
            });
            repository.AddRange(records);

            return await ctx.SaveChangesAsync() > 0
                ? records.ToArray()
                : throw ApiException.ServerError("Ошибка при вставке записи");
        }

        public async Task<TEntity> UpdateAsync(
            int id,
            TUpdateModel updateModel,
            string afterUpdateState = null
            )
        {
            var record = await repository.GetByIdAsync(id)
                ?? throw ApiException.NotFound();
            record.State = afterUpdateState ?? record.State;
            record.MarkUpdated();

            return await UpdateRecordAsync(record, updateModel);
        }


        public async Task<TEntity> UpdateRecordAsync(TEntity record, TUpdateModel updateModel)
        {
            await ValidateBeforeUpdate(record, updateModel);

            updateModel.Adapt(record);

            await ctx.SaveChangesAsync();
            return record;
        }

        public async Task DeleteLogicalAsync(int id)
        {
            var result = await repository
                .WithId(id)
                .UpdateFromQueryAsync(new Dictionary<string, object>
                {
                    [nameof(StatefulCrudBase.State)] = BaseStates.Deleted.ToString()
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

