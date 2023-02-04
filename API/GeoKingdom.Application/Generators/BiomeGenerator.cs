using GeoKingdom.Application.Stores;
using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Generators
{
	public class BiomeGenerator
	{
		private readonly IBiomeStore _biomeStore;
		private readonly Dictionary<BiomeCodeType, Biome> _biomeCache;

		public BiomeGenerator(
			IBiomeStore biomeStore)
		{
			_biomeStore = biomeStore;
			_biomeCache = new Dictionary<BiomeCodeType, Biome>();
		}

		public async Task<Biome> Generate()
		{
			var code = BiomeCodeType.Grasslands;

			if (_biomeCache.TryGetValue(code, out Biome biome))
				return biome;

			biome = await _biomeStore.GetBiome(code);
			_biomeCache.Add(code, biome);

			return biome;
		}

	}
}
