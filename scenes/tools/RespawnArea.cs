using Godot;
using System;

public partial class RespawnArea : Area2D
{  
    /// <summary>
    /// Evento que se dispara cuando un cuerpo entra en el área.
    /// Si el cuerpo es el jugador, se guarda la posicion del jugador como punto de respawn.
    /// </summary>
    /// <param name="body">Nodo que ha entrado en el área. Se espera que sea un jugador.</param>
    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            player.SetRespawnPoint(GlobalPosition);
        }
    }
}
