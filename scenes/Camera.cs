using Godot;
using System;

public partial class Camera : Camera2D
{
    [ExportCategory("Config")]
    [ExportGroup("Required References")]
    [Export] private CharacterBody2D player;

    public override void _Process(double delta) {
        GlobalPosition=player.GlobalPosition;
    }
}
