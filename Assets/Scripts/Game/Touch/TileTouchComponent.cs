using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTouchComponent : TouchComponent {
	public override void ComponentStart() {
		BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
		if (boxCollider == null) {
			gameObject.AddComponent<BoxCollider2D>();
		}
	}

	public override void ComponentDestroy() {
		BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
		if (boxCollider != null) {
			GameObject.Destroy(boxCollider);
		}
	}
}
