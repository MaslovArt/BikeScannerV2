using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.Core.Exceptions;
using BikeScanner.Core.Extensions;
using BikeScanner.DAL;
using BikeScanner.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.App.Services
{
    public class SubscriptionsService : AsyncCrudService<Subscription, SubscriptionCreateModel, SubscriptionCreateModel>
    {
        private int _maximumSubsPerUser = 10;

        public SubscriptionsService(BikeScannerContext ctx)
            : base(ctx)
        { }

        public override async Task ValidateBeforeInsert(SubscriptionCreateModel model)
        {
            if (!model.SearchQuery.IsMinLength(2))
                throw ApiException.Error("Требуется минимум 2 символа для поиска.");

            var duplicate = await ctx.Subscriptions
                .AnyAsync(s => s.UserId == model.UserId &&
                               s.SearchQuery == model.SearchQuery);
            if (duplicate)
                throw ApiException.Confict($"Поиск '{model.SearchQuery}' уже сохранен.");

            var subsCount = await ctx.Subscriptions
                .CountAsync(s => s.UserId == model.UserId);
            if (subsCount >= _maximumSubsPerUser)
                throw ApiException.Error($"Нельзя сохранить больше {_maximumSubsPerUser} поисков.");
        }

        public async Task<TModel[]> GetUserSubs<TModel>(long userId)
        {
            var subs = await GetPageAsync<TModel>(1, 100, s => s.UserId == userId);
            return subs.Items;
        }
            
    }
}

