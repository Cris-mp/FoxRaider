using Godot;
using System;

public partial class Bosque1Fase1 : Node2D
{
    public override void _Ready()
{
    var player = GetNode<Player>("Player");
    player.SetLimits(0f, 2562f); // Límites personalizados para este nivel
}
}
