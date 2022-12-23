using Godot;
using System;

public class Door : CSGBox
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Button_body_entered(object body)
    {
        
        if(body is RigidBody)
        {
            Operation = CSGBox.OperationEnum.Subtraction;
        }
    }

    public void _on_Button_body_exited(object body)
    {
        if(body is RigidBody)
        {
            Operation = CSGBox.OperationEnum.Union;
        }
    }
}
