using Godot;
using System;

public partial class Cherry : Area2D
{
    [Export] public int Points = 100;
    [Export] public AnimatedSprite2D AnimatedSprite;
    private AudioStreamPlayer FeedSound;

    private bool isCollected = false;

    public override void _Ready()
    {
        base._Ready();
        FeedSound = GetNode<AudioStreamPlayer>("Feed");
        AnimatedSprite.Play("Idle");    
    }

    private void recolectarCherry(Node body)
    {
        if (isCollected) return;

        if (body is Player player)
        {
            AnimatedSprite.Play("Collect"); 
            FeedSound?.Play();
            isCollected = true;
        }
    }
   
    public void OnAnimationFinishedCollectable()
    {
        if (AnimatedSprite.Animation == "Collect")
            QueueFree();
    }
}
