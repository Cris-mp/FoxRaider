using Godot;
using System;

/// <summary>
/// Representa un obstáculo tipo pincho que daña al jugador al tocarlo.
/// Ajusta su colisión automáticamente al tamaño de su región.
/// </summary>
public partial class Spike : Sprite2D
{    
    [Export] public CollisionShape2D shape;

    /// <summary>
    /// Se ejecuta al entrar en la escena. Crea una forma de colisión rectangular
    /// basada en el tamaño de la región del sprite.
    /// </summary>
    public override void _Ready()
    {
        RectangleShape2D rectShape = new RectangleShape2D();
        rectShape.Size = this.RegionRect.Size;
        shape.Shape = rectShape;
    }

    /// <summary>
    /// Se ejecuta cuando un cuerpo entra en contacto con el área del pincho.
    /// Si el cuerpo es el jugador, le inflige daño letal.
    /// </summary>
    /// <param name="body">El nodo que entró en contacto con el pincho.</param>
    public void OnBodyEntered(Node body)
    {
        if (body is Player)
        {
            ((Player)body).TakeDamage(6);
        }
    }
}
