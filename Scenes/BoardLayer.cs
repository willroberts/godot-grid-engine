using Godot;
using Godot.Collections;

/*
    Tracks `Dictionary<Vector2I, Node2D>`, a mapping of grid cells to user-defined value types.
    Should this correspond with Tilemap layers?
	Signals: MoveFinished
    Functions: IsOccupied(cell), ResetBoard()
    Inhabitant Functions: Select, Deselect, Clear, Move, GetWalkableCells (FloodFill)
    Input Events: Deselect inhabitant on user-defined input. Provide a method for users to call.
*/
public partial class BoardLayer : Node2D
{
	// Maps grid coordinates to Node2D values.
	private Dictionary<Vector2I, Node2D> _contents;

	public override void _Ready() {}
}
