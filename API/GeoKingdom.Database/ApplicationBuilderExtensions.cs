using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace GeoKingdom.Database
{
	public static class ApplicationBuilderExtensions
	{
		public static void MigrateDbContext<T>(this IApplicationBuilder app)
			where T : DbContext
		{
			using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
			var dbContext = serviceScope.ServiceProvider.GetRequiredService<T>();
			var migrator = dbContext.GetInfrastructure().GetService<IMigrator>();
			if (migrator == null)
				return;

			migrator.Migrate();
		}
	}
}
