using GeoKingdom.Models.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoKingdom.Models
{
	public class Resource
	{
		public int Id { get; set; }
		public ResourceCodeType Code { get; set; }

		public string Name { get; set; }
		public ResourceType Type { get; set; }

		public int Yield { get; set; }
		public TimeSpan? Rate { get; set; }
		public bool Renewable { get; set; }
		public bool Collectable { get; set; }
	}
}
