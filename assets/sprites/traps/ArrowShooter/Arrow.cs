using Godot;
using System;

public partial class Arrow : Area2D
{
    [Export] public float Speed = 400f;
    public Vector2 Direction = Vector2.Right;
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Direction * Speed * (float)delta;
        Rotation = Direction.Angle();
    }

    public void OnBodyEntered(Node body)
    {
        if (body.Name == "Layer 0") 
        {
            QueueFree();
        }
        if (body is Player) 
        {
            ((Player)body).TakeDamage(1);
            QueueFree();
        }
    }


}

