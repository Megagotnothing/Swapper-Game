using Godot;
using System;

public class SwapGun : Spatial
{
    [Export]
    int existingBullets = 2;

    PackedScene bulletScene;
    RigidBody body;
    public override void _Ready()
    {
        Bullet.bulletCount = 1;
        bulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
        body = GetParent().GetParent().GetParent<RigidBody>();
    }
    
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("shoot"))
        {
            RigidBody bullet = (RigidBody)bulletScene.Instance();
            AddChild(bullet);
        }
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
}
