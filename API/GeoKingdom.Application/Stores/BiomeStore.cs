using GeoKingdom.Application.DataAccess;
using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Stores
{
	public class BiomeStore : IBiomeStore
	{
		private readonly ApplicationDbContext _applicationDbContext;

		public BiomeStore(
			ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public Task<Biome> GetBiome(BiomeCodeType code)
		{
			return _applicationDbContext.Biomes
				.FirstOrDefaultAsync(r => r.Code == code);
		}


	}
}
