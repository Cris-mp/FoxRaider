using Godot;
using System;

public partial class Fragmento : Area2D
{
    [Export] public string Color; // "red", "green", "blue"
    [Export] public Texture2D FragmentTexture;

    private Sprite2D sprite;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite");
        sprite.Texture = FragmentTexture;

        BodyEntered += OnCollected;
    }

    private void OnCollected(Node body)
    {
        if (body is Player)
        {
            GameState.CollectFragment(Color);
            //var hud = GetTree().Root.GetNode<LagrimaHUD>("LagrimaHUD");
            //hud.ActualizarHUD();
            QueueFree();
        }
    }
}
