using Godot;
using System;

public partial class FallingPlatform : Node2D
{
    [Export] public float FallDelay = 0.5f;
    [Export] public float RespawnDelay = 3f;
    [Export] public float FallDuration = 2f; // Duración de la caída suave

    private Vector2 startPosition;
    private CharacterBody2D platformBody;
    private CollisionShape2D platformCollision;
    private CollisionShape2D platformSensor;
    private Sprite2D sprite;
    private Timer respawnTimer;

    public override void _Ready()
    {
        // Obtención de nodos
        platformBody = GetNode<CharacterBody2D>("FallingPlatform");
        startPosition = platformBody.GlobalPosition;

        var area = platformBody.GetNode<Area2D>("AreaFallPlat");
        platformCollision = platformBody.GetNode<CollisionShape2D>("CollisionFallPlat");
        platformSensor = area.GetNode<CollisionShape2D>("CollisionSensor");
        sprite = platformBody.GetNode<Sprite2D>("SpriteFallPlat");
        respawnTimer = platformBody.GetNode<Timer>("ResetTimer");
      
        // Area para detectar al jugador
        area.BodyEntered += OnBodyEntered;
        respawnTimer.Timeout += OnRespawn;
    }

    private async void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            await ToSignal(GetTree().CreateTimer(FallDelay), "timeout");
            platformCollision.Disabled = true;
            platformSensor.Disabled = true;
            sprite.Visible = false;
            respawnTimer.Start(RespawnDelay);
        }
    }

    private void OnRespawn()
    {
        platformBody.GlobalPosition = startPosition;
        platformCollision.Disabled = false;
        platformSensor.Disabled = false;
        sprite.Visible = true;
    }
}

