using Godot;
using System.Linq;

[Tool]
public partial class DebugGrid : Node2D
{
	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	[Export]
	public bool Isometric = false;

	public override void _Ready() {
		GD.Print("[DEBUG] Grid size is ", Grid.Size);
		GD.Print("[DEBUG] Cell size is ", Grid.CellSize);
		if (Isometric)
		{
			DrawIsometric();
			return;
		}
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
		foreach (int x in Enumerable.Range(0, Grid.Size.X))
		{
			foreach (int y in Enumerable.Range(0, Grid.Size.Y))
			{
				// Draw cell outlines.
				ReferenceRect rect = new();

				Vector2 orthoPos = Grid.GridToScreen(new(x, y));
				GD.Print("Ortho position: ", orthoPos);
				rect.Position = Grid.OrthoDeltaToIso(orthoPos);
				GD.Print("Iso position: ", rect.Position);

				rect.Size = Grid.CellSize;
				rect.RotationDegrees = 45.0F;
				rect.Scale = new Vector2(1.0F, 0.5F);
				rect.BorderColor = Colors.White;
				rect.EditorOnly = false;
				AddChild(rect);
				
			}
		}
	}
}
