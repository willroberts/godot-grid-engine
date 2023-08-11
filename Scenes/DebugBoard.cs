using Godot;

public partial class DebugBoard : Node2D
{
	[Export]
	public Grid Grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;

	public override async void _Ready()
	{
		if (Grid == null) { await ToSignal(Grid, "ready"); }

		TestBoard();
	}

	private void TestBoard()
	{
		Board board = new();
		if (board.GetLayers().Count != 0)
		{
			GD.Print("[ERROR] Board initialized in a non-empty state");
			return;
		}

		BoardLayer layer = new();
		board.AddLayer("TestLayer", layer);
		if (board.GetLayers().Count != 1)
		{
			GD.Print("[ERROR] Failed to add layer to board");
			return;
		}

		GD.Print("[DEBUG] Board tests passed");
	}
}
