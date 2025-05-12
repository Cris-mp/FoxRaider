using Godot;
using System;

public partial class RespawnArea : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            player.SetRespawnPoint(GlobalPosition);
        }
    }
}
