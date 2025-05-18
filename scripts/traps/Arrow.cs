using Godot;
using System;

// <summary>
/// Representa una flecha que se mueve en una dirección específica y puede dañar al jugador.
/// </summary>
public partial class Arrow : Area2D
{
    [Export] public float Speed = 400f;
    public Vector2 Direction = Vector2.Right;

    /// <summary>
    /// Se llama cada frame de física. Mueve la flecha en la dirección especificada y ajusta su rotación.
    /// </summary>
    /// <param name="delta">Tiempo transcurrido desde el último frame (en segundos).</param>
    public override void _PhysicsProcess(double delta)
    {
        Position += Direction * Speed * (float)delta;
        Rotation = Direction.Angle();
    }

    /// <summary>
    /// Se llama cuando la flecha entra en contacto con un cuerpo (como el jugador o una capa del escenario).
    /// </summary>
    /// <param name="body">El nodo con el que colisionó la flecha.</param>
    public void OnBodyEntered(Node body)
    {
        if (body.Name == "Layer 0")
        {
            QueueFree();
        }
        if (body is Player)
        {
            ((Player)body).TakeDamage(1);
            QueueFree();
        }
    }


}

