using Godot;
using Godot.Collections;

// The Board class represents layers of occupants on the grid.
public partial class Board : Node2D
{
	private Dictionary<string, BoardLayer> _layers;

	public void AddLayer(string layerName, BoardLayer layer)
	{
		_layers.Add(layerName, layer);
	}
}
