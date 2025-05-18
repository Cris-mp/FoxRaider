using Godot;
using System;

/// <summary>
/// Script del nodo raíz de la escena "Bosque1Fase1".
/// Se encarga de inicializar referencias entre el jugador, la cámara y el HUD,
/// estableciendo límites y conectando eventos necesarios.
/// </summary>
public partial class Bosque1Fase1 : Node2D
{
    [ExportGroup("Referencias de Nodo")]
    [Export] private Player player;
    [Export] private Camera camera;
    [Export] private Hud hud;

    /// <summary>
    /// Método llamado cuando el nodo entra en la escena.
    /// Conecta el evento de salud del jugador con el HUD y configura
    /// los límites tanto para el jugador como para la cámara.
    /// </summary>
    public override void _Ready()
    {
        // Conectar el evento de cambio de vida al HUD
        player.HealthChanged += hud.UpdateLife;
        // Establecer los límites del movimiento horizontal del jugador
        player.SetLimits(0f, 2562f);

        // Establecer los límites del área visible para la cámara
        camera.SetLimits(0f, 2562f, -500f, 500f);
    }
}
