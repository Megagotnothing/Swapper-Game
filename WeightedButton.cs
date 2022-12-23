using Godot;
using System;

public class WeightedButton : Spatial
{
    

    [Export]
    float depressedDistance = 0.5f; 
    
    [Export]
    float buttonAccel = 0.5f;
    
    [Export]
    NodePath output;

    RigidBody button;
    Tween tween;
    Interact interactObject;

    public override void _Ready()
    {
        button = GetNode<RigidBody>("Button");
        tween = GetNode<Tween>("Button/Tween");
        interactObject = GetNode<Interact>(output);
    }

    void _on_Area_body_entered(object body)
    {
        tween.InterpolateProperty(button,"translation", button.Transform.origin, -Transform.basis.y * depressedDistance, 1.0f, Tween.TransitionType.Expo, Tween.EaseType.Out);
        tween.Start();
        
        interactObject.activate();
    }

    void _on_Area_body_exited(object body)
    {
        tween.InterpolateProperty(button,"translation", button.Transform.origin, Vector3.Zero, 1.0f, Tween.TransitionType.Expo, Tween.EaseType.Out);
        tween.Start();

        interactObject.deactivate();
    }
}
