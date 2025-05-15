using Godot;
using System;

public partial class Bosque1Fase3 : Node2D
{
    public override void _Ready()
    {
        var player = GetNode<Player>("Player");
        var hud = GetNode<Hud>("Hud");

        player.SetLimits(0f, 2096f);
        player.HealthChanged += hud.UpdateLife; 
        
    }
}
