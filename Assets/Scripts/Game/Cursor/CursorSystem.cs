using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : BaseSystem {

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(TileSelectedComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(TileSelectedComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is TileSelectedComponent) {
			CursorComponent cc = c.gameObject.GetComponent<CursorComponent>();
			cc.Cursor.SetActive(true);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is TileSelectedComponent) {
			CursorComponent cc = c.gameObject.GetComponent<CursorComponent>();
			cc.Cursor.SetActive(false);
		}
	}
}
