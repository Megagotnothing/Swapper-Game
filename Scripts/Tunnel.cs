using Godot;
using System;

public class Tunnel : CSGBox
{   
    [Export]
    NodePath buttonPath;

    Area button;

    Tween tween;
    CSGBox door;
    Vector3 start, end;

    
    public override void _Ready()
    {
        tween = GetNode<Tween>("Tween");
        door = GetNode<CSGBox>("Door");
        start = door.Translation;
        end = door.Translation + Vector3.Up * 4.9f;
        button = GetNode<Area>(buttonPath);
    }

    public void _on_Button_body_entered(object body)
    {
        if(button.GetOverlappingBodies().Count != 0)
        {
            tween.InterpolateProperty(door, "translation", door.Translation, end, 1, transType: Tween.TransitionType.Bounce, easeType: Tween.EaseType.Out);
            tween.CallDeferred("start");
        }
    }

    public void _on_Button_body_exited(object body)
    {
        if(button.GetOverlappingBodies().Count == 0)
        {
            tween.InterpolateProperty(door, "translation", door.Translation, start, 1, transType: Tween.TransitionType.Bounce, easeType: Tween.EaseType.Out);
            tween.CallDeferred("start");
        }
    }
}
