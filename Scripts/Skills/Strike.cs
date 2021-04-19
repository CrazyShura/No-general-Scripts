using UnityEngine;

public class Strike : Skill
{
	#region Fields
	[SerializeField]
	float damage = 10;
	[SerializeField]
	float connectionDelay = 0;

	Cell target = null;
	#endregion

	#region Properties

	#endregion

	#region Methods
	private void Update()
	{
		if (host.MyTurn && Active && Input.GetMouseButtonDown(0))
		{
			Cell cell = PublicStuff.GetCellOnMousePosition();
			if (cell != null && targetCells.Contains(cell))
			{
				skillUsed.Invoke();
				target = cell;
				Invoke("DoDamage", connectionDelay);
			}
			Active = false;
			host.ResetSkillsCells();
		}
	}

	void DoDamage()
	{
		if (target != null)
		{
			target.Occupant.ReceiveDamage(damage , host.Graphics.transform.forward);
		}
		else
		{
			Debug.LogError("Target is null");
		}
	}

	protected override bool CalculateCells()
	{
		for (int i = -1; i <= host.Size; i++)
		{
			for (int j = -1; j <= host.Size; j++)
			{
				Cell tempCell = PublicStuff.GameMaster.Board.GetCell(host.BoardPosition.X + i, host.BoardPosition.Y + j);
				if (tempCell.Occupant != null && tempCell.Occupant != host)
				{
					foreach (Cell cell in tempCell.Occupant.Occupation)
					{
						targetCells.Add(cell);
						highlightedCells.Add(cell);
					}
				}
			}
		}
		if (targetCells.Count > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	#endregion
}
