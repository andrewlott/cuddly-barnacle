
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: If last touch is on a tile that's not touchable anymore, remove it

public class TouchSystem : BaseSystem {
	private TileTouchComponent _lastTouchComponent;

	public override void Start() {
	}

	public override void Stop() {
	}

	public override void Update() {
		CheckTouch();
	}

	public override void OnComponentAdded(BaseComponent c) {
	}

	public override void OnComponentRemoved(BaseComponent c) {
	}

	private GameObject GetTouchedObject() {
		Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
		RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
		if (hitInformation.collider != null) {
			GameObject touchedObject = hitInformation.transform.gameObject;
			return touchedObject;
		}

		return null;
	}

	private void CheckTouch() {
		if (Input.GetMouseButtonDown(0)) {
			GameObject touchedObject = GetTouchedObject();
			if (touchedObject != null) {
				TouchComponent tc = touchedObject.GetComponent<TouchComponent>();
				SwappingComponent sc = touchedObject.GetComponent<SwappingComponent>();
				if (tc != null && sc == null) {
					OnTouchDownForComponent(tc);
				}
			}
		} else if (Input.GetMouseButtonUp(0)) {
			GameObject touchedObject = GetTouchedObject();
			if (touchedObject != null) {
				TouchComponent tc = touchedObject.GetComponent<TouchComponent>();
				SwappingComponent sc = touchedObject.GetComponent<SwappingComponent>();
				if (tc != null && sc == null) {
					OnTouchUpForComponent(tc);
				}
			}
		}
	}

	private void OnTouchDownForComponent(BaseComponent c) {
		if (c is TileTouchComponent) {
			if (GC.IsHidden(c.gameObject)) {
				return;
			}
			if (_lastTouchComponent == null) {
				_lastTouchComponent = c as TileTouchComponent;
			}
			Vector2Int pos = GC.PositionInGrid(c.gameObject);
			Debug.Log(pos);
		}
	}
		
	private void OnTouchUpForComponent(BaseComponent c) {
		if (c is TileTouchComponent) {
			if (GC.IsHidden(c.gameObject)) {
				return;
			}
			if (_lastTouchComponent == c) {
				TileSelectedComponent tsc = c.gameObject.GetComponent<TileSelectedComponent>();
				if (tsc != null) {
					GameObject.Destroy(tsc);
				} else {
					c.gameObject.AddComponent<TileSelectedComponent>();
					return;
				}
			} else if (_lastTouchComponent != null && GC.AreHorizontalNeighbors(_lastTouchComponent.gameObject, c.gameObject)) {
				SwapComponent swapComponent = _lastTouchComponent.gameObject.AddComponent<SwapComponent>();
				swapComponent.Objects.Add(swapComponent.gameObject);
				swapComponent.Objects.Add(c.gameObject);

				TileSelectedComponent tsc = _lastTouchComponent.GetComponent<TileSelectedComponent>();
				GameObject.Destroy(tsc);
			}
		}

		_lastTouchComponent = null;
	}
}