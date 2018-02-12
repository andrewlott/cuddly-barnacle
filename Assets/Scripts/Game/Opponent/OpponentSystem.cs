using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentSystem : BaseSystem {
	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(OpponentComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(OpponentComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is OpponentComponent) {
			OpponentComponent oc = c as OpponentComponent;
			GameController gameController = Controller() as GameController;

			// add character
			GameObject g = GameObject.Instantiate(gameController.OpponentPrefab, gameController.gameObject.transform);
			g.transform.SetParent(gameController.CharacterLayoutContainer);
			AnimationComponent ac = g.AddComponent<AnimationComponent>();
			ac.Trigger = "Fox"; //Utils.CharacterTypeAsString(cc.Character);
			oc.CharacterObject = g;
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is OpponentComponent) {
			
		}
	}
}
