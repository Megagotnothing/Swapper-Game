using Godot;
using System;

public class Tunnel : CSGBox
{   
    Tween tween;
    CSGBox door;
    Vector3 start, end;
    public override void _Ready()
    {
        tween = GetNode<Tween>("Tween");
        door = GetNode<CSGBox>("Door");
        start = door.Translation;
        end = door.Translation + Vector3.Up * 4.9f;
    }

    public void _on_Button_body_entered(object body)
    {
        tween.InterpolateProperty(door, "translation", door.Translation, end, 1, transType: Tween.TransitionType.Bounce, easeType: Tween.EaseType.Out);
        tween.Start();
    }

    public void _on_Button_body_exited(object body)
    {
        tween.InterpolateProperty(door, "translation", door.Translation, start, 1, transType: Tween.TransitionType.Bounce, easeType: Tween.EaseType.Out);
        tween.Start();
    }
}
