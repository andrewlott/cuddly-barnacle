using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : BaseSystem {
	Dictionary<GameObject, GameObject> CursorDictionary = new Dictionary<GameObject, GameObject>();

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
			GameObject g = GameObject.Instantiate((Controller() as GameController).CursorPrefab);
			TileComponent tc = c.gameObject.GetComponent<TileComponent>();
			GridComponent gc = tc.ParentGrid;
			g.transform.position = gc.PositionForGameObject(c.gameObject);
			g.transform.SetParent(gc.gameObject.transform);
			CursorDictionary.Add(c.gameObject, g);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is TileSelectedComponent) {
			GameObject g = CursorDictionary[c.gameObject];
			CursorDictionary.Remove(c.gameObject);
			Utils.DestroyEntity(g);
		}
	}
}
