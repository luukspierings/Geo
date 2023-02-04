using GeoKingdom.Application.Generators;
using GeoKingdom.Application.Stores;
using GeoKingdom.Base.Configuration;
using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using GeoKingdom.Models.Primary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GeoKingdom.Tests
{
	public class ChunkGeneratorTests
	{
		private readonly ITestOutputHelper output;

		public ChunkGeneratorTests(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Fact]
		public async Task Generate()
		{
			var options = Substitute.For<IOptionsSnapshot<MapConfiguration>>();
			var config = GetApplicationConfiguration();
			config.TilesPerChunkDimension = 80;
			options.Value.Returns(config);

			var resourceStore = Substitute.For<IResourceStore>();
			resourceStore.GetResource(Arg.Any<ResourceCodeType>()).Returns(Task.FromResult(new Resource() { }));

			var biomeStore = Substitute.For<IBiomeStore>();

			var resourceGenerator = new ResourceGenerator(options, resourceStore);
			var biomeGenerator = new BiomeGenerator(biomeStore);
			var chunkGenerator = new ChunkGenerator(options, resourceGenerator, biomeGenerator);

			var chunk = await chunkGenerator.GenerateChunk(new Position(300, 300));

			var tiles = chunk.Tiles
				.OrderBy(t => t.Y)
				.ThenBy(t => t.X)
				.GroupBy(t => t.Y);

			foreach (var tile in tiles)
			{
				var symbols = tile.Select(t => t.Resource == null ? "░" : "▓");
				var line = string.Join("", symbols);
				output.WriteLine(line);
			}
		}

		public static IConfigurationRoot GetIConfigurationRoot()
		{
			return new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false)
				.AddEnvironmentVariables()
				.Build();
		}

		public static MapConfiguration GetApplicationConfiguration()
		{
			var configuration = new MapConfiguration();

			var iConfig = GetIConfigurationRoot();

			iConfig
				.GetSection("Map")
				.Bind(configuration);

			return configuration;
		}

	}
}
