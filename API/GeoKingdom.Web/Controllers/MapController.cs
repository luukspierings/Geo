using GeoKingdom.Application.Managers;
using GeoKingdom.Base.Configuration;
using GeoKingdom.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace GeoKingdom.Web.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MapController : ControllerBase
	{
		private readonly MapManager _mapManager;
		private readonly MapConfiguration _mapConfiguration;

		public MapController(
			MapManager mapManager,
			IOptionsSnapshot<MapConfiguration> mapConfiguration)
		{
			_mapManager = mapManager;
			_mapConfiguration = mapConfiguration.Value;
		}

		[HttpGet]
		public async Task<IActionResult> Get(double lon, double lat)
		{
			var map = await _mapManager.GetMap(lon, lat);
			var viewModel = new MapViewModel()
			{
				CenterPosition = map.CenterPosition,
				Tiles = map.Tiles
					.Select(t => new TileViewModel()
					{
						Id = t.Id,
						X = t.X,
						Y = t.Y,
						BiomeId = t.BiomeId,
						ResourceId = t.ResourceId
					})
					.ToArray()
			};

			return Ok(viewModel);
		}

		[HttpGet("configuration")]
		public IActionResult Configuration()
		{
			return Ok(new MapConfigurationViewModel()
			{
				ChunkRenderDistance = _mapConfiguration.ChunkRenderDistance,
				MetersPerTile = _mapConfiguration.MetersPerTile,
				TilesPerChunkDimension = _mapConfiguration.TilesPerChunkDimension
			});
		}

	}
}
