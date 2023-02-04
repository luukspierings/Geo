using GeoKingdom.Application.DataAccess;
using GeoKingdom.Models.Map;
using GeoKingdom.Models.Primary;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Stores
{
	public class ChunkStore
	{
		private readonly ApplicationDbContext _applicationDbContext;

		public ChunkStore(
			ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public Task<Chunk> GetChunk(Position position)
		{
			return _applicationDbContext.Chunks
				.Include(c => c.Tiles)
				.ThenInclude(t => t.Resource)
				.FirstOrDefaultAsync(c => c.X == position.X && c.Y == position.Y);
		}

		public async Task StoreChunk(Chunk chunk)
		{
			await _applicationDbContext.Chunks.AddAsync(chunk);
			await _applicationDbContext.SaveChangesAsync();
		}
	}
}
