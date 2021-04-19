using System.Collections.Generic;
using UnityEngine;

public class Move : Skill
{
	#region Fields
	[SerializeField]
	int moveDistance = 3;

	float maxSpeed = 0;
	Cell targetCell;
	Dictionary<Cell, int> pathGrid = new Dictionary<Cell, int>();

	List<Cell> pathToTake = new List<Cell>();

	List<GameObject> pathIdecation = new List<GameObject>();
	#endregion

	#region Properties
	public override bool Active
	{
		get => base.Active;
		set
		{
			base.Active = value;
			if (!value)
			{
				foreach (GameObject indicator in pathIdecation)
				{
					Destroy(indicator);
				}
				pathIdecation.Clear();
			}
		}
	}
	#endregion

	#region Methods
	protected override void Awake()
	{
		base.Awake();
		maxSpeed = host.Agent.speed;
		EventsManager.CellClicked.AddListener(CalculatePath);
		skillName = "Move";
	}

	private void FixedUpdate()
	{
		if (host.MyTurn && Active)
		{
			Cell cell = PublicStuff.GetCellOnMousePosition(host.Size);
			if (cell != null && targetCells.Contains(cell))
			{
				if (cell != targetCell)
				{
					foreach (GameObject indicator in pathIdecation)
					{
						Destroy(indicator);
					}
					pathIdecation.Clear();
					Cell prevCell = cell;
					List<Cell> movePath = new List<Cell>();
					movePath.Add(host.BoardPosition);
					int tooMuch = 100;
					while (prevCell != host.BoardPosition)
					{
						movePath.Add(prevCell);
						Cell contender = null;
						for (int i = -1; i < 2; i++)
						{
							for (int j = -1; j < 2; j++)
							{
								Cell tempCell = PublicStuff.GameMaster.Board.GetCell(prevCell.X + i, prevCell.Y + j);
								if (targetCells.Contains(tempCell) && pathGrid[prevCell] < pathGrid[tempCell])
								{
									if (contender != null)
									{
										if (pathGrid[contender] < pathGrid[tempCell])
										{
											contender = tempCell;
										}
									}
									else
									{
										contender = tempCell;
									}
								}
							}
						}
						prevCell = contender;
						tooMuch--;
						if (tooMuch < 0)
						{
							Debug.LogError("Loop limit reached");
							break;
						}
					}
					if (movePath.Count > 0)
					{
						foreach (Cell nextCell in movePath)
						{
							GameObject tempObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
							tempObj.transform.localScale = Vector3.one * .2f * host.Size;
							tempObj.transform.position = nextCell.transform.position + new Vector3((host.Size - 1) * .5f, .3f * host.Size, (host.Size - 1) * .5f);
							pathIdecation.Add(tempObj);
						}
					}

					List<Cell> temp = new List<Cell>();
					if (targetCell != null)
					{
						for (int i = 0; i < host.Size; i++)
						{
							for (int j = 0; j < host.Size; j++)
							{
								temp.Add(PublicStuff.GameMaster.Board.GetCell(targetCell.X + i, targetCell.Y + j));
							}
						}
						PublicStuff.GameMaster.Board.HighlightArea(PublicStuff.AvalbeCellColor, temp);
						temp.Clear();
					}
					for (int i = 0; i < host.Size; i++)
					{
						for (int j = 0; j < host.Size; j++)
						{
							temp.Add(PublicStuff.GameMaster.Board.GetCell(cell.X + i, cell.Y + j));
						}
					}
					PublicStuff.GameMaster.Board.HighlightArea(PublicStuff.OccupiedCellColor, temp);
					targetCell = cell;
				}
			}
			else
			{
				if (targetCell != null)
				{
					foreach (GameObject indicator in pathIdecation)
					{
						Destroy(indicator);
					}
					pathIdecation.Clear();
					List<Cell> temp = new List<Cell>();
					for (int i = 0; i < host.Size; i++)
					{
						for (int j = 0; j < host.Size; j++)
						{
							temp.Add(PublicStuff.GameMaster.Board.GetCell(targetCell.X + i, targetCell.Y + j));
						}
					}
					PublicStuff.GameMaster.Board.HighlightArea(PublicStuff.AvalbeCellColor, temp);
					targetCell = null;
				}
			}
		}
	}

