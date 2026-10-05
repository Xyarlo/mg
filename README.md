# mg

A small command-line wrapper for MonoGame development.

## Installation

Download the latest binary Release and add it to your PATH.

## Project Configuration

Copy `mg.config.example` to `mg.config` in the MonoGame project directory and update the project path. Relative paths are resolved from the directory containing `mg.config`.

mg does currently not offer any project-specific configurations.

```ini
[project]
path=MyGame.csproj
```

## Commands

```text
mg build
mg run
mg publish
mg build run
```

Tasks execute in the order provided. If a task fails, later tasks are not executed.
