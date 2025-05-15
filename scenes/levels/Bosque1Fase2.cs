using Godot;
using System;

public partial class Bosque1Fase2 : Node2D
{
    public override void _Ready()
    {
        var player = GetNode<Player>("Player");
        var hud = GetNode<Hud>("Hud");

        player.SetLimits(0f, 1344f); 
        player.HealthChanged += hud.UpdateLife;
        
    }
}
