using System;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.Core.Exceptions;
using BikeScanner.DAL;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BikeScanner.App.Services
{
    public class UsersService : AsyncCrudService<User, UserCreateModel, UserCreateModel>
    {
        private readonly IMemoryCache _cache;

        public UsersService(BikeScannerContext ctx, IMemoryCache cache)
            : base(ctx)
        {
            _cache = cache;
        }

        public override async Task ValidateBeforeInsert(UserCreateModel model)
        {
            var isExists = await ctx.Users.AnyAsync(u => u.UserId == model.UserId);
            if (isExists)
                throw ApiException.Confict($"Пользователь {model.UserId} уже существует.");
        }

        public async Task<User> EnsureUser(UserCreateModel model)
        {
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == model.UserId);
            if (user == null)
                user = await CreateAsync(model);

            return user;
        }

        public async Task ActivateUser(long userId)
        {
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == userId)
                ?? throw ApiException.NotFound($"Пользователь не найден.");

            user.SetState(UserStates.Banned);
            user.MarkUpdated();
            await ctx.SaveChangesAsync();
        }

        public async Task DiactivateUser(long userId)
        {
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == userId)
                ?? throw ApiException.NotFound($"Пользователь не найден.");

            user.SetState(UserStates.Banned);
            user.MarkUpdated();
            await ctx.SaveChangesAsync();
        }

        public async Task<bool> IsBanned(long userId)
        {
            var blackListUsersKey = "black_list";
            var blackListUsers = _cache.Get<long[]>(blackListUsersKey);
            if (blackListUsers == null)
            {
                blackListUsers = await ctx.Users
                    .WithState(UserStates.Banned)
                    .Select(u => u.UserId)
                    .ToArrayAsync();
                _cache.Set(
                    blackListUsersKey,
                    blackListUsers,
                    new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) }
                    );
            }

            return blackListUsers.Any(u => u == userId);
        }
    }
}

