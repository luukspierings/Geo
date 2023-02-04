namespace Assets.Scripts.Calculators
{
	public static class PositionHelper
	{

		public static bool IsWithinOrEqualTo(int pos, int minPos, int maxPos)
			=> pos >= minPos && pos <= maxPos;

		public static bool IsWithinOrEqualToRadius(int centerPos, int currentPos, int distance)
			=> currentPos >= centerPos - distance && currentPos <= centerPos + distance;

	}

}
