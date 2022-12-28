using Godot;
using System;

public class FPSCamera : Spatial
{
    [Export]
    public float mouseSensitvity = 1f;

    Vector2 inputMotion;
    Spatial cameraHand, holdPos;
    SpringArm objectArm;
    SpotLight flashlight;
    RayCast eyeCast;
    bool holding = false;
    public override void _Ready()
    {
        mouseSensitvity /= 1000;
        cameraHand = GetNode<Spatial>("CameraHand");
        flashlight = GetNode<SpotLight>("CameraHand/Flashlight");
        objectArm = GetNode<SpringArm>("CameraHand/ObjectArm");
        holdPos = GetNode<Spatial>("CameraHand/ObjectArm/HoldPosition");
        eyeCast = GetNode<RayCast>("CameraHand/RayCast");
    }

    public override void _PhysicsProcess(float delta)
    {
        pickUp(delta);
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
        GD.Print(eyeCast.GetCollider());
        RigidBody body = null;
        if(eyeCast.GetCollider() is RigidBody)
        {
            body = (RigidBody) eyeCast.GetCollider();
            body.Mode = RigidBody.ModeEnum.Kinematic;
        }
    }
}
