using Godot;
using System;

public class FPSCamera : Spatial
{
    [Export]
    public float mouseSensitvity = 1f;

    [Export]
    public float linDamp = 20f, angDamp = 10f;

    [Export]
    public float grabDistance = 4;

    RigidBody body = null;

    Vector2 inputMotion;
    Spatial cameraHand;
    Spatial holdPos;
    SwapGun swapGun;
    SpotLight flashlight;
    RayCast grabCast;
    RayCast eyeCast;

    PackedScene bulletScene;

    bool holding = false;
    public override void _Ready()
    {
        mouseSensitvity /= 1000;
        cameraHand = GetNode<Spatial>("CameraHand");
        flashlight = GetNode<SpotLight>("CameraHand/Flashlight");
        holdPos = GetNode<Spatial>("CameraHand/HoldPosition");
        grabCast = GetNode<RayCast>("CameraHand/GrabCast");
        grabCast.CastTo = grabCast.CastTo * grabDistance;
        eyeCast = GetNode<RayCast>("CameraHand/EyeCast");
        swapGun = GetNode<SwapGun>("SwapGun");
        bulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
    }

    public override void _PhysicsProcess(float delta)
    {
        pickUp(delta);
        swapGun.aimPoint = eyeCast.GetCollisionPoint();
    }

    public override void _Input(InputEvent @event)
    {
        //Esc to see mouse cursor
        if(Input.IsKeyPressed(16777217))
        {
            Input.SetMouseMode(Input.MouseMode.Visible);
        }
        inputMotion = Input.GetVector("move_left","move_right","move_back","move_forward").Normalized();

        if(@event is InputEventMouseMotion mouseMotion)
        {   Vector3 rot = Rotation;
            float upDown = -mouseMotion.Relative.y * mouseSensitvity;
            float leftRight = -mouseMotion.Relative.x * mouseSensitvity;
            
            rot += Vector3.Right * upDown;
            rot +=  Vector3.Up * leftRight;

            rot.x = Mathf.Clamp(rot.x, -Mathf.Pi/2, Mathf.Pi/2);

            Rotation = rot;
        }

        if(Input.IsActionJustPressed("toggle flashlight"))
        {
            flashlight.Visible = !flashlight.Visible;
        }
    }

    void pickUp(float delta)
    {

        if(holding && body != null)
        {
            Vector3 directionToHold = body.GlobalTransform.origin.DirectionTo(holdPos.GlobalTransform.origin);
            float distanceToHold = body.GlobalTransform.origin.DistanceTo(holdPos.GlobalTransform.origin);
            body.ApplyCentralImpulse(directionToHold * distanceToHold + Vector3.Down * Mathf.Max(0,body.Mass-1));
            if(distanceToHold > 5f)
            {
                body.LinearDamp = -1;
                body.AngularDamp = -1;
                body = null;
                holding = false;
                return;
            }
        }

        if(!holding && Input.IsActionJustPressed("pickup") && grabCast.GetCollider() is RigidBody)
        {
            body = (RigidBody) grabCast.GetCollider();
            body.LinearDamp = linDamp;
            body.AngularDamp = angDamp;
            holding = true;
        }
        else if(holding && Input.IsActionJustPressed("pickup"))
        {
            body.LinearDamp = -1;
            body.AngularDamp = -1;
            body = null;
            holding = false;
        }
    }
}
