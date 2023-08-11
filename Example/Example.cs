using Godot;

partial class Unit : Node2D, IOccupant
{
	private readonly Grid _grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;
	private Vector2I _cell = Vector2I.Zero;
	private readonly Texture2D _texture;

	public Unit(Vector2I cell, Texture2D texture) {
		_cell = cell;
		_texture = texture;
	}

    public override void _Ready()
    {
		Position = _grid.GridToScreen(GetCell());
		GD.Print("Position: ", Position);

        ReferenceRect rect = new()
        {
            Position = Position,
            Size = new Vector2(64, 64),
            BorderColor = Colors.Red,
			ZIndex = 1
        };
        AddChild(rect);

		if (_texture != null) {
			Sprite2D sprite = new(){
				Texture = _texture,
				ZIndex = 1,
				Scale = new Vector2(0.5F, 0.5F)
			};
			AddChild(sprite);
		}
    }

    public Vector2I GetCell() { return _cell; }
	public int GetRange() { return 3; }
	public bool ReadyToMove() { return true; }
}

public partial class Example : Node2D
{
	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	[Export]
	public Texture2D UnitTexture;

	private readonly Board _gameboard = new();
	private readonly BoardLayer _unitLayer = new();

	public override async void _Ready()
	{
		// Add a unit to a layer.
		Vector2I position = new(3, 3);
		Unit unit = new(position, UnitTexture);
		AddChild(unit);
		_unitLayer.Add(unit, position);

		// Add the unit layer to the board.
		if (_gameboard == null) { await ToSignal(_gameboard, "ready"); }
		_gameboard.AddLayer("units", _unitLayer);
	}

	public override void _Input(InputEvent @event)
	{
		if (
			@event is InputEventMouseButton btn &&
			btn.ButtonIndex == MouseButton.Left &&
			btn.Pressed
		) {
			Vector2I cell = Grid.ScreenToGrid(btn.Position);
			GD.Print("Selecting cell ", cell);
			_unitLayer.HandleClick(cell);
			GetViewport().SetInputAsHandled();
		}
	}
}