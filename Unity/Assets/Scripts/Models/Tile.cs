using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
	[Serializable]
	public class Tile
	{
		public int Id { get; set; }
		public int BiomeId { get; set; }
		public int? ResourceId { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
	}
}
