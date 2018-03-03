using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class BlockSystem : BaseSystem {

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
	}

	public override void Update() {
		
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is MatchComponent) {
			MatchComponent mc = c as MatchComponent;
			List<BaseComponent> blockComponents = Pool.Instance.ComponentsForType(typeof(BlockComponent));
			List<int> matchedBlockIds = new List<int>();

			GridComponent gc = null;
			foreach (GameObject matchedObject in mc.matchedObjects) {
				TileComponent tc = matchedObject.GetComponent<TileComponent>();
				gc = tc.ParentGrid;
				foreach (BlockComponent bc in blockComponents) {
					if (gc.AreNeighbors(matchedObject, bc.gameObject) && !matchedBlockIds.Contains(bc.BlockId)) {
						matchedBlockIds.Add(bc.BlockId);
					}
				}
			}

			MatchBlocks(matchedBlockIds, gc:gc);
		}
	}

	private void MatchBlocks(List<int> matchedBlockIds, GridComponent gc) {
		foreach (int blockId in matchedBlockIds) {
			List<BlockComponent> blocks = BlockComponent.BlockComponentsForId(blockId);
			foreach (BlockComponent block in blocks) {
				AnimationComponent ac = block.gameObject.AddComponent<AnimationComponent>();
				ac.Trigger = "Clear";
				ac.Callback = OnBlockClearAnimationComplete;
				ac.CallbackState = "Empty";

				gc.ReplaceBlockWithRandomColoredTile(block);
			}
		}
	}

	public void OnBlockClearAnimationComplete(GameObject g) {
		Utils.DestroyEntity(g);
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is MatchComponent) {

		}
	}
}
