# Drone vs Enemy Combat Simulation (Prototype)

A Unity prototype where the player flies a drone and fires missiles, while NavMesh-patrolling enemy soldiers detect the drone, stop, aim and shoot back.

- **Unity version:** 6000.3.8f1
- **Input:** new Input System (`InputSystem_Actions` asset, actions subscribed through events)
- **Author:** Samriddhi Kushwaha

## Demo

- **Demo video:** [https://drive.google.com/file/d/1760sVBO3Cy0zW0sVpn5b6lV8PVsc7DL2/view?usp=sharing]
- **Build:** [(https://drive.google.com/drive/folders/1S4HRkkCLXMWcSKtLVfs-JMtb8NEAXABB?usp=sharing)]
- **Github:** [https://github.com/Sammyverse0/DroneSimulation]

## Controls

| Input | Action |
|---|---|
| W / A / S / D | Move the drone forward / left / back / right |
| E | Ascend |
| Q | Descend |
| Mouse | Turn the drone and aim up and down |
| Space | Fire a missile |

The crosshair in the centre of the screen shows where missiles will land.

## Features implemented

**Player drone**
- Rigidbody-based flight with smooth acceleration (WASD + Q/E)
- Mouse look for yaw and pitch
- Missile fired with Space, with a cooldown
- Cinemachine follow camera
- Engine hum and missile-launch sound, plus a thruster particle effect
- Health bar, red screen flash on damage and a centre crosshair that missiles aim toward

**Missile system**
- Missile prefab with a Rigidbody and trigger collider
- Explodes on contact, with a blast radius that damages every valid target in range
- Particle explosion effect

**Enemy AI**
- Soldier character with a NavMeshAgent
- Patrols between waypoints using the NavMesh
- Detects the drone using a range check plus a line-of-sight raycast, so walls block vision
- On detection: stops, turns to face the drone and fires bullets at it
- Returns to patrol when the drone is out of range or hidden

**Targets**
- Dummy objects tagged `Target`, destroyed by missiles

**Game flow**
- Main menu scene with Play and Quit
- Mission complete panel when all soldiers are destroyed
- Drone destroyed panel when the drone's health reaches zero
- Restart and Main Menu buttons on both panels

## Project structure

| Script | Purpose |
|---|---|
| `DroneController` | Drone movement and look, driven by Input System events |
| `DroneShooter` | Fires missiles toward the crosshair |
| `Projectile` | Shared by missiles and enemy bullets: flight, explosion, damage |
| `Health` | Health for the drone, soldiers and targets; health bar, damage flash and death event |
| `EnemyAI` | Patrol and attack states, detection and shooting |
| `GameManager` | Win and lose conditions, panels, restart and menu |
| `MainMenu` | Play and Quit for the menu scene |

## How to run

1. Open the project in Unity [version].
2. Open the `MainMenu` scene and press Play, or run the included build.

If you open the game scene directly, re-bake the NavMesh from the `NavMeshSurface` on the ground if the soldiers don't move.

## Known bugs and limitations

- Camera clipping through walls
- Enemies do not lead their shots, so a moving drone is mostly hit by chance.
- A single level, with no ammo limit or enemy variety.
- Enemy detection uses a distance and raycast check, with no field-of-view cone, so soldiers can detect the drone from behind.
- Sounds are limited to the drone hum and the missile launch.
- Targets do not count toward the mission-complete condition; only soldiers do.