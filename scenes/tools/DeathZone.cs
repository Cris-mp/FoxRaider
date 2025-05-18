using Godot;
using System;

public partial class DeathZone : Area2D
{ 
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
