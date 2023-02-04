using System;

namespace GeoKingdom.Models.Primary
{
	public class Position: IEquatable<(int, int)>
	{
		public Position(){}
		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; set; }
		public int Y { get; set; }



		// Equality methods

		public bool Equals((int, int) other)
			=> X == other.Item1 && Y == other.Item2;

        public static bool operator ==(Position obj1, (int, int) obj2)
			=> obj1.Equals(obj2);

		public static bool operator !=(Position obj1, (int, int) obj2)
            => !(obj1 == obj2);

		public override bool Equals(object obj)
			=> Equals(obj as Position);

		public override int GetHashCode()
			=> HashCode.Combine(X, Y);
		
	}
}