	private void Update()
	{
		if (host.MyTurn && Active && Input.GetMouseButtonDown(0))
		{
			Cell cell = PublicStuff.GetCellOnMousePosition(host.Size);
			if (cell != null && targetCells.Contains(cell))
			{
				CalculatePath(cell);
				host.Busy = true;
			}
		}
		if (pathToTake.Count > 0)
		{
			switch (pathToTake.Count)
			{
				case 1:
					{
						host.Agent.autoBraking = true;
						break;
					}
				case 2:
					{
						host.Agent.speed = maxSpeed * 3/4;
						break;
					}
				default:
					{
						host.Agent.autoBraking = false;
						host.Agent.speed = maxSpeed;
						break;
					}
			}
			if (Vector3.Distance(pathToTake[pathToTake.Count - 1].transform.position + new Vector3((host.Size - 1) * .5f, 0, (host.Size - 1) * .5f), this.transform.position) < .4f * host.Size)
			{
				pathToTake.Remove(pathToTake[pathToTake.Count - 1]);
				if (pathToTake.Count == 0)
				{
					host.Busy = false;
				}
				else
				{
					host.Agent.SetDestination(pathToTake[pathToTake.Count - 1].transform.position + new Vector3((host.Size - 1) * .5f, 0, (host.Size - 1) * .5f));
				}
			}
		}
	}

	void CalculatePath(Cell target)
	{
		if (target != host.Occupation[0])
		{
			Cell prevCell = target;
			pathToTake.Clear();
			int tooMuch = 100;
			while (prevCell != host.BoardPosition)
			{
				pathToTake.Add(prevCell);
				Cell contender = null;
				for (int i = -1; i < 2; i++)
				{
					for (int j = -1; j < 2; j++)
					{
						Cell temp = PublicStuff.GameMaster.Board.GetCell(prevCell.X + i, prevCell.Y + j);
						if (targetCells.Contains(temp) && pathGrid[prevCell] < pathGrid[temp])
						{
							if (contender != null)
							{
								if (pathGrid[contender] < pathGrid[temp])
								{
									contender = temp;
								}
							}
							else
							{
								contender = temp;
							}
						}
					}
				}
				prevCell = contender;
				tooMuch--;
				if (tooMuch < 0)
				{
					Debug.LogError("Loop limit reached");
					break;
				}
			}
			pathToTake.Add(host.BoardPosition);
			foreach (Cell cell in host.Occupation)
			{
				cell.Occupant = null;
			}
			host.Occupation.Clear();
			for (int i = 0; i < host.Size; i++)
			{
				for (int j = 0; j < host.Size; j++)
				{
					PublicStuff.GameMaster.Board.GetCell(target.X + i, target.Y + j).Occupant = host;
					host.Occupation.Add(PublicStuff.GameMaster.Board.GetCell(target.X + i, target.Y + j));
				}
			}
		}
		else
		{
			Debug.Log("Target is the currently ocupied cell");
			return;
		}
		foreach (GameObject indicator in pathIdecation)
		{
			Destroy(indicator);
		}
		pathIdecation.Clear();
		Active = false;
		host.ResetSkillsCells();
	}

