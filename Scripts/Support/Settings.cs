using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Setting" , menuName = "New Setting")]
public class Settings : ScriptableObject
{
	#region Fields
	[SerializeField]
	Color defaultCellColor;
	[SerializeField]
	Color occupiedCellColor;
	[SerializeField]
	Color unavalbeCellColor;
	[SerializeField]
	Color avalbeCellColor;

	#endregion

	#region Properies
	public Color DefaultCellColor { get => defaultCellColor;}
	public Color OccupiedCellColor { get => occupiedCellColor;}
	public Color UnavalbeCell { get => unavalbeCellColor;}
	public Color AvalbeCell { get => avalbeCellColor;}
	#endregion

	#region Methods

	#endregion
}
