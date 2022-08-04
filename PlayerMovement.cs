using Godot;
using System;

public class PlayerMovement : KinematicBody
{
    [Export]
    public float mouseSensitvity = 1f;

    [Export]
    public float playerSpeed = 10, airSpeed = 5, swimSpeed = 5;
    [Export]
    public float groundAccel = 10, airAccel = 5, waterDrag = 3;

    [Export]
    public float jumpPower = 10f, waterJump = 5f;

    [Export]
    public int shootDistance = 100;

    [Export]
    float gravity = 9.8f;

    float fallingSpeed = 0f;
    Vector3 velocity;
    Vector3 inputMotion;
    Spatial cameraArm;
    SpotLight flashlight;
    RayCast rayCast;
    
    Godot.Object target = null;

    public bool toSwim = false, surfacing = false;
    
    public override void _Ready()
    {
        mouseSensitvity /= 1000;
        Input.SetMouseMode(Input.MouseMode.Captured);
        cameraArm = GetNode<Spatial>("CameraArm");
        flashlight = GetNode<SpotLight>("CameraArm/Flashlight");
        rayCast = GetNode<RayCast>("CameraArm/RayCast");

        rayCast.CastTo *= shootDistance;
    }

    public override void _Process(float delta)
    {
        handleInputs(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
            cameraArm.Rotation = Vector3.Right * Mathf.Clamp(cameraArm.Rotation.x-mouseMotion.Relative.y * mouseSensitvity, -Mathf.Pi/2, Mathf.Pi/2) ;
            RotateY(-mouseMotion.Relative.x * mouseSensitvity);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        swapGun(delta);
        if(toSwim)
        {
            if(IsOnFloor())
            {
                GroundMovement(delta);
            }
            else
            {
                Swimming(delta);
            }
        }
        else
        {
            GroundMovement(delta);
        }
    }

    private void handleInputs(float delta)
    {
        inputMotion.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        inputMotion.y = -cameraArm.Rotation.x/(Mathf.Pi/2);
        inputMotion.z = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward");
        
        handleFlashlight();
    }

    private void handleFlashlight()
    {
        if(Input.IsActionJustPressed("toggle flashlight"))
        {
            flashlight.Visible = !flashlight.Visible;
        }
    }

    private void swapGun(float delta)
    {
        if(Input.IsActionJustPressed("shoot"))
        {
            rayCast.Enabled = true;
        }
        if(Input.IsActionPressed("shoot"))
        {
            //Shoot Beam 
        }
        else if(Input.IsActionJustReleased("shoot"))
        {
            target = rayCast.GetCollider();
            
            rayCast.Enabled = false;
            // GD.Print(target);
        }

        if(Input.IsActionJustPressed("teleport"))
        {
            GD.Print(target.GetPropertyList());
        }
    }

    private void GroundMovement(float delta)
    {
        

        Vector3 direction = Vector3.Zero;

        direction += inputMotion.x * Transform.basis.x + inputMotion.z * Transform.basis.z;
        
        float accel, speed;
        Vector3 dir;
        if(IsOnFloor())
        {
            accel = groundAccel;
            speed = playerSpeed;
            direction = direction.Normalized() * speed;
            dir = direction;
        }
        else
        {
            accel = airAccel;
            speed = airSpeed;
            direction = direction.Normalized() * speed;

            float currentSpeed = velocity.Dot(direction);
            float addSpeed = Mathf.Clamp(speed - currentSpeed, 0, accel);
            dir = velocity + addSpeed * direction;
        }
        UpdateFalling(delta);
        velocity = velocity.LinearInterpolate(dir, accel * delta);
        velocity = MoveAndSlide(velocity, Vector3.Up, infiniteInertia: false);
    }

    private void Swimming(float delta)
    {
        Vector3 direction = Vector3.Zero;

        direction += inputMotion.x * Transform.basis.x;
        direction.y += inputMotion.y * inputMotion.z;
        direction += inputMotion.z * Transform.basis.z;
        if(Input.IsActionPressed("jump") && toSwim)
        {
            direction.y = 1;
        }
        direction = direction.Normalized() * swimSpeed;

        
        velocity = velocity.LinearInterpolate(direction, waterDrag * delta);
        velocity = MoveAndSlide(velocity, Vector3.Up);
    }

    private void UpdateFalling(float delta)
    {
        if(IsOnFloor())
        {
            fallingSpeed = -0.05f;
        }
        else
        {
            fallingSpeed -=  3 * gravity * delta;
        }

        if(Input.IsActionPressed("jump") && IsOnFloor())
        {
            fallingSpeed = jumpPower;
        }

        if(Input.IsActionPressed("jump") && surfacing)
        {
            fallingSpeed = waterJump;
            surfacing = false;
        }

        
        velocity.y = fallingSpeed;
    }
    public void ResetFall()
    {
        fallingSpeed = 0f;
    }
    
}
