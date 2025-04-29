using Godot;
using System;

public partial class ArrowShooter : Node2D
{
    [Export] public PackedScene ArrowScene;
    [Export] public Vector2 ShootDirection;
    [Export] public float FireInterval = 1.5f;


    private Timer _shootTimer;

    public override void _Ready()
    {
        _shootTimer = GetNode<Timer>("ShootTimer");
        _shootTimer.WaitTime = FireInterval;
        _shootTimer.Timeout += OnShootTimerTimeout;
    }

    private void OnShootTimerTimeout()
    {
        ShootArrow();
    }

    private void ShootArrow()
    {
        if (ArrowScene == null)
        {
            GD.PrintErr("ArrowScene no está asignado");
            return;
        }

        var arrow = ArrowScene.Instantiate<Arrow>();
        arrow.Position = GlobalPosition;
        arrow.Direction = ShootDirection.Normalized();
        GetTree().CurrentScene.AddChild(arrow);
    }
}