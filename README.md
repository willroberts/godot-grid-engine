# Godot Grid Engine

This is a plugin for Godot 4 which provides all of the constructs necessary to
create turn-based games which play out on a grid-based board.

Uses Godot 4, C#, and 2D nodes (3D not supported).

## Quick Start

1. TBD

## Design

Key Points:
- Optimized for mouse and touchscreen control.
- Use's Godot 4's tile system.
- Resources for Data, Nodes for Logic
- See best practices in mobile-trpg README
- UI events are emitted as Signals; user must handle all UI events.

Hierarchy:
- Grid (Resource, GlobalClass)
  Static data-only resource. Referenced by nodes. No inheritance.
- GameBoard (Node2D)
  Contains 1 or more GridLayer children.
  Methods: DrawNeighbors, DrawPath
  Exported Attributes: HighlightedTiles Tilemap, Arrow Tilemap
  Contains Pathfinder (RefCounted) script
  - GridLayer (Node2D)
    Tracks `Dictionary<Vector2I, T>`, a mapping of grid cells to user-defined value types.
    Should this correspond with Tilemap layers?
	Signals: MoveFinished
    Functions: IsOccupied(cell), ResetBoard()
    Inhabitant Functions: Select, Deselect, Clear, Move, GetWalkableCells (FloodFill)
    Input Events: Deselect inhabitant on user-defined input. Provide a method for users to call.

#### --- Engine vs Game Boundary ---

- Main
  - GridEngine Content (GameBoard, GridLayer)
    - UnitLayer (GridLayer)
      - Unit (Path2D + PathFollow2D + Sprite + AnimationPlayer)
        Some Path components may move to UnitLayer; API TBD.
        Handles animations via PathFollow2D curve.
		WalkAlong method enables processing
  - Map
  - UI
    - Cursor
      Parses input and emits signals to be handled by the game board.
      Draws a rectangle around the current cell (needed with mouse control?)
      Signals: Moved, Pressed emitted from UnhandledInput (move this)
      Input Events: Mouse movement, mouse click (both emit signals)

## To Do

- Implement multiple selection as an alternative to shortest path
- Implement turn engine

## License

GNU General Public License v3. See [LICENSE](LICENSE).
