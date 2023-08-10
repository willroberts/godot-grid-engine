using Godot;
using Godot.Collections;

// The Board class represents layers of occupants on the grid.
// TODO: Consider tight integration with Godot 4 tilemap layers.
public partial class Board : Node2D
{
	private readonly Dictionary<string, BoardLayer> _layers = new();

	public Dictionary<string, BoardLayer> GetLayers()
	{
		return _layers;
	}

	public void AddLayer(string layerName, BoardLayer layer)
	{
		_layers.Add(layerName, layer);
	}
}
