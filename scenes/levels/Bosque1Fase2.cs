using Godot;
using System;

// <summary>
/// Script del nodo raíz de la escena "Bosque1Fase2".
/// Se encarga de inicializar referencias entre el jugador, la cámara y el HUD,
/// estableciendo límites y conectando eventos necesarios.
/// </summary>
public partial class Bosque1Fase2 : Node2D
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
        player.HealthChanged += hud.UpdateLife;
        player.SetLimits(0f, 2562f);

        camera.SetLimits(0f, 1344f, -1040f, 280f);
    }
}
