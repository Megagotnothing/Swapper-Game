using Godot;
using System;

public class PlayerMovement : RigidBody
{
    

    [Export]
    public float playerSpeed = 25, airSpeed = 5, swimSpeed = 5;

    [Export]
    public float groundDrag = 0.2f;
    
    [Export]
    public float groundAccel = 5, airAccel = 0.5f, waterDrag = 3;

    [Export]
    public float jumpPower = 10f, waterJump = 5f;

    [Export]
    public int shootDistance = 100;

    [Export]
    float gravity = 9.8f;

    [Export]
    float floorCastDistance = -1;

    Vector2 inputMotion;
    
    Spatial cameraArm;
    
    Area floorCollide;
    Godot.Object target = null;
    
    public bool toSwap = false, onGround = false, toSwim = false, surfacing = false;
    
    public override void _Ready()
    {
        // OS.WindowFullscreen = true;
        // floorCast.CastTo = floorCast.CastTo * floorCastDistance;
        cameraArm = GetNode<Spatial>("CameraArm");
        
        // flashlight.Visible = false;
        Input.SetMouseMode(Input.MouseMode.Captured);
        floorCollide = GetNode<Area>("OnGroundCheck");
    }

    public override void _Process(float delta)
    {
    
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        //Esc to see mouse cursor
        if(Input.IsKeyPressed(16777217))
        {
            Input.SetMouseMode(Input.MouseMode.Visible);
        }
        inputMotion = Input.GetVector("move_left","move_right","move_back","move_forward").Normalized();
    }

    public override void _PhysicsProcess(float delta)
    {
        GroundMovement(delta);
    }

    public override void _IntegrateForces(PhysicsDirectBodyState state)
    {
        Vector3 flatVector = new Vector3(state.LinearVelocity.x, 0, state.LinearVelocity.z);
        float dotFlat = getMoveDirection().Dot(state.LinearVelocity);
        
        if(onGround && flatVector.Length() > playerSpeed)
        {
            Vector3 limit = flatVector.Normalized() * playerSpeed;
            state.LinearVelocity = new Vector3(limit.x, state.LinearVelocity.y, limit.z);
        }

        if(onGround && inputMotion.Length() < 0.2)
        {
            Vector3 lerped = state.LinearVelocity.LinearInterpolate(Vector3.Zero, groundDrag);
            state.LinearVelocity = new Vector3(lerped.x, state.LinearVelocity.y, lerped.z);
        }
        if(toSwap)
        {
            SwapPositionWithPlayer(state);
            SwapVelocityWithPlayer(state);
            toSwap = false;
        }
    }

    private void GroundMovement(float delta)
    {
        float accel = onGround ? groundAccel : airAccel;
        
        
        if(Input.IsActionJustPressed("jump") && onGround)
        {
            ApplyCentralImpulse(Vector3.Up * jumpPower);
        }
        Vector3 flatVector = new Vector3(LinearVelocity.x, 0, LinearVelocity.z);
        float dotFlat = flatVector.Dot(getMoveDirection());
        
        AddCentralForce(getMoveDirection() * accel);
    }

    Vector3 getMoveDirection()
    {
        Vector3 moveDirection = Vector3.Zero;
        moveDirection += inputMotion.x * cameraArm.GlobalTransform.basis.x;
        moveDirection -= inputMotion.y * cameraArm.GlobalTransform.basis.z;
        return moveDirection;
    }

    void SwapPositionWithPlayer(PhysicsDirectBodyState state)
    {
        Transform tempTarget = (Transform) target.Get("global_transform");
        target.Set("global_transform", GlobalTransform);
        state.Transform = new Transform(state.Transform.basis, tempTarget.origin);
    }

    void SwapVelocityWithPlayer(PhysicsDirectBodyState state)
    {
        Vector3 tempTarget = (Vector3) target.Get("linear_velocity");
        target.Set("linear_velocity", state.LinearVelocity);
        state.LinearVelocity = tempTarget;
    }

    public void _on_OnGroundCheck_body_entered(object body)
    {
        onGround = true;
    }

    public void _on_OnGroundCheck_body_exited(object body)
    {
        onGround = false;
    }
}
