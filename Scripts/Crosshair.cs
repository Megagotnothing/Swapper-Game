using Godot;
using System;

public class Crosshair : TextureRect
{
    [Export]
    public float scale = 1; // scale of 1 is 64x64 pixels

    public override void _Ready()
    {
        RectScale = RectScale * scale;
        Vector2 offset = RectSize * scale;
        RectPosition = (OS.WindowSize / 2) - offset/2;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
