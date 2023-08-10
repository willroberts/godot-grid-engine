using Godot;
using Godot.Collections;
using System.Linq;

public partial class DebugPathfinder : Node2D
{
	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	public override async void _Ready()
	{
		if (Grid == null) { await ToSignal(Grid, "ready"); }

		TestPathfinder();
	}

	private void TestPathfinder()
	{
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

		Pathfinder pf = new Pathfinder(Grid, cells);
		Array<Vector2I> path = pf.GetPointPath(Vector2I.Zero, new Vector2I(3, 3));
		Array<Vector2I> expected = new()
		{
			Vector2I.Zero,
			new Vector2I(1, 0),
			new Vector2I(1, 1),
			new Vector2I(1, 2),
			new Vector2I(2, 2),
			new Vector2I(2, 3),
			new Vector2I(3, 3)
		};

		if (path.Count != expected.Count)
		{
			GD.Print("[ERROR] Pathfinder returned unexpected path");
			GD.Print("[ERROR] Got: ", path);
			GD.Print("[ERROR] Expected: ", expected);
			return;
		}

		foreach (int i in Enumerable.Range(0, path.Count))
		{
			if (path[i] != expected[i])
			{
				GD.Print("[ERROR] Pathfinder returned unexpected path");
				GD.Print("[ERROR] Got: ", path);
				GD.Print("[ERROR] Expected: ", expected);
				return;
			}
		}

		GD.Print("[DEBUG] Pathfinder tests passed");
	}
}
