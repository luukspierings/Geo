using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
	[Serializable]
	public class Map
	{
		public Position CenterPosition { get; set; }
		public IEnumerable<Tile> Tiles { get; set; }
	}
}