	protected override bool CalculateCells()
	{
		List<Cell> pathList = new List<Cell>();
		pathGrid.Clear();
		pathList.Add(host.BoardPosition);
		pathGrid.Add(host.BoardPosition, moveDistance);
		int tooMuch = 1000;
		while (pathList.Count > 0)
		{
			targetCells.Add(pathList[0]);
			for (int a = 0; a < host.Size; a++)
			{
				for (int b = 0; b < host.Size; b++)
				{
					highlightedCells.Add(PublicStuff.GameMaster.Board.GetCell(pathList[0].X + a, pathList[0].Y + b));
				}
			}
			if (pathGrid[pathList[0]] > 0)
			{
				for (int i = 0; i < 4; i++)
				{
					int z = 1, w = 1;
					switch (i)
					{
						case 0:
							{
								z = -1;
								w = 0;
								break;
							}
						case 1:
							{
								z = 1;
								w = 0;
								break;
							}
						case 2:
							{
								z = 0;
								w = -1;
								break;
							}
						case 3:
							{
								z = 0;
								w = 1;
								break;
							}
					}
					Cell cell = PublicStuff.GameMaster.Board.GetCell(pathList[0].X + z, pathList[0].Y + w);
					if (cell != null)
					{
						if ((cell.Occupant == null || cell.Occupant == host))
						{
							bool addCell = true;
							for (int a = 0; a < host.Size; a++)
							{
								for (int b = 0; b < host.Size; b++)
								{
									Cell cellToCheck = PublicStuff.GameMaster.Board.GetCell(cell.X + a, cell.Y + b);
									if (cellToCheck != null)
									{
										if (cellToCheck.Occupant != null && cellToCheck.Occupant != host)
										{
											addCell = false;
										}
									}
									else
									{
										addCell = false;
									}
								}
							}
							if (addCell)
							{
								if (pathGrid.ContainsKey(cell))
								{
									if (pathGrid[cell] < pathGrid[pathList[0]] - 1)
									{
										pathGrid.Remove(cell);
										pathGrid.Add(cell, pathGrid[pathList[0]] - 1);
										pathList.Add(cell);
									}
								}
								else
								{
									pathGrid.Add(cell, pathGrid[pathList[0]] - 1);
									pathList.Add(cell);
								}
							}
						}
					}
				}
			}
			pathList.Remove(pathList[0]);
			tooMuch--;
			if (tooMuch <= 0)
			{
				Debug.LogError("Loop limit reached");
				return false;
			}
		}
		if (targetCells.Count == 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	#endregion

	#region Legacy
	/* ///Old move code///
		void MoveToCell(Cell target)
		{
			if (target != host.Occupation[0])
			{
				bool spaceIsFree = true;
				for (int i = 0; i < host.Size; i++)
				{
					for (int j = 0; j < host.Size; j++)
					{
						Cell cell = PublicStuff.GameMaster.Board.GetCell(target.X + i, target.Y + j);
						if (cell != null)
						{
							if (cell.Occupant != null && cell.Occupant != host)
							{
								spaceIsFree = false;
							}
						}
						else
						{
							Debug.Log("Target is out of bounds");
							Active = false;
							return;
						}
					}
				}
				if (spaceIsFree)
				{
					foreach (Cell cell in host.Occupation)
					{
						cell.Occupant = null;
					}
					host.Occupation.Clear();
					this.transform.position = target.transform.position + new Vector3((host.Size - 1) * .5f, 0, (host.Size - 1) * .5f);
					for (int i = 0; i < host.Size; i++)
					{
						for (int j = 0; j < host.Size; j++)
						{
							PublicStuff.GameMaster.Board.GetCell(target.X + i, target.Y + j).Occupant = host;
							host.Occupation.Add(PublicStuff.GameMaster.Board.GetCell(target.X + i, target.Y + j));
						}
					}
					PublicStuff.GameMaster.Board.ResetCellColor();
				}
				else
				{
					Debug.Log("Target Cell is occupied");
				}
			}
			else
			{
				Debug.Log("Target is the currently ocupied cell");
			}
			Active = false;
			cellsCalculeted = false;
		}
	*/

	/* ///Old move code wich is more flying then walking///
	for (int i = -moveDistance; i <= moveDistance + host.Size - 1; i++)
	{
		for (int j = -moveDistance; j <= moveDistance + host.Size - 1; j++)
		{
			Cell cell = PublicStuff.GameMaster.Board.GetCell(host.BoardPosition.X + i, host.BoardPosition.Y + j);
			if (cell != null)
			{
				if ((cell.Occupant == null || cell.Occupant == host) && cell != host.BoardPosition)
				{
					int temp = Mathf.Abs(cell.X - host.BoardPosition.X) + Mathf.Abs(cell.Y - host.BoardPosition.Y);
					if (temp <= moveDistance)
					{
						bool addCell = true;
						for (int a = 0; a < host.Size; a++)
						{
							for (int b = 0; b < host.Size; b++)
							{
								Cell cellToCheck = PublicStuff.GameMaster.Board.GetCell(cell.X + a, cell.Y + b);
								if (cellToCheck != null)
								{
									if (cellToCheck.Occupant != null && cellToCheck.Occupant != host)
									{
										addCell = false;
									}
								}
								else
								{
									addCell = false;
								}
							}
						}
						if (addCell)
						{
							targetCells.Add(cell);
							for (int a = 0; a < host.Size; a++)
							{
								for (int b = 0; b < host.Size; b++)
								{
									highlightedCells.Add(PublicStuff.GameMaster.Board.GetCell(cell.X + a, cell.Y + b));
								}
							}
						}
					}
				}
			}
			else
			{
				continue;
			}
		}
	}
	*/
	#endregion
}
