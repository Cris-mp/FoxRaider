using Godot;
using System;

/// <summary>
/// Disparador automático de flechas que instancia flechas en una dirección específica a intervalos regulares.
/// </summary>
public partial class ArrowShooter : Node2D
{
    /// <summary>
    /// Escena preinstanciada de la flecha que será disparada.
    /// </summary> 
    [Export] public PackedScene ArrowScene;
    /// <summary>
    /// Dirección en la que se dispararán las flechas. Debe ser un vector unitario o será normalizado.
    /// </summary>
    [Export] public Vector2 ShootDirection;
    /// <summary>
    /// Temporizador que controla cuándo se dispara una nueva flecha.
    /// </summary>
    [Export] public Timer ShootTimer;
    /// <summary>
    /// Intervalo en segundos entre cada disparo automático.
    /// </summary>
    [Export] public float FireInterval = 1.5f;   

    /// <summary>
    /// Método llamado cuando el nodo entra en la escena. Configura el temporizador de disparo.
    /// </summary>
    public override void _Ready()
    {
        ShootTimer.WaitTime = FireInterval;
    }

    /// <summary>
    /// Método invocado cuando el temporizador alcanza su tiempo de espera. Dispara una flecha.
    /// </summary>
    private void OnShootTimerTimeout()
    {
        ShootArrow();
    }

    /// <summary>
    /// Instancia una nueva flecha y la añade a la escena actual.
    /// </summary>
    private void ShootArrow()
    {
        if (ArrowScene == null)
        {
            GD.PrintErr("ArrowScene no está asignado");
            return;
        }

        var arrow = ArrowScene.Instantiate<Arrow>();
        arrow.Position = GlobalPosition;
        arrow.Direction = ShootDirection.Normalized();
        GetTree().CurrentScene.AddChild(arrow);
    }
}