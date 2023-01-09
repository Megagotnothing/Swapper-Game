using Godot;
using System;

public class LevelSelector : GridContainer
{
    // Need to update to newer Godot Version to work...I think
    [Export]
    PackedScene[] levelScenes;
}
