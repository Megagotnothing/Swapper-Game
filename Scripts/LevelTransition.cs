using Godot;
using System;

public class LevelTransition : CSGBox
{
    [Export]
    PackedScene nextLevel;

    public void _on_EndArea_body_entered(object body)
    {
        if(((Node)body).Name == "Player")
        {
            GetTree().ChangeSceneTo(nextLevel);
        }
    }
}
