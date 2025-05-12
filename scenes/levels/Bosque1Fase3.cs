using Godot;
using System;

public partial class Bosque1Fase3 : Node2D
{
    public override void _Ready()
    {
        var player = GetNode<Player>("Player");
        var hud = GetNode<Hud>("Hud");

        player.SetLimits(0f, 2096f); // Límites personalizados para este nivel
        player.HealthChanged += hud.UpdateLife; // Conectar la señal de cambio de salud al HUD
        //player.TeerChange += hud.UpdateTeer; // Conectar la señal de cambio de teer al HUD
        //player.ScoreChange += hud.UpdateScore; // Conectar la señal de cambio de puntaje al HUD
    }
}
