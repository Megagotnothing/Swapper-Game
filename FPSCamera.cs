using Godot;
using System;

public class FPSCamera : Spatial
{
    [Export]
    public float mouseSensitvity = 1f;

    Vector2 inputMotion;
    Spatial cameraHand;
    SpotLight flashlight;
    public override void _Ready()
    {
        mouseSensitvity /= 1000;
        cameraHand = GetNode<Spatial>("CameraHand");
        flashlight = GetNode<SpotLight>("CameraHand/Flashlight");
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
        {
            float upDown = -mouseMotion.Relative.y * mouseSensitvity;
            float leftRight = -mouseMotion.Relative.x * mouseSensitvity;
            
            Rotation += Vector3.Right * upDown;
            Rotation +=  Vector3.Up * leftRight;

            float lockUpDown = Mathf.Clamp(cameraHand.Rotation.x, -Mathf.Pi/2, Mathf.Pi/2);
            cameraHand.Rotation = new Vector3(lockUpDown, cameraHand.Rotation.y, cameraHand.Rotation.z);
            
        }
    }
    private void handleFlashlight()
    {
        if(Input.IsActionJustPressed("toggle flashlight"))
        {
            flashlight.Visible = !flashlight.Visible;
        }
    }
}
