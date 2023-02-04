using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Extensions
{
	public static class IEnumerableExtensions
	{

		public static async Task<IEnumerable<T>> ToArrayAsync<T>(this IAsyncEnumerable<T> enumerable)
		{
			var result = new List<T>();
			await foreach (var item in enumerable)
			{
				result.Add(item);
			}
			return result;
		}

		public static Task<IEnumerable<T>> ToArrayAsync<T>(this IEnumerable<Task<T>> enumerable)
			=> enumerable.ToAsyncEnumerable().ToArrayAsync();


		public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<Task<T>> enumerable)
		{
			foreach (var item in enumerable)
				yield return await item;
		}

	}
}
