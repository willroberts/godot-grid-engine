# Godot Grid Engine

A Godot library for games with grid-based movement.

Uses Godot 4 and C#.

## Library Design

This library provides three public types: `Grid`, `Board`, and `BoardLayer`.

[`Grid`](Resources/Grid.cs) is a static, data-only `Resource` type which
contains the basic representation of a 2D grid. It is loaded by the other
types.

[`Board`](Scenes/Board.cs) is a `Node2D` container type which serves as a
wrapper around a `Dictionary` data structure to hold one or more `BoardLayer`
values. Using multiple layers allows for the organization of differing types,
such as separating movable units from items on the ground.

[`BoardLayer`](Scenes/BoardLayer.cs) is another `Node2D` type which contains
logic for interacting with occupants, cell highlighting, and pathfinding. At
its core, `BoardLayer` is a wrapper around a `Dictionary` data structure
containing values which implement the `IOccupant` interface, allowing you to
provide your own types when adding content to a layer. `IOccupant` is a minimal
interface to ensure `BoardLayer` occupants can be moved between grid cells. The
interface contains the following methods:

```cs
public interface IOccupant
{
  // GetCell should return the Occupant's current position. This is used when
  // moving Occupants, or when pathfinding for an Occupant.
  Vector2I GetCell();

  // GetRange determines how far the Occupant can move by restricting pathfinding.
  int GetRange();

  // ReadyToMove provides an opportunity to prevent an Occupant's movement.
  bool ReadyToMove();
}
```

## Usage

In order to get started, do the following:

1. Create a `Grid` resource based on the definition in `Resources/Grid.cs`.
1. Instantiate a `Board` with one or more `BoardLayer` children.
1. Implement an occupant type using the `IOccupant` interface.

Once this is done, you can start adding units to the board, and connect `Input`
signals to perform actions. See the [`BoardLayer`](https://github.com/willroberts/godot-grid-engine/blob/main/Scenes/BoardLayer.cs)
interface for details.

There is an end-to-end example in the [`Example/`](Example) directory.
Specifically, see [`Example.cs`](Example/Example.cs) and the accompanying scene.
To reproduce this in the editor, you will need to create tilesets for each
tilemap. Be sure to add a terrain layer and bitmask to the `PathTiles` tileset.

## To Do

- Integrate with Godot 4's tile system.
  - Allow better customization of layer, source, and tile atlas IDs.
  - Investigate integrations between level tilemaps and Grid/BoardLayer.
- Implement multiple selection as an alternative to shortest path
- Implement turn engine

## License

GNU General Public License v3. See [LICENSE](LICENSE).
