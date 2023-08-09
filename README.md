# Godot Grid Engine

A Godot library for games with grid-based movement.

Uses Godot 4 and C#.

## Quick Start

1. TBD

## Design

Key Points:
- Optimized for mouse and touchscreen control.
- Use's Godot 4's tile system.
- Resources for Data, Nodes for Logic
- See best practices in mobile-trpg README
- UI events are emitted as Signals; user must handle all UI events.
- No plans for 3D support.

Library Components:
- Grid (Resource, GlobalClass): Static data-only resource. Referenced by nodes.
- Board (Node2D): Contains 1 or more BoardLayer children.
  - BoardLayer (Node2D): Maps grid cells to inhabitants, e.g. units.

User Components (not included; signals and callbacks provided):
- Units and animations
- Level maps
- UI elements

#### --- Engine vs Game Boundary ---

- Main
  - GridEngine Content (GameBoard, BoardLayer)
    - UnitLayer (BoardLayer)
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

## API Mapping

GameBoard:OnCursorAcceptPressed -> BoardLayer:SelectOrMoveCallback
GameBoard:OnCursorMoved -> BoardLayer:DrawPathCallback
Pathfinder -> Pathfinder

## To Do

- Implement multiple selection as an alternative to shortest path
- Implement turn engine?

## License

GNU General Public License v3. See [LICENSE](LICENSE).
