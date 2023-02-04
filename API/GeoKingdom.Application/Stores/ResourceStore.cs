using GeoKingdom.Application.DataAccess;
using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Stores
{
	public class ResourceStore : IResourceStore
	{
		private readonly ApplicationDbContext _applicationDbContext;

		public ResourceStore(
			ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public Task<Resource> GetResource(ResourceCodeType code)
		{
			return _applicationDbContext.Resources
				.FirstOrDefaultAsync(r => r.Code == code);
		}


	}
}
