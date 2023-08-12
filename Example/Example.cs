using Godot;

enum ZOrder { Map, Highlight, Path, Unit }

partial class Unit : Node2D, IOccupant
{
	private readonly Grid _grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;
	private Vector2I _cell = Vector2I.Zero;
	private readonly Texture2D _texture;

	// Constructing a Unit sets its basic property values.
	public Unit(Vector2I cell, Texture2D texture)
	{
		_cell = cell;
		_texture = texture;
	}

	// When the Unit has been initialized, set its position and sprite.
	public override void _Ready()
	{
		Position = _grid.GridToScreen(GetCell());

		if (_texture == null) { return; }
		AddChild(new Sprite2D()
		{
			Texture = _texture,
			ZIndex = (int)ZOrder.Unit,
			Scale = new Vector2(0.5F, 0.5F)
		});
	}

	// Defining these methods implements the IOccupant interface.
	public Vector2I GetCell() { return _cell; }
	public int GetRange() { return 3; }
	public bool ReadyToMove() { return true; }

	// When the Unit finishes moving, update its location and position.
	public void OnMoved(Vector2I newCell)
	{
		_cell = newCell;
		Position = _grid.GridToScreen(newCell);
	}
}

public partial class Example : Node2D
{
	// Loading the Grid as a Resource gives us access to static helper methods.
	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	// UnitTexture is the 2D texture to apply to units.
	[Export]
	public Texture2D UnitTexture;

	// HighlightTiles are used to highlight walkable cells when a Unit is selected.
	// This tileset only needs to contain a single tile.
	[Export]
	public TileMap HighlightTiles;

	// PathTiles are used to draw an arrow from the Unit to the hovered cell.
	// PathTiles require a TerrainSet with a 3x3 "match corners and sides" bitmask.
	[Export]
	public TileMap PathTiles;

	private readonly Board _gameboard = new();
	private readonly BoardLayer _unitLayer = new();
	private Vector2I _hoveredCell = Vector2I.Zero;

	// _Ready sets up the board state with a single Unit.
	public override async void _Ready()
	{
		// Attach our tilesets to the BoardLayer.
		_unitLayer.HighlightTiles = HighlightTiles;
		_unitLayer.PathTiles = PathTiles;

		// Create a new Unit, and subscribe its OnMoved handler to the MoveFinished signal.
		Unit unit = new(new(3, 3), UnitTexture);
		_unitLayer.MoveFinished += unit.OnMoved;

		// Add the Unit to the BoardLayer.
		_unitLayer.Add(unit, new(3, 3));
		AddChild(unit);

		// Add the BoardLayer to the Board.
		if (_gameboard == null) { await ToSignal(_gameboard, "ready"); }
		_gameboard.AddLayer("units", _unitLayer);
	}

	// _Input handles mouse movement and mouse clicks.
	public override async void _Input(InputEvent @event)
	{
		if (_unitLayer == null) { await ToSignal(_unitLayer, "ready"); }

		// Handle mouse click.
		if (@event is InputEventMouseButton btn && btn.ButtonIndex == MouseButton.Left && btn.Pressed)
		{
			_unitLayer.HandleClick(Grid.ScreenToGrid(btn.Position));
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
