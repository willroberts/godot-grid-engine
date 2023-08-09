using Godot;
using Godot.Collections;

// The Board class represents layers of inhabitants on the grid.
// Replaces: GameBoard, UnitOverlay, UnitPath.
public partial class Board : Node2D
{
	[Export]
	public TileMap HighlightTiles;

	[Export]
	public TileMap PathTiles;

	private Grid _grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	// The Board can have multiple layers. The outer dictionary is keyed by
	// a user provided name. The value is an inner dictionary, which maps grid
	// coordinates to Node2D values.
	private Dictionary<string, BoardLayer> _layers;

	public override void _Ready() {}
	public void AddLayer(string layerName, BoardLayer layer) {}
	public void DrawHighlight() {}
	public void DrawPath() {}
}
