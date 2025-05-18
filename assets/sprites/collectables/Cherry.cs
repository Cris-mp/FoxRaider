using Godot;
using System;

/// <summary>
/// Representa una cereza coleccionable en el juego. 
/// Al ser recogida por el jugador, reproduce una animación, un sonido y suma puntos al HUD.
/// </summary>
public partial class Cherry : Area2D
{
    [Export] public int Points = 100;

    [ExportGroup("Referencias de Nodo")]
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] private AudioStreamPlayer FeedSound;

    private bool isCollected = false;

    public override void _Ready()
    {
        base._Ready();
        AnimatedSprite.Play("Idle");
    }

    /// <summary>
    /// Lógica de recolección. Se llama cuando un cuerpo entra en contacto con el área de la cereza.
    /// Si el cuerpo es un jugador y la cereza aún no ha sido recogida, reproduce la animación y sonido,
    /// actualiza el puntaje en el HUD y marca la cereza como recolectada.
    /// </summary>
    /// <param name="body">El nodo que entró en contacto con la cereza.</param>
    private void recolectarCherry(Node body)
    {
        if (isCollected) return;

        if (body is Player player)
        {
            AnimatedSprite.Play("Collect");
            FeedSound?.Play();
            isCollected = true;
            var tree = (SceneTree)Engine.GetMainLoop();
            var hud = (Hud)tree.Root.GetNodeOrNull(body.GetParent().Name + "/Hud");
            hud?.UpdateScore(Points);
        }
    }

    /// <summary>
    /// Método llamado automáticamente cuando finaliza una animación del sprite.
    /// Si la animación terminada es "Collect", elimina el nodo de la escena.
    /// </summary>
    public void OnAnimationFinishedCollectable()
    {
        if (AnimatedSprite.Animation == "Collect")
            QueueFree();
    }
}
