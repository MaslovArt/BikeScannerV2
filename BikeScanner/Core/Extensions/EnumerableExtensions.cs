using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Core.Extensions
{
    public static class EnumerableExtensions
	{
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable) =>
            enumerable != null && enumerable.Any();

        public static Task<T[]> WhenAllAsync<T>(this IEnumerable<Task<T>> tasks) =>
            Task.WhenAll(tasks);

        public static async Task WhenAllSequentialAsync(this IEnumerable<Task> tasks)
        {
            foreach (var task in tasks)
                await task;
        }

    }
}

