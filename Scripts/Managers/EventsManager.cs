using UnityEngine.Events;

public static class EventsManager
{
	public static EventHandler<Cell> CellClicked = new EventHandler<Cell>();
	public static EventHandler TimePassed = new EventHandler();
	public static EventHandler<Unit> UnitIsReady = new EventHandler<Unit>();
}
public class CellClickedEvent : UnityEvent<Cell>, IEventInterface<Cell> { }
public class TimePassed : UnityEvent, IEventInterface { }
public class UnitEvent : UnityEvent<Unit>, IEventInterface<Unit> { }