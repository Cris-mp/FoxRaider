using Godot;
using System;

public partial class DeathZone : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            player.TakeDamage(6);
        }
    }
}
