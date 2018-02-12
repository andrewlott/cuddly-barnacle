using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSystem : BaseSystem {
	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(CharacterComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(CharacterComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is CharacterComponent) {
			CharacterComponent cc = c as CharacterComponent;
			GameController gameController = Controller() as GameController;

			// add character
			GameObject g = GameObject.Instantiate(gameController.CharacterPrefab, gameController.gameObject.transform);
			g.transform.SetParent(gameController.CharacterLayoutContainer);
			AnimationComponent ac = g.AddComponent<AnimationComponent>();
			ac.Trigger = "Fox"; //Utils.CharacterTypeAsString(cc.Character);
			cc.CharacterObject = g;

			// add background
			g = GameObject.Instantiate(gameController.BoardBackgroundPrefab, gameController.gameObject.transform);
			SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
			sr.color = Color.grey;

			ac = g.AddComponent<AnimationComponent>();
			ColorType ct = (ColorType)cc.Character;
			ac.Trigger = Utils.ColorTypeAsString(ct);

			// add foreground
			g = GameObject.Instantiate(gameController.CharacterBackgroundPrefab, gameController.gameObject.transform);
			g.transform.position = new Vector3(0.0f, Screen.height / 2.0f /  Utils.CameraScale / 100.0f, 0.0f);
			ac = g.AddComponent<AnimationComponent>();
			ac.Trigger = Utils.ColorTypeAsString(ct);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is CharacterComponent) {
			CharacterComponent cc = c as CharacterComponent;

		}
	}
}
