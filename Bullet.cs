using Godot;
using System;

public class Bullet : RigidBody
{
    [Export]
    float bulletSpeed = 100f;

    public static int bulletCount = 2;

    public static Godot.Collections.Array<Bullet> bulletEntities = new Godot.Collections.Array<Bullet>();
    CollisionShape colShape;
    bool flying = true, reparented = false;
    public override void _Ready()
    {
        colShape = GetNode<CollisionShape>("BulletCollision");
        SetAsToplevel(true);
        bulletEntities.Add(this);
        GravityScale = 0f;
        if(bulletEntities.Count > bulletCount)
        {
            bulletEntities[0].QueueFree();
            bulletEntities.RemoveAt(0);
            
        }
    }

    public override void _IntegrateForces(PhysicsDirectBodyState state)
    {
        shootBullet(state);
    }

    void shootBullet(PhysicsDirectBodyState state)
    {
        if(flying && GetCollidingBodies().Count == 0)
        {
            state.LinearVelocity = -Transform.basis.z*bulletSpeed;
        }
        else if(flying && GetCollidingBodies()[0] is RigidBody)
        {
            Node collidedNode = (Node)GetCollidingBodies()[0];
            Transform pos = this.GlobalTransform;
            Node old = GetParent();
            old.RemoveChild(this);
            collidedNode.AddChild(this);
            this.GlobalTransform = pos;
            colShape.Disabled = true;
            reparented = true;
            SetAsToplevel(false);
            flying = false;
        }
        else
        {
            state.LinearVelocity = Vector3.Zero;
            colShape.Disabled = true;
            AxisLockAngularX = true;
            AxisLockAngularY = true;
            AxisLockAngularZ = true;
            flying = false;
        }
    }
}

