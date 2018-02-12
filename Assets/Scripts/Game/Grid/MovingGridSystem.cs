using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Issue can arrive if SpeedFactor is changed mid-game because old speedfactor + new one might not be evenly divisible by maxspeed

public class MovingGridSystem : BaseSystem {
	public static int SpeedFactor = 1;
	private static int _maxSpeed = 500;
	private static float _riseRate = GridComponent.YOffset() / _maxSpeed * SpeedFactor;

	private GameObject _gridEntity;
	private int _moves;

	public override void Start() {
		_gridEntity = new GameObject("Grid Entity");
		_gridEntity.AddComponent<GridComponent>();
	}

	public override void Stop() {

	}

	public override void Update() {
		_moves = (_moves + SpeedFactor) % _maxSpeed;
		if (SpeedFactor > 0 && _moves == 0) {
			GC.ShiftRows();
			GC.SetPositionsOffset(Vector3.zero);
		}

		Vector3 RiseVector = new Vector3(0.0f, _riseRate, 0.0f);
		GC.transform.position += RiseVector;

		GC.UpdatePositionsOffset(_riseRate);
	}

	public void MakeRandomBlock() {
		GC.AddBlock(GridComponent.GridWidth());
	}
}

