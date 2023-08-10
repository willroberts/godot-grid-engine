# Godot Grid Engine

A Godot library for games with grid-based movement.

Uses Godot 4 and C#.

## Usage

Library Components:
- Grid (Resource, GlobalClass): Static data-only resource. Referenced by nodes.
- Board (Node2D): Contains 1 or more BoardLayer children.
- BoardLayer (Node2D): Maps grid cells to occupants, e.g. units.

User-implemented Components (signals and/or callbacks provided):
- Units using the IOccupant interface
- Level tilemaps
- UI elements

## To Do

- Optimize for mouse and touchscreen control.
- Integrate with Godot 4's tile system.
- Emit UI events as Signals; user must handle all UI events.
  - Use `Callable` and `FuncRef` for callbacks on Signals
  - Consider the Signal Bus pattern (autoload singleton of Signals)
- Implement all debug component scenes
- Implement multiple selection as an alternative to shortest path
- Implement turn engine

## License

GNU General Public License v3. See [LICENSE](LICENSE).
