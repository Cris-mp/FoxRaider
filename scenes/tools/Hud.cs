using Godot;
using System;

public partial class HUD : CanvasLayer
{
    [Export] private Sprite2D lifeSprite;
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
        int clamped = Mathf.Clamp(currentHealth, 0, 5);
        lifeSprite.Frame = clamped; // Cambia el frame según la vida
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreLabel.Text = $"score: {score:0000}";
    }

    public void UpdateTeer()
    {
        int count = 0;
        if (GameState.HasRedFragment) count++;
        if (GameState.HasGreenFragment) count++;
        if (GameState.HasBlueFragment) count++;

        teerSprite.Texture = teerStates[count];
    }
}