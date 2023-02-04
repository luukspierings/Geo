using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Assets.Scripts.Calculators
{
	public static class ViewingCalculator
	{



		public static (quaternion, float3) CalculateLookat(
			float xRotation,
			float yRotation,
			float3 focusPosition,
			float radius)
		{
			var lookRotation = quaternion.Euler(new float3(math.radians(xRotation), math.radians(yRotation), 0));
			var lookDirection = math.mul(lookRotation, new float3(0, 0, 1));
			var lookPosition = focusPosition - (lookDirection * radius);

			return (lookRotation, lookPosition);
		}





	}
}
