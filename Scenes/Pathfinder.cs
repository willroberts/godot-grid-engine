using Godot;
using Godot.Collections;

// The Pathfinder class provides methods for shortest path computation via A*.
public partial class Pathfinder : RefCounted
{
    public readonly Vector2I[] Directions = {
        Vector2I.Left,
        Vector2I.Right,
        Vector2I.Up,
        Vector2I.Down
    };

    private readonly Grid _grid;
    private readonly AStar2D _aStar = new();

    public Pathfinder(Grid grid, Array<Vector2I> cells)
    {
        // Populate cell mappings.
        _grid = grid;
        Dictionary<Vector2I, int> cellIndices = new();
        foreach (Vector2I cell in cells)
        {
            cellIndices[cell] = _grid.ToIndex(cell);
        }

        // Initialize A* pathfinder.
        foreach (Vector2I cell in cellIndices.Keys)
        {
            _aStar.AddPoint(cellIndices[cell], cell);
        }
        foreach (Vector2I cell in cellIndices.Keys)
        {
            foreach (int neighbor in FindNeighbors(cell, cellIndices))
            {
                _aStar.ConnectPoints(cellIndices[cell], neighbor);
            }
        }
    }

    public Array<Vector2I> GetPointPath(Vector2I start, Vector2I end)
    {
        int startIndex = _grid.ToIndex(start);
        int endIndex = _grid.ToIndex(end);

        if (_aStar.HasPoint(startIndex) && _aStar.HasPoint(endIndex))
        {
            return UnpackArray(_aStar.GetPointPath(startIndex, endIndex));
        }

        GD.Print("[ERROR] Pathfinder failed to get A* point path");
        return new Array<Vector2I>();
    }

    private Array<int> FindNeighbors(Vector2I cell, Dictionary<Vector2I, int> cellIndices)
    {
        Array<int> result = new();
        foreach (Vector2I direction in Directions)
        {
            Vector2I neighbor = cell + direction;
            if (!cellIndices.ContainsKey(neighbor)) { continue; }

            if (!_aStar.ArePointsConnected(cellIndices[cell], cellIndices[neighbor]))
            {
                result.Add(cellIndices[neighbor]);
            }
        }
        return result;
    }

    // Convenience function for converting `Vector2[]` to `Array<Vector2I>`.
    // Has similar performance to returning `new Array<Vector2>(input)`, while
    // also converting inner values from `float` to `int`.
    // Useful when dealing with A*, which returns Vector2 values.
    private Array<Vector2I> UnpackArray(Vector2[] input)
    {
        Array<Vector2I> converted = new();
        foreach (Vector2 value in input)
        {
            converted.Add(new Vector2I((int)value.X, (int)value.Y));
        }
        return converted;
    }
}
