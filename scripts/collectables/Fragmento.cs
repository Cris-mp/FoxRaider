using Godot;
using System;

/// <summary>
/// Representa un fragmento coleccionable en el juego, identificado por su color y nivel.
/// Al ser recogido por el jugador, actualiza el estado del juego y se elimina.
/// </summary>
public partial class Fragmento : Area2D
{
    /// <summary>
    /// Color del fragmento. Puede ser "red", "green" o "blue".
    /// Se usa para registrar el tipo de fragmento recogido.
    /// </summary>
    [Export] public string Color;
    /// <summary>
    /// Nombre o identificador del nivel donde se encuentra el fragmento.
    /// Se utiliza para registrar progreso por nivel.
    /// </summary>
    [Export] public string nivel;

    [ExportGroup("Referencias de Nodo")]
    [Export] public Texture2D FragmentTexture;
    [Export] public Sprite2D sprite;
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] private AudioStreamPlayer CollectSound;

    public override void _Ready()
    {
        sprite.Texture = FragmentTexture;

    }

    /// <summary>
    /// Método llamado cuando un cuerpo entra en contacto con el área del fragmento.
    /// Si el cuerpo es un jugador, se registra la recolección y se elimina el fragmento.
    /// </summary>
    /// <param name="body">El nodo que entra en contacto con el fragmento.</param>
    private void OnCollected(Node body)
    {
        if (body is Player)
        {
            sprite.Visible = false; // Ocultar sprite base
            AnimatedSprite.Visible = true; // Asegurarse que esté visible
            AnimatedSprite.Play("Collect");
            CollectSound?.Play();
            GameState.CollectFragment(Color, nivel);
         
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
