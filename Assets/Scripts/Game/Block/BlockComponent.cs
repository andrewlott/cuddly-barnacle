using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockComponent : BaseComponent {
	private static int _nextBlockId = 0;
	public int BlockId;

	public static void IncrementNextBlockId() {
		_nextBlockId++;
	}

	public static List<BlockComponent> BlockComponentsForId(int id) {
		List<BlockComponent> componentsWithId = new List<BlockComponent>();
		List<BaseComponent> allBlockComponents = Pool.Instance.ComponentsForType(typeof(BlockComponent));
		foreach (BlockComponent bc in allBlockComponents) {
			if (bc.BlockId == id) {
				componentsWithId.Add(bc);
			}
		}

		return componentsWithId;
	}

	public override void ComponentStart() {
		BlockId = _nextBlockId;
	}
}
