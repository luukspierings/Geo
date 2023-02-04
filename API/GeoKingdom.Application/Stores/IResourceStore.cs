using GeoKingdom.Models;
using GeoKingdom.Models.Map;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Stores
{
	public interface IResourceStore
	{
		Task<Resource> GetResource(ResourceCodeType code);

	}
}
