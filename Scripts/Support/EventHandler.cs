using System.Collections.Generic;
using UnityEngine.Events;

public class EventHandler<N>
{
	#region Fields
	static List<IEventInterface<N>> Invokers = new List<IEventInterface<N>>();
	static List<UnityAction<N>> Listeners = new List<UnityAction<N>>();
	#endregion

	#region Properties
	public bool IsListenersZero
	{
		get
		{
			if (Listeners.Count == 0)
				return true;
			else
				return false;
		}
	}
	#endregion

	#region Methods
	public void AddInvoker(IEventInterface<N> invoker)
	{
		Invokers.Add(invoker);
		foreach (UnityAction<N> listener in Listeners)
		{
			invoker.AddListener(listener);
		}
	}

	public void AddListener(UnityAction<N> action)
	{
		Listeners.Add(action);
		foreach (IEventInterface<N> button in Invokers)
		{
			button.AddListener(action);
		}
	}

	public void RemoveListener(UnityAction<N> action)
	{
		Listeners.Remove(action);
		foreach (IEventInterface<N> button in Invokers)
		{
			button.RemoveListener(action);
		}
	}

	public void ClearAllListeners()
	{
		Listeners.Clear();
		foreach (IEventInterface<N> invoker in Invokers)
		{
			invoker.RemoveAllListeners();
		}
	}
	#endregion
}

public class EventHandler
{
	#region Fields
	static List<IEventInterface> Invokers = new List<IEventInterface>();
	static List<UnityAction> Listeners = new List<UnityAction>();
	#endregion

	#region Properties
	public bool IsListenersZero
	{
		get
		{
			if (Listeners.Count == 0)
				return true;
			else
				return false;
		}
	}
	#endregion

	#region Methods
	public void AddInvoker(IEventInterface invoker)
	{
		Invokers.Add(invoker);
		foreach (UnityAction listener in Listeners)
		{
			invoker.AddListener(listener);
		}
	}

	public void AddListener(UnityAction action)
	{
		Listeners.Add(action);
		foreach (IEventInterface button in Invokers)
		{
			button.AddListener(action);
		}
	}

	public void RemoveListener(UnityAction action)
	{
		Listeners.Remove(action);
		foreach (IEventInterface button in Invokers)
		{
			button.RemoveListener(action);
		}
	}

	public void ClearAllListeners()
	{
		Listeners.Clear();
		foreach (IEventInterface invoker in Invokers)
		{
			invoker.RemoveAllListeners();
		}
	}
	#endregion
}
