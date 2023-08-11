using Godot;

class Unit : IOccupant
{
	private Vector2I _cell = Vector2I.Zero;

	public Vector2I GetCell() { return _cell; }
	public void SetCell(Vector2I value) { _cell = value; }
	public int GetRange() { return 3; }
	public bool ReadyToMove() { return true; }
}

public partial class Example : Node2D
{
	private Board _gameboard = new();

	public override async void _Ready()
	{
		// Add units to a layer.
		BoardLayer unitLayer = new();
		CreateUnit(unitLayer, new Vector2I(1, 1));
		CreateUnit(unitLayer, new Vector2I(6, 6));

		// Add the unit layer to the board.
		if (_gameboard == null) { await ToSignal(_gameboard, "ready"); }
		_gameboard.AddLayer("units", unitLayer);
	}

	private void CreateUnit(BoardLayer layer, Vector2I cell)
	{
		Unit unit = new();
		unit.SetCell(cell);
		layer.Add(unit, cell);
	}
}