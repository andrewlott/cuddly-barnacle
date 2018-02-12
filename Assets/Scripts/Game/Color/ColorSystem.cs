using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSystem : BaseSystem {

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(ColorComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(ColorComponent), this);
	}

	public override void Update() {
		List<BaseComponent> colorComponents = Pool.Instance.ComponentsForType(typeof(ColorComponent));
		foreach (ColorComponent cc in colorComponents) {
			SpriteRenderer sr = cc.gameObject.GetComponent<SpriteRenderer>();


			if (cc.color != ColorType.None && GC.IsHidden(cc.gameObject)) {
				sr.color = Color.grey;
			} else {
				sr.color = Color.white;
			}
		}
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is ColorComponent) {
			ColorComponent cc = c as ColorComponent;
			if (cc.shouldRandomize) {
				cc.color = Utils.NextRandomColor();
			}

			Animator ccAnimator = cc.GetComponent<Animator>();
			ccAnimator.SetTrigger(Utils.ColorTypeAsString(cc.color));


//			GridComponent gc = Pool.Instance.ComponentForType(typeof(GridComponent)) as GridComponent;
//			ColorComponent cc = c as ColorComponent;
//			// temp
////			if (cc.color == ColorType.None && cc.shouldRandomize) {
////				List<ColorType> shuffledColors = new List<ColorType>{ ColorType.Blue, ColorType.Green, ColorType.Red, ColorType.Teal, ColorType.Yellow };
////				if (!gc.IsHidden(cc.gameObject)) {
////					shuffledColors.Add(ColorType.None);
////				}
////				shuffledColors.Shuffle();
////				cc.color = shuffledColors[0];
////			}
//			SpriteRenderer sr = cc.gameObject.GetComponent<SpriteRenderer>();
//			sr.color = ColorForColorType(cc.color);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is ColorComponent) {
		}
	}

	private Color ColorForColorType(ColorType ct) {
		switch (ct) {
		case ColorType.Blue:
			return Color.blue;
		case ColorType.Green:
			return Color.green;
		case ColorType.Red:
			return Color.red;
		case ColorType.Teal:
			return Color.cyan;
		case ColorType.Yellow:
			return Color.yellow;
		case ColorType.None:
			return Color.clear;
		case ColorType.Block:
			return Color.grey;
		default:
			return Color.white;
		}
	}
}
