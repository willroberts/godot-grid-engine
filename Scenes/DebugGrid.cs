using Godot;
using System.Linq;

public partial class DebugGrid : Node2D
{
	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	public override void _Ready() {
		GD.Print("[DEBUG] Grid size is ", Grid.Size);
		GD.Print("[DEBUG] Cell size is ", Grid.CellSize);
		DrawOrthogonal();
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
				if (Grid.CellSize < new Vector2I(40, 40)) { return ; }
				Label label = new();
				label.Position = Grid.GridToScreen(new(x, y)) - Grid.CellSize / 2;
				label.Text = new Vector2I(x, y).ToString();
				AddChild(label);
			}
		}
	}

	private void DrawIsometric()
	{
		// TBD.
	}
}
