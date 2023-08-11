using Godot;
using System.Linq;

public partial class DebugGrid : Node2D
{
	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	[Export]
	public bool Isometric = false;

	public override async void _Ready()
	{
		if (Grid == null) { await ToSignal(Grid, "ready"); }

		GD.Print("[DEBUG] Grid size is ", Grid.Size);
		GD.Print("[DEBUG] Cell size is ", Grid.CellSize);
		TestGrid();

		// Draw the grid.
		if (Isometric)
		{
			DrawIsometric();
			return;
		}
		DrawOrthogonal();
	}

	public override void _Input(InputEvent @event)
	{
		if (
			@event is InputEventMouseButton btn &&
			btn.ButtonIndex == MouseButton.Left &&
			btn.Pressed
		)
		{
			GD.Print("[DEBUG] Player clicked on cell ", Grid.ScreenToGrid(btn.Position));
		}
	}

	private void DrawOrthogonal()
	{
		foreach (int x in Enumerable.Range(0, Grid.Size.X))
		{
			foreach (int y in Enumerable.Range(0, Grid.Size.Y))
			{
				// Draw cell outlines.
				ReferenceRect rect = new();
				rect.Position = Grid.GridToScreen(new(x, y)) - Grid.CellSize / 2;
				rect.Size = Grid.CellSize;
				rect.BorderColor = Colors.White;
				rect.EditorOnly = false;
				AddChild(rect);

				// Draw cell labels.
				if (Grid.CellSize < new Vector2I(40, 40)) { return; }
				Label label = new();
				label.Position = Grid.GridToScreen(new(x, y)) - Grid.CellSize / 2;
				label.Text = new Vector2I(x, y).ToString();
				AddChild(label);
			}
		}
	}

	private void DrawIsometric()
	{
		foreach (int x in Enumerable.Range(0, Grid.Size.X))
		{
			foreach (int y in Enumerable.Range(0, Grid.Size.Y))
			{
				// Draw cell outlines.
				ReferenceRect rect = new();
				Vector2 orthoPos = Grid.GridToScreen(new(x, y));
				rect.Position = OrthoDeltaToIso(orthoPos);
				rect.Size = Grid.CellSize;
				rect.RotationDegrees = 45.0F;
				rect.Scale = new Vector2(1.0F, 0.5F);
				rect.BorderColor = Colors.White;
				rect.EditorOnly = false;
				AddChild(rect);

			}
		}
	}

	public Vector2 IsoDeltaToOrtho(Vector2 orthoCoords)
	{
		return new Vector2(
			orthoCoords.X - orthoCoords.Y,
			(orthoCoords.X + orthoCoords.Y) / 2
		);
	}

	public Vector2 OrthoDeltaToIso(Vector2 isoCoords)
	{
		var foo = (isoCoords.X + isoCoords.Y * 2) / 2;
		return new Vector2(
			foo,
			foo - isoCoords.X
		);
	}

	private void TestGrid()
	{
		// Test IsWithinBounds().
		if (Grid.IsWithinBounds(Grid.Size))
		{
			GD.Print("[ERROR] Grid failed to detect out-of-bounds cell");
			return;
		}

		// Test Clamp().
		if (!Grid.IsWithinBounds(Grid.Clamp(Grid.Size)))
		{
			GD.Print("[ERROR] Failed to clamp out-of-bounds cell");
			return;
		}

		// Test ToIndex().
		if (Grid.ToIndex(new Vector2I(5, 5)) != 45)
		{
			GD.Print("[ERROR] Failed to convert coordinates to index");
			return;
		}

		GD.Print("[DEBUG] Grid tests passsed");
	}
}
