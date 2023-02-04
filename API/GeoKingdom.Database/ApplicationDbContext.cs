using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using Microsoft.EntityFrameworkCore;

namespace GeoKingdom.Application.DataAccess
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Chunk> Chunks { get; set; }
		public DbSet<Tile> Tiles { get; set; }
		public DbSet<Resource> Resources { get; set; }
		public DbSet<Biome> Biomes { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
