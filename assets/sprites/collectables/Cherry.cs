using Godot;
using System;

public partial class Cherry : Area2D
{
    [Export] public int Points = 100;
    [Export] public AnimatedSprite2D AnimatedSprite;

    private bool isCollected = false;

    public override void _Ready()
    {
        base._Ready();
        AnimatedSprite.Play("Idle");    
    }

    private void recolectarCherry(Node body)
    {
        if (isCollected) return;

        if (body is Player player)
        {
            AnimatedSprite.Play("Collect"); // Reproducir animación de "Collect" al ser recolectada
            isCollected = true;
        }
    }
    //Esto me esta dando problemas
    public void OnAnimationFinishedCollectable(StringName animName)
    {
        if (animName == "Collect")
            QueueFree(); // Eliminar el objeto Cherry después de la animación
    }
}
