using UnityEngine;

public class ButtonRegistration : MonoBehaviour
{


	public void OnBuild()
	{
		Debug.Log("build clicked");

		EventManager.Instance.ActivateEvent(Assets.Scripts.Configuration.EventType.Build);
	}

}
