using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	#region Fields
	Cell[,] cells;
	[SerializeField]
	int width;
	[SerializeField]
	int height;
	#endregion

	#region Properies
	public int Width { get => width; }
	public int Height { get => height; }
	#endregion

	#region Methods
	private void Awake()
	{
		cells = new Cell[Width, Height];
		GameObject cellOgj = (GameObject)Resources.Load("Generation/Cell");
		GameObject boardGrid = new GameObject { name = "BoardGrid" };
		boardGrid.transform.parent = this.transform;
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				GameObject temp = Instantiate(cellOgj);
				temp.name = "Cell(" + x + "," + y + ")";
				temp.transform.parent = boardGrid.transform;
				temp.transform.position = new Vector3((float)(-Width / 2) + x, this.transform.position.y, (float)(-Height / 2) + y);
				cells[x, y] = temp.GetComponent<Cell>();
				cells[x, y].X = x;
				cells[x, y].Y = y;
				cells[x, y].Initilized = true;
			}
		}
		PublicStuff.GameMaster.SetBoard(this);
	}

	public Cell GetCell(int X, int Y)
	{
		if (X < Width && Y < Height && X >= 0 && Y >= 0) 
		{
			return cells[X, Y];
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// Resets color for cells in given list. If given Null will reset color on whole board.
	/// </summary>
	/// <param name="CellsToReset"></param>
	public void ResetCellColor(List<Cell> CellsToReset = null)
	{
		if(CellsToReset == null)
		{
			foreach (Cell cell in cells)
			{
				if (cell.Occupant == null)
				{
					cell.TargetColor = PublicStuff.DefaultCellColor;
				}
				else
				{
					cell.TargetColor = PublicStuff.UnavalbeCellColor;
				}
			}
		}
		else
		{
			foreach (Cell cell in CellsToReset)
			{
				if (cell.Occupant == null)
				{
					cell.TargetColor = PublicStuff.DefaultCellColor;
				}
				else
				{
					cell.TargetColor = PublicStuff.UnavalbeCellColor;
				}
			}
		}
	}

	public void HighlightArea(Color HighlightColor, List<Cell> AreaToHighligt)
	{
		foreach(Cell cell in AreaToHighligt)
		{
			cell.TargetColor = HighlightColor;
		}
	}
	#endregion
}
