using GeoKingdom.Application.Stores;
using GeoKingdom.Base.Configuration;
using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Generators
{
	public class ResourceGenerator
	{
		private readonly MapConfiguration _mapConfiguration;
		private readonly IResourceStore _resourceStore;
		private readonly Dictionary<ResourceCodeType, Resource> _resourcesCache;

		private readonly OpenSimplexNoise _openSimplexNoise;

		public ResourceGenerator(
			IOptionsSnapshot<MapConfiguration> mapConfiguration,
			IResourceStore resourceStore)
		{
			_mapConfiguration = mapConfiguration.Value;
			_openSimplexNoise = new OpenSimplexNoise(12345);
			_resourcesCache = new Dictionary<ResourceCodeType, Resource>();
			_resourceStore = resourceStore;
		}


		public async Task<Resource> Generate(int x, int y)
		{
			foreach (var distribution in _mapConfiguration.ResourceDistribution)
			{
				var zoom = distribution.Value.Zoom;
				var evaluation = _openSimplexNoise.Evaluate(x / zoom, y / zoom);

				var normalizedEvaluation = evaluation + 1d / 2d;
				if (normalizedEvaluation * 100 < distribution.Value.Percentage)
					return await ResolveResource(distribution.Key);
			}
			return null;
		}

		private async Task<Resource> ResolveResource(ResourceCodeType code)
		{
			if (_resourcesCache.TryGetValue(code, out Resource resource))
				return resource;

			resource = await _resourceStore.GetResource(code);
			_resourcesCache.Add(code, resource);

			return resource;
		}


	}
}
