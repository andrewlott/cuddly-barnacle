using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapComponent : BaseComponent {
	// swap list that does lshift
	// ORDER MATTERS!
	public List<GameObject> Objects = new List<GameObject>();
//	public List<Vector2Int> swapIndices = new List<Vector2Int>();
	public bool Swapped = false;
}
