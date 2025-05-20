using Godot;
using System;

/// <summary>
/// Cámara 2D personalizada que sigue al jugador con desplazamiento opcional en Y,
/// manteniéndose dentro de los límites definidos para evitar mostrar áreas fuera del nivel.
/// </summary>
public partial class Camera : Camera2D
{
    /// <summary>
    /// Referencia al jugador que la cámara debe seguir.
    /// </summary>
    [Export] private CharacterBody2D player;

    private float leftLimit;
    private float rightLimit;
    private float topLimit;
    private float bottomLimit;

    /// <summary>
    /// Desplazamiento vertical adicional aplicado al objetivo de la cámara.
    /// Permite ajustar la vista del jugador hacia arriba o abajo.
    /// </summary>
    private float yOffset = 0f;


    public override void _Ready()
    {
        base._Ready();
    }
    /// <summary>
    /// Establece los límites del área visible para la cámara.
    /// Estos límites definen la región rectangular que la cámara no debe exceder.
    /// </summary>
    /// <param name="left">Límite izquierdo en coordenadas globales.</param>
    /// <param name="right">Límite derecho en coordenadas globales.</param>
    /// <param name="top">Límite superior en coordenadas globales.</param>
    /// <param name="bottom">Límite inferior en coordenadas globales.</param>
    public void SetLimits(float left, float right, float top, float bottom)
    {
        leftLimit = left;
        rightLimit = right;
        topLimit = top;
        bottomLimit = bottom;
    }

    /// <summary>
    /// Llamado en cada frame. Calcula la posición deseada de la cámara en base a la posición del jugador,
    /// ajustándola por el `yOffset`, y limitándola dentro de los límites establecidos.
    /// </summary>
    /// <param name="delta">Tiempo en segundos desde el último frame.</param>
    public override void _Process(double delta)
    {
        if (player == null) return;

        // Posición objetivo basada en el jugador más desplazamiento vertical
        Vector2 target = player.GlobalPosition + new Vector2(0, yOffset);

        // Tamaño de media pantalla, ajustado por el zoom
        Vector2 halfScreenSize = GetViewportRect().Size / 2 * Zoom;

        // Restringe X dentro de los límites
        float clampedX = Mathf.Clamp(
            target.X,
            leftLimit + halfScreenSize.X,
            rightLimit - halfScreenSize.X
        );

        // Restringe Y dentro de los límites
        float clampedY = Mathf.Clamp(
            target.Y,
            topLimit + halfScreenSize.Y,
            bottomLimit - halfScreenSize.Y
        );

        // Aplica la posición final
        GlobalPosition = new Vector2(clampedX, clampedY);
    }
    
    
    public void SetYOffset(float offset)
    {
        yOffset = offset;
    }
}
