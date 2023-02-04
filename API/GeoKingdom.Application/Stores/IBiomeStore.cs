using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Stores
{
	public interface IBiomeStore
	{
		Task<Biome> GetBiome(BiomeCodeType code);
	}
}
