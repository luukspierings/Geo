using GeoKingdom.Models.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKingdom.Application.Helpers
{
	public class PositionHelper
	{


		public static IEnumerable<Position> GetPositionsInArea(int centerX, int centerY, int radius)
		{
			var minX = centerX - radius;
			var maxX = centerX + radius;
			var minY = centerY - radius;
			var maxY = centerY + radius;

			return GetPositionsInArea(minX, maxX, minY, maxY);
		}

		public static IEnumerable<Position> GetPositionsInArea(int minX, int maxX, int minY, int maxY)
		{
			for (int x = minX; x <= maxX; x++)
			{
				for (int y = minY; y <= maxY; y++)
				{
					yield return new Position()
					{
						X = x,
						Y = y
					};
				}
			}
		}



	}
}
