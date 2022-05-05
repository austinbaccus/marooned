# Terminology

## Entity

An object that has components. Each [entity JSON](#entity-json) corresponds
to an entity.

## Component

A grouping of data. Each [component JSON](#component-json) corresponds
to a component.

## System

A group of functionality/logic performed on some set(s) of common components.
For instance, if there were a `PositionComponent` and a `VelocityComponent`,
then a corresponding system could be a `PhysicsSystem` that updates every
single `PositionComponent` given a corresponding `VelocityComponent`.

# File Structure

The file structure for data files will be as follows:

```
data/
    levels/
        [Example] level1.json
        [Example] level2.json
        ...
    entities/
        players/
            [Example] reimu.json
            [Example] marisa.json
            ...
        enemies/
            [Example] regular_fairy.json
            [Example] hard_fairy.json
            ...
        bosses/
            ...
        bullets/
            [Example] regular_bullet.json
            ...
        powerups/
            [Example] regular_powerup.json
            [Example] improved_powerup.json
            ...
    scripts/
        [Example] spawn_fairy_swarm.json
        [Example] spawn_regular_powerup.json
        ...
```

# Parsing

The program should parse the JSON at runtime. For example, when the game starts,
no JSON file (except for things like settings) should be loaded yet. It is only
when a level needs to be loaded is when its corresponding `.json` file is loaded.
For example, when the player begins a new game, `level1.json` should then be
parsed and loaded. When a [`spawn`](#spawn-action) action.

# Entity JSON

Every entity may have an optional `prototype` attribute, which is essentially
a way of copying another entity as a sort of "base" entity. For instance, a
`hard_fairy.json` could just look like this:

```json
{
    "prototype": "regular_fairy",
    "components": [
        {
            "type": "health",
            "value": 50
        }
    ],
    "on_death_script": [
        {
            "action": "script",
            "script": "spawn_regular_powerup"
        }
    ]
}
```

## Entity JSON Components

Every entity JSON can have some set of components, as long as the specified
component has a corresponding [component JSON](#component-json).

Usually, they will also have a `value` attribute, but they may also have other
attributes named something other than `value`. For instance, if there were
a `HealthComponent`, then an enemy could have that component by specifying:

```json
"components": [
    {
        "type": "health", // program will correspond "health" to a HealthComponent
        "value": 10
    }
]
```

in its JSON file.

An entity JSON will also usually have a [`script`](#script) attribute, which defines
the script the entity will run. For instance:

```json
"script": [
    {
        "action": "script",
        "script": "spawn_fairy_swarm"
    },
    {
        "time": 5,
        "action": "spawn",
        "position": "150 -10",
        "entity": "hard_fairy"
    }
]
```

will run the `spawn_fairy_swarm` script (at `time=0`, as [`time`](#time) is omitted), then
5 seconds later will [`spawn`](#spawn-action) 1 `hard_fairy` entity.

Here is a simple example of a `reimu.json` entity JSON under `players/`:

```json
{
    "components": [
        {
            "type": "texture",
            "value": "textures/reimu.png"
        },
        {
            "type": "player_move_input"
        },
        {
            "type": "physics",
            "velocity": 10.0
        },
        {
            "type": "collision",
            "radius_size": 5.0
        },
        {
            "type": "respawn",
            "respawns_left": 3
        },
        {
            "type": "powerup",
            "value": "regular"
        }
    ],
    "script": []
}
```

where e.g., `"type": "texure"` corresponds to a `TextureComponent`.

# Component JSON

```json
{
    "type": "component name"
}
```

The `type` is a string corresponding to some component. Each component
type will have its own additional attributes that can be inserted into
its corresponding JSON. For instance, the `respawn` component has
an additional attribute called `respawns_left`.

# Script

A **script** is a sequence of [actions/commands](#script-actions), each with a corresponding
[time](#time) to execute the action. A script by itself is not aware of an
[actor](#actor), as the actor is the one to perform a script. However, each
action will operate on some actor. For instance, a [`move`](#move) action will
move an entity (actor) in some way for some duration of time, but the script
does not need to know which entity. The script merely execute the action's code.

`Script` class:

```cs
public class Script
{
    public Queue<IAction> Actions;

    public void Add(IAction action);
    public void RemoveAll();
    public void RemoveFirst();
    public void ExecuteAll(GameTime gameTime);
    public void ExecuteFirst(GameTime gameTime);
}
```

## Actor

An **actor** is something that runs a [script](#script). This can be as simple
as an entity in the game, or it can be the current level.

# Script JSON

A script can be written in JSON, then referenced anywhere in an actor's JSON.
A script JSON is simply a list of [actions](#actions). For instance, the
`spawn_fairy_swarm` might look something like this:

```json
[
    {
        "action": "spawn",
        "entity": "regular_fairy",
        "position": "310 50",
        "script": [
            {
                "action": "move",
                "pattern": "left",
                "duration": 5
            },
            {
                "action": "move",
                "pattern": "up",
                "duration": 5
            }
        ]
    },
    {
        "action": "spawn",
        "entity": "regular_fairy",
        "position": "320 50",
        "script": [
            {
                "action": "move",
                "pattern": "left",
                "duration": 5
            },
            {
                "action": "move",
                "pattern": "up",
                "duration": 5
            }
        ]
    },
    {
        "action": "spawn",
        "entity": "regular_fairy",
        "position": "330 50",
        "script": [
            {
                "action": "move",
                "pattern": "left",
                "duration": 5
            },
            {
                "action": "move",
                "pattern": "up",
                "duration": 5
            }
        ]
    }
]
```

For instance,
in the `hard_fairy.json` example in the [entity JSON](#entity-json) section, the JSON
defines the following:

```json
"on_death_script": [
    {
        "action": "script",
        "script": "spawn_regular_powerup"
    }
]
```

which means, when the fairy dies, it will run the `spawn_regular_power.json` script.

Note that a script may also run other scripts using the [`script`](#script-action) action.

## Time

**Time** is an attribute inherent in each [action](#script-actions). If not specified in an action, the action will be executed immediately after script
is executed/run.

# Actions

An **action** (or command, corresponding to the Command Design Pattern) is some
sequence of code performed on some [actor](#actor) (or multiple actors). Every
action can have an (optional) [`time`](#time) attribute that indicates when
the action is to be performed after the [script](#script) is executed.

`IAction` class (same as Command in Command Design Pattern):

```cs
public interface IAction
{
    void Execute(GameTime gameTime);
}
```

## Spawn Action

Usage:
```json
{
    "action": "spawn",
    "entity": "entity json name to spawn",
    "position": "vector2",
    "count": 5,
    "script": []
}
```

Attributes:
- `entity: string`, the name of the entity to spawn (name as in its 
JSON file name)
- [Optional] `position: Vector2`, the position at which to spawn the entity
  - When omitted, will spawn at the location of the [actor](#actor). Note: this means
  it *should* be included for a Level actor, otherwise it would just spawn at 0, 0.
- [Optional] `count: integer`, the number of entities to spawn
  - Defaults to 1
- [Optional] `script: `[`ScriptJSON`](#script-json), the script to run for the entity.
  - This merges into the entity's script; this will **not** override an entity's
  script, but run "alongside" it.

## Script Action

Usage:
```json
{
    "action": "script",
    "script": "script json name to run"
}
```

Attributes:
- `script: string`, the name of the script to execute (name as in its
JSON file name)

## Attach Action

Usage:
```json
{
    "action": "attach",
    "components": []
}
```

Attributes:
- `components: list of components`, list of components to attach to the entity

## Detach Action

Usage:
```json
{
    "action": "detach",
    "components": [""]
}
```

Attributes:
- `components: list of names of components`, list of name of components to detach
from the entity (each name being a string, same as `type` attribute in a component JSON).

# Conventions

- Use [snake_case](https://en.wikipedia.org/wiki/Snake_case) for everything
in JSON.

# TODO

- Add more [actions](#actions)
- When we get around to adding [components](#component), we should add
sections on each component JSON that needs it
  - e.g., once we add something like a `PhysicsComponent`, we should add a
  `PhysicsComponent JSON` section
- Add level interpreter section
