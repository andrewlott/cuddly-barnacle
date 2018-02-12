using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridComponent : BaseComponent {
	private static float _tileWidth = 0.12f;
	private static int _bottomBuffer = 2;
	private static int _topBuffer = 2;
	private static int _width = 6;
	private static int _height = 9 + _bottomBuffer + _topBuffer;
	private static float _xOffset = _tileWidth;
	private static float _yOffset = _tileWidth;
	private static Vector3 _boardCenter = new Vector3(0.0f, -0.4f, 0.0f);

	private Vector3 _positionOffset = Vector3.zero;

	private GameObject[,] _grid = new GameObject[_height, _width];

	public override void ComponentStart() {
		for (int i = 0; i < _grid.GetLength(0); i++) {
			for (int j = 0; j < _grid.GetLength(1); j++) {
				bool shouldColor = i < _bottomBuffer;
				AddTileForPosition(_grid, i, j, shouldColor);
			}
		}
	}

	public void ShiftRows() {
		GameObject[,] newGrid = new GameObject[_height, _width];

		// Destroy gameobjects in top rows of grid
		for (int i = _height - _topBuffer + 1; i < _grid.GetLength(0); i++) {
			for (int j = 0; j < _grid.GetLength(1); j++) {
				GameObject g = _grid[i, j];
				if (g != null) {
					Utils.DestroyEntity(g);
				}
			}
		}
		// Copy elements of grid to newgrid, shifted up one row
		for (int i = _grid.GetLength(0) - 1; i > 0; i--) {
			for (int j = 0; j < _grid.GetLength(1); j++) {
				newGrid[i, j] = _grid[i - 1, j];
			}
		}
		// Add new game objects for bottom row
		for (int i = 0; i < _grid.GetLength(1); i++) {
			AddTileForPosition(newGrid, 0, i, true);
		}

		_grid = newGrid;
	}

	public void AddTileForPosition(GameObject [,] grid, int i, int j, bool shouldColor = false, bool shouldDestroyExisting = true) {
		Vector3 pos = PositionForIndex(i, j);
		GameController gameController = GameObject.FindObjectOfType<GameController>();
		GameObject g = Instantiate(gameController.GridPrefab);
		g.transform.position = pos;
		g.transform.SetParent(gameObject.transform);
		if (grid[i, j] != null && shouldDestroyExisting) {
			Utils.DestroyEntity(grid[i, j]);
		}
		grid[i, j] = g;

		ColorComponent cc = g.GetComponent<ColorComponent>();
		cc.shouldRandomize = shouldColor;
	}

	public void AddBlock(int width) {
		width = 2;
		int randomDeviation = Utils.RandomInt(_width - width);
		int randomCol = 2; // todo: figure this formula out

		GameController gameController = GameObject.FindObjectOfType<GameController>();
		for (int i = 0; i < width; i++) {
			GameObject g = Instantiate(gameController.BlockPrefab);
			Vector3 pos = PositionForIndex(_height - _topBuffer, randomCol - i);
			g.transform.position = pos;
			g.transform.SetParent(gameObject.transform);
			if (_grid[_height - _topBuffer, randomCol - i] != null) {
				Utils.DestroyEntity(_grid[_height - _topBuffer, randomCol - i]);
			}
			_grid[_height - _topBuffer, randomCol - i] = g;
		}

		BlockComponent.IncrementNextBlockId(); // problematic to automate some of this
	}

	public void ReplaceBlockWithRandomColoredTile(BlockComponent bc) {
		Vector2Int pos = PositionInGrid(bc.gameObject);
		AddTileForPosition(_grid, pos.x, pos.y, true, false);
	}

	public static float YOffset() {
		return _yOffset;
	}

	public static int GridWidth() {
		return _width;
	}

	public static int GridActiveHeight() {
		return _height - _topBuffer - _bottomBuffer;
	}

	public List<GameObject> GridAsList() {
		return _grid.Cast<GameObject>().ToList();
	}

	public List<GameObject> AllObjectsAbove(GameObject g) {
		Vector2Int pos = PositionInGrid(g);
		List<GameObject> objectsAbove = new List<GameObject>();
		while (pos.x + 1 < _height && _grid[pos.x + 1, pos.y] != null) {
			pos.x += 1;
			objectsAbove.Add(_grid[pos.x, pos.y]);
		}

		return objectsAbove;
	}

	public bool CanBlockFall(GameObject blockGameObject) {
		BlockComponent block = blockGameObject.GetComponent<BlockComponent>();
		if (block == null) {
			return false;
		}
		List<Vector2Int> blockPositions = new List<Vector2Int>();
		List<BlockComponent> blocksWithMatchingId = BlockComponent.BlockComponentsForId(block.BlockId);
		foreach (BlockComponent bc in blocksWithMatchingId) {
			blockPositions.Add(PositionInGrid(bc.gameObject));
		}
		int cols = 0;
		foreach (Vector2Int p in blockPositions) {
			Vector2Int pos = new Vector2Int(p.x, p.y);
			while (pos.x - 1 >= 0 && _grid[pos.x - 1, pos.y] != null) {
				pos.x -= 1;
				ColorComponent cc = _grid[pos.x, pos.y].GetComponent<ColorComponent>();
				SwappingComponent sc = _grid[pos.x, pos.y].GetComponent<SwappingComponent>();
				if (cc.color == ColorType.None && sc == null) {
					cols += 1;
					break;
				}
			}
		}

		return cols == blockPositions.Count;
	}

	public GameObject TopNeighbor(GameObject g) {
		Vector2Int pos = PositionInGrid(g);
		pos.x += 1;
		if (pos.x < _height && !IsHidden(pos)) {
			return _grid[pos.x, pos.y];
		}

		return null;
	}

	public GameObject BottomNeighbor(GameObject g) {
		Vector2Int pos = PositionInGrid(g);
		pos.x -= 1;
		if (pos.x >= 0 && !IsHidden(pos)) {
			return _grid[pos.x, pos.y];
		}

		return null;
	}

	public GameObject LeftNeighbor(GameObject g) {
		Vector2Int pos = PositionInGrid(g);
		pos.y -= 1;
		if (pos.y >= 0 && !IsHidden(pos)) {
			return _grid[pos.x, pos.y];
		}

		return null;
	}

	public GameObject RightNeighbor(GameObject g) {
		Vector2Int pos = PositionInGrid(g);
		pos.y += 1;
		if (pos.y < _width && !IsHidden(pos)) {
			return _grid[pos.x, pos.y];
		}

		return null;
	}

	public bool AreNeighbors(GameObject g1, GameObject g2) {
		Vector2Int p1 = PositionInGrid(g1);
		Vector2Int p2 = PositionInGrid(g2);
		return Mathf.Abs((p1 - p2).magnitude) == 1;
	}

	public bool AreHorizontalNeighbors(GameObject g1, GameObject g2) {
		Vector2Int p1 = PositionInGrid(g1);
		Vector2Int p2 = PositionInGrid(g2);
		return Mathf.Abs(p1.y - p2.y) == 1 && p1.x - p2.x == 0;
	}

	public Vector2Int PositionInGrid(GameObject g) {
		for (int i = 0; i < _grid.GetLength(0); i++) {
			for (int j = 0; j < _grid.GetLength(1); j++) {
				if (_grid[i, j] == g) {
					return new Vector2Int(i, j);
				}
			}
		}	
		return Vector2Int.zero - Vector2Int.one;
	}

	public void Swap(GameObject g1, GameObject g2) {
		Swap(PositionInGrid(g1), PositionInGrid(g2));
	}

	public void Swap(Vector2Int p1, Vector2Int p2) {
		GameObject g1 = _grid[p1.x, p1.y];
		GameObject g2 = _grid[p2.x, p2.y];
		_grid[p1.x, p1.y] = g2;
		_grid[p2.x, p2.y] = g1;
	}

	public GameObject ObjectAtIndex(Vector2Int pos) {
		return _grid[pos.x, pos.y];
	}

	public Vector3 PositionForIndex(int y, int x) {
		return _boardCenter + new Vector3((x - _grid.GetLength(1) / 2.0f + 0.5f) * _xOffset, 
			               (y - _grid.GetLength(0) / 2.0f + 0.5f) * _yOffset, 
			                0) 
			+ _positionOffset;
	}

	public void UpdatePositionsOffset(float riseRate) {
//		_positionOffset += new Vector3(0.0f, riseRate, 0.0f);
	}

	public void SetPositionsOffset(Vector3 newOffset) {
		_positionOffset = newOffset;
	}

	public bool IsHidden(GameObject g) {
		Vector2Int pos = PositionInGrid(g);
		return IsHidden(pos);
	}

	public bool IsHidden(Vector2Int pos) {
		return pos.x < _bottomBuffer || pos.x >= _height - _topBuffer;
	}

	public int ActiveHeight() {
		for (int i = _grid.GetLength(0) - _topBuffer - 1; i >= _bottomBuffer; i--) {
			for (int j = 0; j < _grid.GetLength(1); j++) {
				ColorComponent cc = _grid[i, j].GetComponent<ColorComponent>();
				if (cc.color != ColorType.None) {
					return i;
				}
			}
		}	

		return 0;
	}
}
