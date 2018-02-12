﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSystem {
	public GameObject BaseObject;
	private static GridComponent _GC;

	public virtual void Start() {

	}

	public virtual void Stop() {

	}

	public virtual void Update() {

	}

	public virtual void OnComponentAdded(BaseComponent c) {

	}

	public virtual void OnComponentRemoved(BaseComponent c) {

	}

	protected BaseController Controller() {
		return BaseObject.GetComponent<BaseController>();
	}

	protected GridComponent GC {
		get {
			if (_GC == null) {
				_GC = Pool.Instance.ComponentForType(typeof(GridComponent)) as GridComponent;
			}

			return _GC;
		}
	}
}