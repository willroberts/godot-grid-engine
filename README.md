# Godot Grid Engine

A Godot library for games with grid-based movement.

Uses Godot 4 and C#.

## Library Design

This library provides three public types: `Grid`, `Board`, and `BoardLayer`.

`Grid` is a static, data-only `Resource` type which contains the basic
representation of a 2D grid. It is loaded by the other types.

`Board` is a `Node2D` container type which serves as a wrapper around a
`Dictionary` data structure to hold one or more `BoardLayer` values. Using
multiple layers allows for the organization of differing types, such as
separating movable units from items on the ground.

`BoardLayer` is another `Node2D` type which contains logic for interacting with
occupants, cell highlighting, and pathfinding. At its core, `BoardLayer` is a
wrapper around a `Dictionary` data structure containing values which implement
the `IOccupant` interface, allowing you to provide your own types when adding
content to a layer.

`IOccupant` is a minimal interface to ensure `BoardLayer` occupants can be
moved between grid cells. The interface contains the following methods:

```cs
public interface IOccupant
{
  Vector2I GetCell();
  int GetRange();
  bool ReadyToMove();
}
```

## Usage

In order to get started, do the following:

1. Create a `Grid` resource based on the definition in `Resources/Grid.cs`.
1. Instantiate a `Board` with one or more `BoardLayer` children.
1. Implement an occupant type using the `IOccupant` interface.

Once this is done, you can start adding units to the board, and connect `Input`
signals to perform actions. See the `BoardLayer` interface for details.

## To Do

- Integrate with Godot 4's tile system.
- Emit UI events as Signals; user must handle all UI events.
  - Use `Callable` and `FuncRef` for callbacks on Signals
  - Consider the Signal Bus pattern (autoload singleton of Signals)
- Implement multiple selection as an alternative to shortest path
- Implement turn engine

## License

GNU General Public License v3. See [LICENSE](LICENSE).
