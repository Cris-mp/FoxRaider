using Godot;
using System;

public partial class Hud : CanvasLayer
{
    [Export] private AnimatedSprite2D lifeSprite;
    [Export] private Sprite2D teerSprite;
    [Export] private Label scoreLabel;

    [Export] private Texture2D[] teerStates; // 0: vacía, 1: parcial, 2: completa

    private int score = 0;

    public override void _Ready()
    {
        UpdateTeer();
        UpdateScore(0);
        UpdateLife(6); // Vida inicial máxima
    }

    public void UpdateLife(int currentHealth)
    {
        GD.Print($"[HUD] Actualizando vida: {currentHealth}");
        int clamped = Mathf.Clamp(currentHealth, 0, 6);

        if (!lifeSprite.IsPlaying())
        lifeSprite.Animation = "vida";


        lifeSprite.Frame = clamped; // Cambia el frame según la vida
    }

    public void UpdateScore(int points)
    {
        GD.Print($"[HUD] instancia real: {this}, está en escena: {IsInsideTree()}");
        score += points;
        scoreLabel.Text = $"score: {score:0000}";
    }

    public void UpdateTeer()
    {     
        GD.Print($"[HUD] instancia real: {this}, está en escena: {IsInsideTree()}");
        GD.Print($"[HUD] UpdateTeer llamado. Fragments - R:{GameState.HasRedFragment}, G:{GameState.HasGreenFragment}, B:{GameState.HasBlueFragment}");  
        int count = 0;
        if (GameState.HasRedFragment) count++;
        if (GameState.HasGreenFragment) count++;
        if (GameState.HasBlueFragment) count++;

        teerSprite.Texture = teerStates[count];
    }
}