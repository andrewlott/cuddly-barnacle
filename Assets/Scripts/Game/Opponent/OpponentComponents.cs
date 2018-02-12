using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentComponent : BaseComponent {
	public CharacterType Character;
	public GameObject CharacterObject;
}

public class SimpleAIOpponent : OpponentComponent {

}

public class AIOpponent : SimpleAIOpponent {

}

public class SimpleRealOpponent : OpponentComponent {

}

public class RealOpponent : SimpleRealOpponent {

}