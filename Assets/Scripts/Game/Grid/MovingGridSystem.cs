using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGridSystem : BaseSystem {
	private GameObject _gridEntity;

	private Vector3 _initialPosition;
	private long _steps = 0;

	public override void Start() {
		_gridEntity = new GameObject("Grid Entity");
		_gridEntity.AddComponent<GridComponent>();
//		GameObject g = new GameObject("Grid Entity 2");
//		g.transform.position = _gridEntity.transform.position + new Vector3(1.0f, 0.0f, 0.0f);
//		BaseObject.AddComponent<GridComponent>();
		_initialPosition = _gridEntity.transform.position;
	}

	public override void Stop() {

	}

	public override void Update() {
		GC.transform.position = NewPosition();
		_steps += GridComponent.StepSize();
		GC.IncrementShiftCounter();
	}

	private Vector3 NewPosition() {
		return _initialPosition + (new Vector3(0.0f, _steps * GridComponent.StepPixelSize(), 0.0f));
	}

	public void MakeRandomBlock() {
		GC.AddBlock(GridComponent.GridWidth());
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is SwappingComponent) {

		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is SwappingComponent) {

		}
	}
}

