using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : BaseSystem {
	private Slider _HPBar;
	private static readonly float _WarningThreshold = 0.2f;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(HealthComponent), this);

		_HPBar = (Controller() as GameController).HPBar;
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(HealthComponent), this);
	}

	public override void Update() {
		float playersHealth = PlayersHealth();
		_HPBar.value = playersHealth;

		WarningComponent wc = Pool.Instance.ComponentForType(typeof(WarningComponent)) as WarningComponent;
		if (playersHealth <= _WarningThreshold) {
			if (wc == null) {
				BaseObject.AddComponent<WarningComponent>();
			}
		} else {
			if (wc != null) {
				GameObject.Destroy(wc);
			}		
		}
	}

	private float PlayersHealth() {
		GridComponent gc = Pool.Instance.ComponentForType(typeof(GridComponent)) as GridComponent;
		int activeHeight = gc.ActiveHeight();
		return 1.0f - (activeHeight * 1.0f / (GridComponent.GridActiveHeight() + 1));
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is HealthComponent) {

		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is HealthComponent) {

		}
	}
}
