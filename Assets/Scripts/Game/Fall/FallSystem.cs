using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSystem : BaseSystem {
//	private float _fallDelay = 2.0f;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(SwappingComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(SwappingComponent), this);
	}

	public override void Update() {
		List<BaseComponent> fallComponents = Pool.Instance.ComponentsForType(typeof(FallComponent));
		foreach (FallComponent c in fallComponents) {
			SwappingComponent swappingComponent = c.GetComponent<SwappingComponent>();
			if (swappingComponent != null) {
				continue;
			}
			ColorComponent fcc = c.GetComponent<ColorComponent>();
			if (fcc.color != ColorType.None) {
				continue;
			}
			List<GameObject> objectsAbove = GC.AllObjectsAbove(c.gameObject);
			if (objectsAbove.Count == 0) {
				continue;
			}
//			FallingComponent fc = c.gameObject.GetComponent<FallingComponent>();
//			if (fc != null) {
//				continue;
//			}
//
			List<GameObject> swappableObjectsAbove = new List<GameObject>();
			foreach (GameObject g in objectsAbove) {
				ColorComponent cc = g.GetComponent<ColorComponent>();
				SwappingComponent scc = g.GetComponent<SwappingComponent>();
				if (cc.color == ColorType.None || scc != null) {
					break;
				}
				if (cc.color == ColorType.Block && !GC.CanBlockFall(g)) {
					break;
				}
				swappableObjectsAbove.Add(g);

				g.AddComponent<FallingComponent>();
			}

			if (swappableObjectsAbove.Count == 0) {
				continue;
			}

			SwapComponent sc = c.gameObject.AddComponent<SwapComponent>();
			sc.Objects.Add(c.gameObject);
			sc.Objects.AddRange(swappableObjectsAbove);
		}
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is SwappingComponent) {
			
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is SwappingComponent) {
			FallingComponent fc = c.gameObject.GetComponent<FallingComponent>();
			if (fc != null) {
				GameObject.Destroy(fc);
			}
		}
	}
}
