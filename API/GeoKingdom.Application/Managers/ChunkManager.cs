using GeoKingdom.Application.Calculators;
using GeoKingdom.Application.Generators;
using GeoKingdom.Application.Stores;
using GeoKingdom.Models.Map;
using GeoKingdom.Models.Primary;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Managers
{
	public class ChunkManager
	{
		private readonly ChunkStore _chunkStore;
		private readonly ChunkGenerator _chunkGenerator;

		public ChunkManager(
			ChunkStore chunkStore,
			ChunkGenerator chunkGenerator)
		{
			_chunkStore = chunkStore;
			_chunkGenerator = chunkGenerator;
		}


		public async Task<Chunk> GetChunk(Position position)
		{
			var chunk = await _chunkStore.GetChunk(position);
			if (chunk == null)
			{
				chunk = await _chunkGenerator.GenerateChunk(position);
				await _chunkStore.StoreChunk(chunk);
			}

			return chunk;
		}



	}
}
