using Godot;
using Godot.Collections;

class TestOccupant : IOccupant
{
	public Vector2I GetCell() { return new(3, 3); }
	public int GetRange() { return 3; }
	public bool ReadyToMove() { return true; }
}

public partial class DebugBoardLayer : Node2D
{
	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	public override async void _Ready()
	{
		if (Grid == null) { await ToSignal(Grid, "ready"); }

		TestBoardLayer();
	}

	private void TestBoardLayer()
	{
		BoardLayer layer = new()
		{
			HighlightTiles = new TileMap(),
			PathTiles = new TileMap()
		};

		// Test occupant methods.
		TestOccupant occ = new();
		layer.Add(occ, occ.GetCell());
		if (!layer.IsOccupied(occ.GetCell()))
		{
			GD.Print("[ERROR] Failed to mark cell as occupied");
			return;
		}
		layer.Select(occ.GetCell());
		layer.ClearSelection();
		layer.Select(occ.GetCell());
		layer.MoveSelection(new Vector2I(2, 2));
		if (!layer.IsOccupied(new Vector2I(2, 2)))
		{
			GD.Print("[ERROR] Failed to move occupant");
			return;
		}

		// Test highlight methods.
		// TODO: Add ComputeHighlight() validation for result.
		Array<Vector2I> result = layer.ComputeHighlight(Vector2I.Zero, 2);
		layer.DrawHighlight(result);
		layer.ClearHighlight();

		// Test path methods.
		// TODO: Add DrawPath() validation for PathTiles.
		Array<Vector2I> cells = new()
		{
			Vector2I.Zero,
			new Vector2I(0, 1),
			new Vector2I(0, 2),
			new Vector2I(0, 3),
			new Vector2I(1, 0),
			new Vector2I(1, 1),
			new Vector2I(1, 2),
			new Vector2I(1, 3),
			new Vector2I(2, 0),
			new Vector2I(2, 1),
			new Vector2I(2, 2),
			new Vector2I(2, 3),
			new Vector2I(3, 0),
			new Vector2I(3, 1),
			new Vector2I(3, 2),
			new Vector2I(3, 3)
		};
		layer.ComputePath(cells);
		layer.DrawPath(Vector2I.Zero, new Vector2I(3, 3));
		layer.ClearPath();

		GD.Print("[DEBUG] BoardLayer tests passed");
	}
}
