using GeoKingdom.Application.DataAccess;
using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoKingdom.Database
{
	public static class DatabaseSeeder
	{

		public static void Seed(this IApplicationBuilder app)
		{
			using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
			var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			var resources = new[]
			{
				new Resource()
				{
					Name = "Trees",
					Code = ResourceCodeType.Trees,
					Type = ResourceType.Wood,
					Rate = TimeSpan.FromMinutes(1),
					Yield = 1,
					Collectable = false,
					Renewable = true
				},
			};
			var biomes = new[]
			{
				new Biome()
				{
					Name = "Grasslands",
					Code = BiomeCodeType.Grasslands,
					Type = BiomeType.Grass
				}
			};

			AddIfNotExists(applicationDbContext.Resources, resources, (e1, e2) => e1.Code == e2.Code);
			AddIfNotExists(applicationDbContext.Biomes, biomes, (e1, e2) => e1.Code == e2.Code);
			applicationDbContext.SaveChanges();
		}

		private static void AddIfNotExists<T>(
			DbSet<T> dbSet,
			IEnumerable<T> entities,
			Func<T, T, bool> predicate)
			where T : class, new()
		{
			foreach (var item in entities)
				AddIfNotExists(dbSet, item, predicate);
		}

		private static void AddIfNotExists<T>(
			DbSet<T> dbSet,
			T entity,
			Func<T, T, bool> predicate)
			where T : class, new()
		{
			var exists = dbSet.ToArray().Any(e => predicate(e, entity));
			if (exists)
				return;

			dbSet.Add(entity);
		}
	}
}
