using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utility
{
	public abstract class Singleton<T> : MonoBehaviour
		where T : MonoBehaviour
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				return _instance;
			}
		}

		protected abstract T This { get; }

		void Awake()
		{
			if (_instance != null)
				return;

			_instance = This;
		}
	}
}
