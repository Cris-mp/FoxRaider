using Godot;
using System;

public partial class DeathZone : Area2D
{
    /// <summary>
    /// Conecta la señal de entrada de cuerpo al inicializar el nodo.
    /// </summary>
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    /// <summary>
    /// Evento que se dispara cuando un cuerpo entra en el área.
    /// Si el cuerpo es el jugador, aplica un daño letal de 6 puntos.
    /// </summary>
    /// <param name="body">Nodo que ha entrado en el área. Se espera que sea un jugador.</param>
    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            player.TakeDamage(6);
        }
    }
}
