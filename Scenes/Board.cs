using Godot;
using Godot.Collections;

// The Board class represents layers of inhabitants on the grid.
public partial class Board : Node2D
{
	private Grid _grid = ResourceLoader.Load("res://Resources/Grid.tres") as Grid;
	private Dictionary<string, BoardLayer> _layers;

	public void AddLayer(string layerName, BoardLayer layer)
	{
		_layers.Add(layerName, layer);
	}
}
