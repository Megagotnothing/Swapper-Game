using Godot;
using System;

public class SwapGun : Spatial
{
    [Export(PropertyHint.Enum,"Swap,Possess")]
    public String gunMode = "";

    [Export]
    public int existingBullets = 2;

    PackedScene bulletScene;

    Spatial muzzle;
    public Vector3 aimPoint = Vector3.Zero;
    public override void _Ready()
    {
        Bullet.bulletCount = existingBullets;
        bulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
        muzzle = GetNode<Spatial>("Muzzle");
    }
    
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("shoot"))
        {
            Bullet bullet = (Bullet)bulletScene.Instance();
            muzzle.AddChild(bullet);
            bullet.LookAt(aimPoint, Vector3.Up);
            LookAt(aimPoint, Vector3.Up);
            bullet.flying = true;
            GD.Print(aimPoint);
        }
        if(Bullet.bulletEntities.Count == 2 && 
            Bullet.bulletEntities[0].target != null && 
                Bullet.bulletEntities[1].target != null)
        {
            switch(gunMode)
            {
                case "Swap":
                    if(Input.IsActionJustPressed("activate_gun"))
                    {
                        SwapPosition(Bullet.bulletEntities[0].target, Bullet.bulletEntities[1].target);
                        SwapVelocity(Bullet.bulletEntities[0].target, Bullet.bulletEntities[1].target);
                    }
                    break;
                
                case "Possess":
                    if(Input.IsActionPressed("activate_gun"))
                    {
                        Possess(Bullet.bulletEntities[0].target, Bullet.bulletEntities[1].target);
                    }
                    else
                    {
                        if(Bullet.bulletEntities[1].target == Owner)
                        {
                            PlayerMovement player = (PlayerMovement)Bullet.bulletEntities[1].target;
                            player.canMove = true;
                        }
                    }
                    break;

                default:
                    GD.Print("Invalid Gun Mode");
                    break;
            }
        }
        else if(Input.IsActionJustPressed("activate_gun"))
        {
            GD.Print("Invalid Target");
        }
    }

    void SwapPosition(RigidBody target1, RigidBody target2)
    {
        Vector3 org1 = target1.GlobalTransform.origin;
        Transform t1 = target1.GlobalTransform;
        t1.origin = target2.GlobalTransform.origin;
        target1.GlobalTransform = t1;

        Transform t2 = target2.GlobalTransform;
        t2.origin = org1;
        target2.GlobalTransform = t2;
    }

    void SwapVelocity(RigidBody target1, RigidBody target2)
    {
        Vector3 tempTarget = target1.LinearVelocity;
        target1.LinearVelocity = target2.LinearVelocity;
        target2.LinearVelocity = tempTarget;
    }

    //Possibly add control of multiple bodies with one controller?
    //Posses also allows rotation control?
    //Possessed player would rotate in ragdoll and eventuall get back up
    void Possess(RigidBody controller, RigidBody body)
    {
        if(body == Owner)
        {
            PlayerMovement player = (PlayerMovement)Owner;
            player.canMove = false;
        }
        body.LinearVelocity = controller.LinearVelocity;
    }

    bool isPlayer(RigidBody body)
    {
        return Owner == body;
    }
}
