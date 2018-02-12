using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Tile Warning

public class WarningSystem : BaseSystem {
	private WarningComponent _WC;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(WarningComponent), this);

	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(WarningComponent), this);
	}

	public override void Update() {
		if (_WC != null) {
			// need to show warning for tiles but needs to be in same frame
			// maybe do warning animation on add and in update check active tiles for animating and use any grid object as animation reference (unless clearing)
		}
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is WarningComponent) {
			_WC = c as WarningComponent;
			CharacterComponent cc = c.gameObject.GetComponent<CharacterComponent>();
			AnimationComponent ac = cc.CharacterObject.AddComponent<AnimationComponent>();
			ac.Trigger = "Warning";
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is WarningComponent) {
			_WC = null;
			CharacterComponent cc = c.gameObject.GetComponent<CharacterComponent>();
			AnimationComponent ac = cc.CharacterObject.AddComponent<AnimationComponent>();
			ac.Trigger = "Idle";
		}
	}
}
