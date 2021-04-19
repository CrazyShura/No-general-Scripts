using UnityEngine;

public static class PublicStuff
{
	#region Fields
	static GM gm;
	static Color defaultCellColor;
	static Color occupiedCellColor;
	static Color unavalbeCell;
	static Color avalbeCell;
	static bool initilised = false;
	static Plane plane = new Plane(Vector3.up, 0f);
	static LayerMask mask;
	#endregion

	#region Properies
	static public GM GameMaster { get { return gm; } }
	static public Color DefaultCellColor { get => defaultCellColor; }
	static public Color OccupiedCellColor { get => occupiedCellColor; }
	static public Color UnavalbeCellColor { get => unavalbeCell; }
	public static Color AvalbeCellColor { get => avalbeCell; }
	public static bool Initilised { get => initilised;}
	#endregion

	#region Methods
	static public void SetGM(GM target)
	{
		if (gm == null || gm == target)
		{
			gm = target;
		}
		else
		{
			Debug.LogError("Tring to assign GM when alredy have assigned GM");
			GameObject.Destroy(target);
		}
	}

	static public void Initilize()
	{
		Settings settings = (Settings)Resources.Load("Settings/Standart");
		defaultCellColor = settings.DefaultCellColor;
		occupiedCellColor = settings.OccupiedCellColor;
		unavalbeCell = settings.UnavalbeCell;
		avalbeCell = settings.AvalbeCell;
		mask = LayerMask.GetMask("Cells");
		initilised = true;
	}

	/// <summary>
	/// Tries to get cell from mouse position.
	/// </summary>
	/// <param name="SizeOffset">Offset for raycast.</param>
	/// <returns></returns>
	static public Cell GetCellOnMousePosition(int SizeOffset = 1)
	{
		Vector3 anchor = Vector3.zero;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float dist;
		if (plane.Raycast(ray, out dist))
		{
			anchor = ray.GetPoint(dist);
		}
		anchor += new Vector3(-(SizeOffset - 1) * .5f, 5f, -(SizeOffset - 1) * .5f);
		Cell cell = null;
		RaycastHit[] raycastHits = Physics.RaycastAll(anchor, Vector3.down, 20f, mask);
		foreach (RaycastHit hit in raycastHits)
		{
			if (hit.collider.gameObject.CompareTag("Cell"))
			{
				cell = hit.collider.gameObject.GetComponent<Cell>();
				return cell;
			}
		}
		return null;
	}
	#endregion
}
