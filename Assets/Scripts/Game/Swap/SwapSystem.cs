using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwapSystem : BaseSystem {
	public static readonly float SwapDuration = 0.1f;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(SwapComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(SwapComponent), this);
	}

	public override void Update() {
		List<BaseComponent> swapComponents = Pool.Instance.ComponentsForType(typeof(SwapComponent));
		foreach (SwapComponent s in swapComponents) {
			if (s.Swapped) {
				continue;
			}
			if (s.Objects.Count == 0) {
				continue;
			}

			TileComponent tc = s.gameObject.GetComponent<TileComponent>();
			GridComponent gc = tc.ParentGrid;

			Tweener lastTween = null;

			for (int i = s.Objects.Count - 1; i >= 0; i--) {
				int index = (i - 1 + s.Objects.Count) % s.Objects.Count;
				GameObject swapFromObject = s.Objects[i];
				GameObject swapToObject = s.Objects[index];
				lastTween = swapFromObject.transform.DOLocalMove(swapToObject.transform.localPosition, SwapDuration).SetEase(Ease.Linear);
				if (i < s.Objects.Count - 1) {
					gc.Swap(swapFromObject, s.Objects[i + 1]);
				}
			}
			s.Swapped = true;
			lastTween.OnComplete(()=>GameObject.Destroy(s));
		}
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is SwapComponent) {
			SwapComponent sc = c as SwapComponent;
			foreach (GameObject g in sc.Objects) {
				SwappingComponent scc = g.GetComponent<SwappingComponent>();
				if (scc == null) {
					g.AddComponent<SwappingComponent>();
				}
			}
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is SwapComponent) {
			SwapComponent sc = c as SwapComponent;
			foreach (GameObject g in sc.Objects) {
				SwappingComponent scc = g.GetComponent<SwappingComponent>();
				if (scc != null) {
					GameObject.Destroy(scc);
				}
				TileSelectedComponent tsc = g.GetComponent<TileSelectedComponent>();
				if (tsc != null) {
					GameObject.Destroy(tsc);
				}
			}
		}
	}
}
