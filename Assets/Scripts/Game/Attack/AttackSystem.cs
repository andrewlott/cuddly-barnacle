using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : BaseSystem {
	private static readonly int _MinMatchForAttack = 4;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(AttackComponent), this);
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(AttackComponent), this);
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is AttackComponent) {
			CharacterComponent cc = c.gameObject.GetComponent<CharacterComponent>();
			AnimationComponent ac = cc.CharacterObject.AddComponent<AnimationComponent>();
			ac.Trigger = "Attack";
		} else if (c is MatchComponent) {
			MatchComponent mc = c as MatchComponent;
			if (mc.matchedObjects.Count >= _MinMatchForAttack) {
				mc.gameObject.AddComponent<AttackComponent>();
			}
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is AttackComponent) {

		}
	}
}
