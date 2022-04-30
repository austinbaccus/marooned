# Terminology

## Entity

An object that has components. Each [Entity JSON](#entity-json) corresponds
to an entity.

## Component

A grouping of data.

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
            [Example] normal_fairy.json
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
    "prototype": "normal_fairy",
    "components": [
        {
            "type": "health",
            "value": 50
        }
    ],
    "on_death_script": [
        {
            "action": "spawn",
            "entity": "regular_powerup"
        }
    ]
}
```

## Entity JSON Components

Every entity JSON can have some set of components, as long as the specified
component has a corresponding component JSON. That is, the component can be
loaded directly from JSON.

Each component has a `type` attribute that corresponds to some real
[component](#component). i.e.,

```json
{
    "type": "name of component"
}
```

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

Here is a simple example of a `reimu.json` entity JSON under `players/`:

```json
{
    "components": [
        {
            "type": "texture",
            "value": "textures/reimu.png"
        },
        {
            "type": "player_input"
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

where `"type": "texure"` corresponds to a `TextureComponent`.

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

    public void AddAction(IAction action);
    public void ExecuteAll(GameTime gameTime);
    public void ExecuteFirst(GameTime gameTime);
}
```

## Actor

An **actor** is something that runs a [script](#script). This can be as simple
as an entity in the game, or it can be the current level.

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
    public void Execute(GameTime gameTime);
}
```

## Spawn Action

Usage:
```json
{
    "action": "spawn",
    "entity": "entity json name to spawn",
    "count": 1
}
```

Attributes:
- `entity: string`, the name of the entity to spawn (name as in its 
JSON file name)
- [Optional] `count: integer`, the number of entities to spawn

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

## Powerup Action

Usage:
```json
{
    "action": "powerup",
    "powerup": "regular"
}
```

Attributes:
- `powerup: string`, the name of the powerup to grant to the entity

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
