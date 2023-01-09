using Godot;
using System;

public class PlayButton : Button
{
    public PackedScene savedLevel;
    public override void _Ready()
    {
        savedLevel = GD.Load<PackedScene>("res://Level1.tscn");
    }

    public void _on_PlayButton_pressed()
    {
        GetTree().ChangeSceneTo(savedLevel);
    }
}
