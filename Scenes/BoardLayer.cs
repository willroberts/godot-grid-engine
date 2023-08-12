using Godot;
using Godot.Collections;

public interface IOccupant
{
	// GetCell should return the Occupant's current position. This is used when
	// moving Occupants, or when pathfinding for an Occupant.
	Vector2I GetCell();

	// GetRange determines how far the Occupant can move by restricting pathfinding.
	int GetRange();

	// ReadyToMove provides an opportunity to prevent an Occupant's movement.
	bool ReadyToMove();
}

// The BoardLayer class controls the occupants for a single layer in the grid.
// Occupants use the `IOccupant` interface.
public partial class BoardLayer : Node2D
{
	[Signal]
	public delegate void MoveFinishedEventHandler(Vector2I newCell);

	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	[Export]
	public TileMap HighlightTiles;

	[Export]
	public TileMap PathTiles;

	private readonly Vector2I[] Directions = {
		Vector2I.Left,
		Vector2I.Right,
		Vector2I.Up,
		Vector2I.Down
	};

	private readonly System.Collections.Generic.Dictionary<Vector2I, IOccupant> _cellContents = new();
	private IOccupant _selection = null;
	private Array<Vector2I> _highlightCells = new();
	private Pathfinder _pathfinder = null;
	private Array<Vector2I> _currentPath = new();

	public void HandleHover(Vector2I newCell)
	{
		if (_selection == null) { return; }

		DrawPath(_selection.GetCell(), newCell);
	}

	public void HandleClick(Vector2I cell)
	{
		if (_selection == null)
		{
			Select(cell);
			return;
		}

		if (_selection.ReadyToMove()) { MoveSelection(cell); }
	}

	public void HandleCancel()
	{
		ClearHighlight();
		ClearPath();
		ClearSelection();
	}

	/*
	* Occupant methods
	*/

	public bool IsOccupied(Vector2I cell)
	{
		return _cellContents.ContainsKey(cell);
	}

	public void Add(IOccupant occupant, Vector2I cell)
	{
		if (IsOccupied(cell))
		{
			GD.Print("Error: Cell already occupied");
			return;
		}

		_cellContents[cell] = occupant;
	}

	public void Select(Vector2I cell)
	{
		if (!_cellContents.ContainsKey(cell)) { return; }

		_selection = _cellContents[cell];
		_highlightCells = ComputeHighlight(cell, _selection.GetRange());
		DrawHighlight(_highlightCells);
		ComputePath(_highlightCells);
	}

	public void MoveSelection(Vector2I newCell)
	{
		if (IsOccupied(newCell) || !_highlightCells.Contains(newCell)) { return; }

		_cellContents.Remove(_selection.GetCell());
		_cellContents[newCell] = _selection;

		ClearHighlight();
		ClearPath();
		ClearSelection();

		EmitSignal("MoveFinished", newCell);
	}

	public void ClearSelection()
	{
		_highlightCells.Clear();
		_selection = null;
	}

	/*
	* Highlight methods show connected cells within a certain range.
	*/

	// ComputeHighlight implements a flood fill algorithm for the grid, using
	// the given cell and range.
	public Array<Vector2I> ComputeHighlight(Vector2I cell, int range)
	{
		Array<Vector2I> result = new();
		System.Collections.Generic.Stack<Vector2I> stack = new();
		stack.Push(cell);

		while (stack.Count != 0)
		{
			Vector2I currentCell = stack.Pop();
			if (!Grid.IsWithinBounds(currentCell)) { continue; }
			if (result.Contains(currentCell)) { continue; }

			Vector2I difference = (currentCell - cell).Abs();
			int distance = difference.X + difference.Y;
			if (distance > range) { continue; }

			result.Add(currentCell);
			foreach (Vector2I direction in Directions)
			{
				Vector2I coords = currentCell + direction;
				if (IsOccupied(coords)) { continue; }
				if (result.Contains(coords)) { continue; }
				stack.Push(coords);
			}
		}

		return result;
	}

	public void DrawHighlight(Array<Vector2I> cells)
	{
		if (HighlightTiles == null)
		{
			GD.Print("HighlightTiles are null; skipping highlighting");
			return;
		}

		ClearHighlight();
		foreach (Vector2I cell in cells)
		{
			HighlightTiles.SetCell(0, cell, 0, Vector2I.Zero, 0);
		}
	}

	public void ClearHighlight()
	{
		if (HighlightTiles == null) { return; }
		HighlightTiles.Clear();
	}

	/*
	* Path mehthods show the shortest path between two cells.
	*/

	public void ComputePath(Array<Vector2I> cells)
	{
		_pathfinder = new Pathfinder(Grid, cells);
	}

	public void DrawPath(Vector2I start, Vector2I end)
	{
		if (PathTiles == null)
		{
			GD.Print("PathTiles are null; skipping path drawing");
			return;
		}

		PathTiles.Clear();
		_currentPath = _pathfinder.GetPointPath(start, end);

		if (_currentPath.Count == 0)
		{
			GD.Print("Error: Pathfinder returned empty path");
			return;
		}

		if (PathTiles == null) { return; }
		foreach (Vector2I cell in _currentPath)
		{
			PathTiles.SetCell(0, cell, 0, Vector2I.Zero, 0);
		}
		PathTiles.SetCellsTerrainConnect(0, _currentPath, 0, 0);
	}

	public void ClearPath()
	{
		_pathfinder = null;
		if (PathTiles != null) { PathTiles.Clear(); }
	}
}
