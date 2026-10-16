# Strategy camera

Open `Assets/Scenes/SampleScene.unity`. The existing `_player` object has
`PlayerCore` and `PlayerControls`, with `_playerCamera` as its child. The input
asset and camera references are already assigned.

The player rig's world position is the virtual focus point. Position `_player`
to choose the starting focus. `PlayerControls` sets the camera's world rotation
and places it at `Distance` from that point; no target object is required.
The default scene pose looks at the origin from 20 units away with a 45-degree
downward pitch. The sample scene currently has no ground or buildings, so add
temporary scene geometry when checking camera motion visually.

## Controls

| Input | Player action | Behavior |
| --- | --- | --- |
| W / A / S / D | Move | Move the focus on the horizontal plane, relative to camera yaw |
| Mouse wheel | Zoom | Change orbit distance; scrolling up moves closer |
| Right mouse button + mouse movement | Rotate + Look | Orbit around the focus with yaw and clamped pitch |
| Middle mouse button + mouse movement | Pan + Look | Drag the view along the horizontal plane |
| Left Shift, held | Boost | Multiply WASD movement speed while held |

If both right and middle buttons are held, rotation takes priority. Mouse
movement alone does nothing. Pan moves opposite to the mouse to give a dragging
effect. Its sensitivity scales with the current orbit distance.

## Inspector setup

On `PlayerControls`, configure movement speed, boost multiplier, pan speed,
rotation speed, zoom speed, initial distance/yaw/pitch, distance limits, and
pitch limits. Distances stay positive and pitch limits remain within 1-89 degrees.
Zoom speed is in world units per scroll input unit; adjust it for the desired
mouse wheel sensitivity. Keyboard movement uses elapsed time; mouse delta and
wheel input are already per-frame deltas and are applied without `deltaTime`.

For another scene, add `PlayerCore` to a player rig; it requires a
`PlayerControls` component and resolves its reference if unassigned. Add a
separate child Camera and assign `Assets/InputSystem_Actions.inputactions` to
`PlayerControls.Input Actions`. Assign `Player Camera`, or leave it empty to use
the first child Camera. Keep the rig's scale at `(1, 1, 1)`.

Initialization works from `PlayerCore.Awake` and `PlayerControls.OnEnable`
without depending on script execution order. Each controls component owns a
runtime copy of the input asset, enables/disables only its `Player` map, and
destroys that copy when removed. The source asset's `UI` map and control schemes
are unchanged. No `PlayerInput` component or generated input wrapper is needed.

## Manual Unity verification

Use Unity **6000.6.6f1** and the project's Input System **1.20.0** package. The
project already uses the new Input System. After the project imports without
compiler errors, enter Play Mode and check:

1. The camera starts at the configured focus, distance, yaw, and pitch.
2. WASD moves horizontally at the same speed for single keys and diagonals.
   Rotate about 90 degrees and confirm W now follows the new camera heading.
3. Shift increases WASD speed only while held, including when released during
   movement. Keyboard travel over the same time is comparable at different FPS.
4. Moving the mouse without a button leaves the view unchanged. Holding the
   right button rotates around a stationary focus; extreme vertical motion
   stops at the configured pitch limits without flipping.
5. Holding the middle button drags the view horizontally. Rotation and orbit
   distance stay constant, and releasing the button stops dragging. Test after
   changing yaw and at both zoom limits.
6. Wheel up/down changes distance without moving the focus, and repeated input
   stops at both distance limits. Mouse sensitivity stays comparable at different
   FPS for the same mouse travel and wheel input.
7. Disable/re-enable `PlayerControls` and then `_player`. Controls resume without
   resetting the focus or orbit. Check the Console for errors and, if a UI is
   present, confirm its existing navigation/click/scroll bindings still work.
8. Leave `PlayerCore.Player Controls` and `PlayerControls.Player Camera` empty
   and confirm their component/child lookups work. Removing the input asset
   reference should log a clear setup error and disable camera controls.

Collision avoidance, obstruction handling, and automatic terrain height are
outside this implementation's scope.
