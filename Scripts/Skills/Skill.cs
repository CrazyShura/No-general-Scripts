using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Unit))]
public abstract class Skill : MonoBehaviour , IEventInterface
{
	#region Fields
	protected Unit host;
	bool active;
	protected string skillName;
	protected List<Cell> targetCells = new List<Cell>();
	protected List<Cell> highlightedCells = new List<Cell>();
	protected bool cellsCalculeted = false;
	protected UnityEvent skillUsed = new UnityEvent();
	#endregion

	#region Properties
	public virtual bool Active
	{
		get => active;
		set
		{
			if (!host.Busy)
			{
				if (!cellsCalculeted)
				{
					ClearCells();
					if (CalculateCells())
					{
						cellsCalculeted = true;
					}
					else
					{
						cellsCalculeted = false;
						active = false;
						return;
					}
				}
				if (value)
				{
					PublicStuff.GameMaster.Board.HighlightArea(PublicStuff.AvalbeCellColor, HighlightedCells);
				}
				else
				{
					PublicStuff.GameMaster.Board.ResetCellColor(HighlightedCells);
				}
				active = value;
			}
		}
	}
	public string SkillName { get => skillName; }
	public List<Cell> TargetCells
	{
		get
		{
			if (!cellsCalculeted)
			{
				ClearCells();
				CalculateCells();
				cellsCalculeted = true;
			}
			return targetCells;
		}
	}
	public List<Cell> HighlightedCells
	{
		get
		{
			if (!cellsCalculeted)
			{
				ClearCells();
				CalculateCells();
				cellsCalculeted = true;
			}
			return highlightedCells;
		}
	}
	#endregion

	#region Methods
	virtual protected void Awake()
	{
		host = this.gameObject.GetComponent<Unit>();
		host.AddListener(ClearCells);
		if (!host.Initialized)
		{
			host.Initialise();
		}
	}
	protected abstract bool CalculateCells();

	public void ClearCells()
	{
		targetCells.Clear();
		highlightedCells.Clear();
		cellsCalculeted = false;
	}

	#region IEventInterface implemantation
	public void AddListener(UnityAction action)
	{
		skillUsed.AddListener(action);
	}

	public void RemoveListener(UnityAction action)
	{
		skillUsed.RemoveListener(action);
	}

	public void RemoveAllListeners()
	{
		skillUsed.RemoveAllListeners();
	}
	#endregion

	#endregion
}
