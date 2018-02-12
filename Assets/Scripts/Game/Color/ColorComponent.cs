using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType {
	None = 0,
	Blue = 1,
	Green = 2,
	Red = 3,
	Teal = 4,
	Yellow = 5,
	Block = 6
}

public class ColorComponent : BaseComponent {
	public ColorType color;
	public bool shouldRandomize = true;
}
