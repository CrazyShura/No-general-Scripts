using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInterface<T>
{
	void AddListener(UnityAction<T> action);
	void RemoveListener(UnityAction<T> action);
	void RemoveAllListeners();
}

public interface IEventInterface
{
	void AddListener(UnityAction action);
	void RemoveListener(UnityAction action);
	void RemoveAllListeners();
}
