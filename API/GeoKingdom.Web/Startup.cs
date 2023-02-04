using GeoKingdom.Application.Calculators;
using GeoKingdom.Application.DataAccess;
using GeoKingdom.Application.Generators;
using GeoKingdom.Application.Managers;
using GeoKingdom.Application.Stores;
using GeoKingdom.Base.Configuration;
using GeoKingdom.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GeoKingdom.Web
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public IWebHostEnvironment WebHostEnvironment { get; }

		public Startup(
			IConfiguration configuration,
			IWebHostEnvironment webHostEnvironment)
		{
			Configuration = configuration;
			WebHostEnvironment = webHostEnvironment;
		}


		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions();
			services.Configure<MapConfiguration>(Configuration.GetSection("Map"));
			services.Configure<DatabaseConfiguration>(Configuration.GetSection("Database"));

			var databaseConfiguration = Configuration.GetSection("Database").Get<DatabaseConfiguration>();
			if (databaseConfiguration.UseInMemory)
			{
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseInMemoryDatabase("Default")
				);
			}
			else
			{
				var dbConnectionString = Configuration.GetConnectionString("SqlServerConnection");
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(dbConnectionString)
				);
			}

			services
				.AddScoped<TilePositionCalculator>()
				.AddScoped<ChunkPositionCalculator>()

				.AddScoped<ChunkGenerator>()
				.AddScoped<BiomeGenerator>()
				.AddScoped<ResourceGenerator>()
				
				.AddScoped<MapManager>()
				.AddScoped<ChunkManager>()
				
				.AddScoped<ChunkStore>()
				.AddScoped<TileStore>()
				.AddScoped<IBiomeStore, BiomeStore>()
				.AddScoped<IResourceStore, ResourceStore>()
				;


			services.AddControllers();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.MigrateDbContext<ApplicationDbContext>();
			app.Seed();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
