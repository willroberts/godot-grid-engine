using Godot;
using Godot.Collections;

enum ZOrder { Map, Highlight, Path, Unit }

partial class Unit : Node2D, IOccupant
{
	private readonly Grid _grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;
	private Vector2I _cell = Vector2I.Zero;
	private readonly Texture2D _texture;

	public Unit(Vector2I cell, Texture2D texture)
	{
		_cell = cell;
		_texture = texture;
	}

	public override void _Ready()
	{
		Position = _grid.GridToScreen(GetCell());

		if (_texture != null)
		{
			Sprite2D sprite = new()
			{
				Texture = _texture,
				ZIndex = (int)ZOrder.Unit,
				Scale = new Vector2(0.5F, 0.5F)
			};
			AddChild(sprite);
		}
	}

	public Vector2I GetCell() { return _cell; }
	public int GetRange() { return 3; }
	public bool ReadyToMove() { return true; }

	public void OnMoved(Vector2I newCell)
	{
		_cell = newCell;
		Position = _grid.GridToScreen(newCell);
	}
}

public partial class Example : Node2D
{
	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	[Export]
	public Texture2D UnitTexture;

	[Export]
	public TileMap HighlightTiles;

	[Export]
	public TileMap PathTiles;

	private readonly Board _gameboard = new();
	private readonly BoardLayer _unitLayer = new();
	private Vector2I _hoveredCell = Vector2I.Zero;

	public override async void _Ready()
	{
		// Add a unit to a layer.
		Vector2I position = new(3, 3);
		Unit unit = new(position, UnitTexture);
		_unitLayer.HighlightTiles = HighlightTiles;
		_unitLayer.PathTiles = PathTiles;
		_unitLayer.Add(unit, position);
		_unitLayer.MoveFinished += unit.OnMoved;
		AddChild(unit);

		// Add the unit layer to the board.
		if (_gameboard == null) { await ToSignal(_gameboard, "ready"); }
		_gameboard.AddLayer("units", _unitLayer);
	}

	public override void _Input(InputEvent @event)
	{
		// Handle mouse click.
		if (
			_unitLayer != null &&
			@event is InputEventMouseButton btn &&
			btn.ButtonIndex == MouseButton.Left &&
			btn.Pressed
		)
		{
			Vector2I cell = Grid.ScreenToGrid(btn.Position);
			_unitLayer.HandleClick(cell);
			GetViewport().SetInputAsHandled();
			return;
		}

		// Handle mouse motion.
		if (@event is InputEventMouseMotion evt)
		{
			Vector2I hoveredCell = Grid.Clamp(Grid.ScreenToGrid(evt.Position));
			if (hoveredCell.Equals(_hoveredCell)) { return; }
			_hoveredCell = hoveredCell;
			_unitLayer.HandleHover(_hoveredCell);
		}
	}
}
