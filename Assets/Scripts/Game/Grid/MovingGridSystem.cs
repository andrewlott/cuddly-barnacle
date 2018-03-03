using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGridSystem : BaseSystem {
	private GameObject _gridEntity;
	private GameObject _gridEntity2;

	private long _steps = 0;

	public override void Start() {
		_gridEntity = new GameObject("Grid Entity");
		_gridEntity.AddComponent<GridComponent>();
//		_gridEntity2 = new GameObject("Grid Entity 2");
//		_gridEntity2.transform.position = _gridEntity.transform.position + new Vector3(1.0f, 0.0f, 0.0f);
//		_gridEntity2.AddComponent<GridComponent>();
	}

	public override void Stop() {

	}

	public override void Update() {
		List<BaseComponent> gridComponents = Pool.Instance.ComponentsForType(typeof(GridComponent));
		foreach (GridComponent gc in gridComponents) {
			gc.transform.position = NewPosition(gc);
			gc.IncrementShiftCounter();
		}
		_steps += GridComponent.StepSize();
	}

	private Vector3 NewPosition(GridComponent gc) {
		return gc.InitialPosition + new Vector3(0.0f, _steps * GridComponent.StepPixelSize(), 0.0f);
	}

	public void MakeRandomBlock() {
		GridComponent gc = Pool.Instance.ComponentForType(typeof(GridComponent)) as GridComponent;
		gc.AddBlock(GridComponent.GridWidth());
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is GridComponent) {

		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is GridComponent) {

		}
	}
}

