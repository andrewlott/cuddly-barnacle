using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType {
	None = 0,
	Whale = 1,
	Lizard = 2,
	Cat = 3,
	Penguin = 4,
	Fox = 5,
}

public class CharacterComponent : BaseComponent {
	public CharacterType Character;
	public GameObject CharacterObject;
}
