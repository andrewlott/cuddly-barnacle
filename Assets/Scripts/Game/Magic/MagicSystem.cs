using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSystem : BaseSystem {
	private Slider _MPBar;
	private MagicComponent _MagicComponent;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);

		_MPBar = (Controller() as GameController).MPBar;
		_MagicComponent = _MPBar.GetComponent<MagicComponent>();
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
	}

	public override void Update() {
		if (_MagicComponent != null) {
			_MPBar.value = Mathf.Min(1.0f, _MagicComponent.MP / MagicComponent.MaxMP);
		}
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is MatchComponent) {
			MatchComponent mc = c as MatchComponent;
			CharacterComponent chc = Pool.Instance.ComponentForType(typeof(CharacterComponent)) as CharacterComponent;
			foreach (GameObject g in mc.matchedObjects) {
				ColorComponent cc = g.GetComponent<ColorComponent>();
				if ((int)chc.Character == (int)cc.color) {
					_MagicComponent.MP += MagicComponent.MPIncrement;
				}
			}
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is MagicComponent) {

		}
	}
}
