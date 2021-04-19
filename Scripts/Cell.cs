using UnityEngine;

public class Cell : MonoBehaviour
{
	#region Fields
	int x;
	int y;
	bool initilized;
	CellClickedEvent cellClicked = new CellClickedEvent();
	Unit occupant;
	Material material;
	Color targetColor;
	#endregion

	#region Properies
	public Unit Occupant
	{
		get { return occupant; }
		set
		{
			if (value != null)
			{
				TargetColor = PublicStuff.UnavalbeCellColor;
			}
			else
			{
				TargetColor = PublicStuff.AvalbeCellColor;
			}
			occupant = value;
		}
	}
	public int X
	{
		get { return x; }
		set
		{
			if (!initilized)
			{
				x = value;
			}
			else
			{
				Debug.LogError("Trying to change value of X in initilized cell");
			}
		}
	}
	public int Y
	{
		get { return y; }
		set
		{
			if (!initilized)
			{
				y = value;
			}
			else
			{
				Debug.LogError("Trying to change value of Y in initilized cell");
			}
		}
	}
	public Color TargetColor
	{
		get { return targetColor; }
		set { targetColor = value; }
	}

	public bool Initilized
	{
		get { return initilized; }
		set
		{
			if (!initilized && value)
			{
				initilized = true;
			}
			else
			{
				if (initilized)
				{
					Debug.LogError("Trying to initilise cell that is alredy intilised");
				}
				else
				{
					Debug.LogWarning("Why?");
				}
			}
		}
	}
	#endregion

	#region Methods
	private void Awake()
	{
		material = this.transform.GetChild(0).GetComponent<MeshRenderer>().material;
		material.color = PublicStuff.DefaultCellColor;
		targetColor = PublicStuff.DefaultCellColor;
	}

	private void Start()
	{
		EventsManager.CellClicked.AddInvoker(cellClicked);
	}

	private void Update()
	{
		material.color = Color.Lerp(material.color, targetColor, Time.deltaTime * 10);
	}

	public void ClickMe()
	{
		cellClicked.Invoke(this);
	}
	/*
	private void OnMouseDown()
	{
		cellClicked.Invoke(this);
	}

	private void OnMouseOver()
	{

		//Put this stuff in Board class

		if (PublicStuff.GameMaster.CurrentUnit != null)
		{
			int size = PublicStuff.GameMaster.CurrentUnit.Size;
			Color colorToSet = PublicStuff.AvalbeCellColor;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (PublicStuff.GameMaster.Board.GetCell(x + i, y + j) != null)
					{
						if (PublicStuff.GameMaster.Board.GetCell(x + i, y + j).Occupant != null)
						{
							if (PublicStuff.GameMaster.CurrentUnit != PublicStuff.GameMaster.Board.GetCell(x + i, y + j).Occupant)
							{
								colorToSet = PublicStuff.UnavalbeCellColor;
							}
						}
					}
					else
					{
						colorToSet = PublicStuff.UnavalbeCellColor;
						continue;
					}
				}
			}
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (PublicStuff.GameMaster.Board.GetCell(x + i, y + j) != null)
					{
						if (PublicStuff.GameMaster.Board.GetCell(x + i, y + j).Occupant == null)
						{
							PublicStuff.GameMaster.Board.GetCell(x + i, y + j).TargetColor = colorToSet;
						}
					}
					else
					{
						continue;
					}
				}
			}
		}
	}

	private void OnMouseExit()
	{
		if (PublicStuff.GameMaster.CurrentUnit != null)
		{
			int size = PublicStuff.GameMaster.CurrentUnit.Size;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (PublicStuff.GameMaster.Board.GetCell(x + i, y + j) != null)
					{
						if (PublicStuff.GameMaster.Board.GetCell(x + i, y + j).Occupant == null)
						{
							PublicStuff.GameMaster.Board.GetCell(x + i, y + j).TargetColor = PublicStuff.DefaultCellColor;
						}
					}
					else
					{
						continue;
					}
				}

			}

		}
	}
	*/
	#endregion
}
