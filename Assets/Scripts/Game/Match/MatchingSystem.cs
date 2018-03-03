using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingSystem : BaseSystem {

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
	}

	public override void Update() {
		List<BaseComponent> matchableComponents = Pool.Instance.ComponentsForType(typeof(MatchableComponent));
		foreach (MatchableComponent mc in matchableComponents) {
			TileComponent tc = mc.gameObject.GetComponent<TileComponent>();
			GridComponent gc = tc.ParentGrid;
			if (gc.IsHidden(mc.gameObject)) {
				continue;
			}

			if (mc.hasBeenChecked) {
				continue;
			}

			List<MatchableComponent> matchList = FindMatch(mc, gc);
			if (matchList.Count == 0) {
				continue;
			}

			MatchComponent matchComponent = Controller().gameObject.AddComponent<MatchComponent>();
			foreach (MatchableComponent m in matchList) {
				matchComponent.matchedObjects.Add(m.gameObject);
			}
			foreach (MatchableComponent m in matchList) {
				GameObject.Destroy(m);
			}
		}

		foreach (MatchableComponent mc in matchableComponents) {
			mc.hasBeenChecked = false;
		}
	}

	private List<MatchableComponent> FindMatch(MatchableComponent mc, GridComponent gc) {
		List<MatchableComponent> queue = new List<MatchableComponent> { mc };
		List<MatchableComponent> checkedList = new List<MatchableComponent>();
		List<MatchableComponent> matchList = new List<MatchableComponent>();
		while (queue.Count > 0) {
			MatchableComponent currentMC = queue[0];
			queue.RemoveAt(0);

			if (gc.IsHidden(mc.gameObject)) {
				continue;
			}

			ColorComponent currentCC = currentMC.gameObject.GetComponent<ColorComponent>();
			if (currentCC.color == ColorType.None) {
				continue;
			}

			SwappingComponent sc = currentMC.gameObject.GetComponent<SwappingComponent>();
			if (sc != null) {
				continue;
			}

			FallingComponent fc = currentMC.gameObject.GetComponent<FallingComponent>();
			if (fc != null) {
				continue;
			}

			GameObject topNeighbor = gc.TopNeighbor(currentMC.gameObject);
			GameObject bottomNeighbor = gc.BottomNeighbor(currentMC.gameObject);
			if (topNeighbor != null && bottomNeighbor != null) {
				SwappingComponent topSwap = topNeighbor.GetComponent<SwappingComponent>();
				SwappingComponent bottomSwap = bottomNeighbor.GetComponent<SwappingComponent>();
				if (topSwap == null && bottomSwap == null) {
					FallingComponent topFall = topNeighbor.GetComponent<FallingComponent>();
					FallingComponent bottomFall = bottomNeighbor.GetComponent <FallingComponent>();
					if (topFall == null && bottomFall == null) {
						MatchableComponent topMC = topNeighbor.GetComponent<MatchableComponent>();
						MatchableComponent bottomMC = bottomNeighbor.GetComponent<MatchableComponent>();
						if (topMC != null && bottomMC != null) {
							ColorComponent topColor = topNeighbor.gameObject.GetComponent<ColorComponent>();
							ColorComponent bottomColor = bottomNeighbor.gameObject.GetComponent<ColorComponent>();
							if (topColor.color == currentCC.color && bottomColor.color == currentCC.color) {
								if (!matchList.Contains(currentMC)) {
									matchList.Add(currentMC);
								}
								if (!matchList.Contains(topMC)) {
									matchList.Add(topMC);
								}
								if (!matchList.Contains(bottomMC)) {
									matchList.Add(bottomMC);
								}

								if (!checkedList.Contains(topMC)) {
									queue.Add(topMC);
								}
								if (!checkedList.Contains(bottomMC)) {
									queue.Add(bottomMC);
								}
							}
						}
					}
				}
			}

			GameObject leftNeighbor = gc.LeftNeighbor(currentMC.gameObject);
			GameObject rightNeighbor = gc.RightNeighbor(currentMC.gameObject);
			if (leftNeighbor != null && rightNeighbor != null) {
				SwappingComponent leftSwap = leftNeighbor.GetComponent<SwappingComponent>();
				SwappingComponent rightSwap = rightNeighbor.GetComponent<SwappingComponent>();
				if (leftSwap == null && rightSwap == null) {
					FallingComponent leftFall = leftNeighbor.GetComponent<FallingComponent>();
					FallingComponent rightFall = rightNeighbor.GetComponent <FallingComponent>();
					if (leftFall == null && rightFall == null) {
						MatchableComponent leftMC = leftNeighbor.GetComponent<MatchableComponent>();
						MatchableComponent rightMC = rightNeighbor.GetComponent<MatchableComponent>();
						if (leftMC != null && rightMC != null) {
							ColorComponent leftColor = leftNeighbor.gameObject.GetComponent<ColorComponent>();
							ColorComponent rightColor = rightNeighbor.gameObject.GetComponent<ColorComponent>();
							if (leftColor.color == currentCC.color && rightColor.color == currentCC.color) {
								if (!matchList.Contains(currentMC)) {
									matchList.Add(currentMC);
								}
								if (!matchList.Contains(leftMC)) {
									matchList.Add(leftMC);
								}
								if (!matchList.Contains(rightMC)) {
									matchList.Add(rightMC);
								}

								if (!checkedList.Contains(leftMC)) {
									queue.Add(leftMC);
								}
								if (!checkedList.Contains(rightMC)) {
									queue.Add(rightMC);
								}
							}
						}
					}
				}
			}

			currentMC.hasBeenChecked = true;
			checkedList.Add(currentMC);
		}
		return matchList;
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is MatchComponent) {
			MatchComponent mc = c as MatchComponent;
			foreach (GameObject g in mc.matchedObjects) {
				AnimationComponent ac = g.AddComponent<AnimationComponent>();
				ac.Trigger = "Clear";
				ac.Callback = ClearAnimationCallback;
				ac.CallbackState = "Empty";
			}

			GameObject.Destroy(mc);
		}
	}

	public void ClearAnimationCallback(GameObject g) {
		ColorComponent gc = g.GetComponent<ColorComponent>();
		GameObject.Destroy(gc);
		gc = g.AddComponent<ColorComponent>();
		gc.color = ColorType.None;
		gc.shouldRandomize = false;
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is MatchComponent) {
			MatchComponent mc = c as MatchComponent;
			Debug.LogFormat("Match of size {0}!", mc.matchedObjects.Count);
		}
	}
}
