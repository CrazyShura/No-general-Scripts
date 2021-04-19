using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
	#region Fields
	static GM instance;
	Unit currentUnit = null;
	Board board;
	List<Unit> units = new List<Unit>();
	List<Unit> currentTurn = new List<Unit>();

	TimePassed timePassed = new TimePassed();
	#endregion

	#region Properies
	public Board Board
	{
		get { return board; }
	}
	public Unit CurrentUnit
	{
		get{ return currentUnit; }
	}
	#endregion

	#region Methods
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Tring to make more then one GM");
			Destroy(this);
		}
		else
		{
			instance = this;
			PublicStuff.SetGM(this);
		}
		if(!PublicStuff.Initilised)
		{
			PublicStuff.Initilize();
		}
	}

	private void Start()
	{
		EventsManager.TimePassed.AddInvoker(timePassed);
		EventsManager.UnitIsReady.AddListener(AddUnitToTurn);
	}

	private void Update()
	{
		if (currentTurn.Count == 0)
		{
			timePassed.Invoke();
		}
		else
		{
			if (!currentTurn[0].MyTurn)
			{
				Debug.Log("Unit " + currentTurn[0].name + " is ready");
				currentTurn[0].MyTurn = true;
				currentTurn[0].AddListener(UnitTurnDone);
				currentUnit = currentTurn[0];
			}
		}
	}

	public void SetBoard(Board target)
	{
		if (target != null)
		{
			if (board == null)
			{
				board = target;
			}
			else
			{
				Debug.Log("Changing board");
				Destroy(board.gameObject);
				board = target;
			}
		}
	}

	public void AddUnit(Unit target)
	{
		if (!units.Contains(target))
		{
			units.Add(target);
		}
	}

	public void RemoveUnit(Unit target)
	{
		if (units.Contains(target))
		{
			units.Remove(target);
		}
		else
		{
			Debug.LogError("Trying to remove unit that isnt in all units list");
		}
	}


	void AddUnitToTurn(Unit target)
	{
		currentTurn.Add(target);
	}

	void UnitTurnDone()
	{
		//Debug.Log("Unit " + currentTurn[0].name + " turn is over");
		currentTurn[0].MyTurn = false;
		currentTurn[0].RemoveListener(UnitTurnDone);
		currentTurn.Remove(currentTurn[0]);
		currentUnit = null;
		board.ResetCellColor();
	}
	#endregion
}
