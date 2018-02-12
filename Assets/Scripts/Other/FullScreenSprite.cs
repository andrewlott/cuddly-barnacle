using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenSprite : MonoBehaviour {
	public SpriteRenderer SpriteRenderer;

	void Start () {
		if (SpriteRenderer == null) {
			SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		}

		ResizeSpriteRendererIfNecessary();
	}
	
	void Update () {
		ResizeSpriteRendererIfNecessary();
	}

	private void ResizeSpriteRendererIfNecessary() {
		Vector2 desiredSize = new Vector2(Screen.width / Utils.CameraScale / 100.0f, Screen.height / Utils.CameraScale / 100.0f);
		if (SpriteRenderer != null && SpriteRenderer.size != desiredSize) {
			SpriteRenderer.size = desiredSize;
		}
	}
}
