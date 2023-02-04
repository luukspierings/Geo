using Assets.Scripts.Configuration;
using Assets.Scripts.Utility;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{
	protected override EventManager This => this;

	private List<EventType> _eventActivationRegistration;
	private Queue<EventType> _eventQueue;

	private void Start()
	{
		_eventActivationRegistration = new List<EventType>();
		_eventQueue = new Queue<EventType>();
	}

	public void ActivateEvent(EventType eventName)
	{
		_eventQueue.Enqueue(eventName);
	}

	public bool EventIsActive(EventType eventName)
	{
		return _eventActivationRegistration.Contains(eventName);
	}

	public IEnumerable<EventType> GetActiveEvents()
	{
		return _eventActivationRegistration;
	}

	public void StartNewCycle()
	{
		_eventActivationRegistration.Clear();

		while (_eventQueue.Count != 0)
		{
			var eventName = _eventQueue.Dequeue();
			_eventActivationRegistration.Add(eventName);
		}
	}

}


