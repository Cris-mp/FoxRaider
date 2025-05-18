using Godot;
using System;
using System.Threading.Tasks;

/// <summary>
/// Plataforma que cae suavemente al ser pisada por el jugador.
/// Después de un retardo opcional, vibra (efecto visual) antes de caer.
/// Posteriormente, se reinicia y vuelve a su posición original tras cierto tiempo.
/// </summary>
public partial class FallingPlatform : Node2D
{
    [ExportGroup("Tiempos de Espera")]
    /// <summary>
    /// Tiempo que la plataforma espera antes de empezar a vibrar y caer, tras ser pisada.
    /// </summary>
    [Export] public float FallDelay = 0.5f;

    /// <summary>
    /// Tiempo que tarda en reaparecer la plataforma tras haber caído.
    /// </summary>
    [Export] public float RespawnDelay = 3f;

    /// <summary>
    /// Tiempo que tarda la plataforma en caer, desde que inicia la animación.
    /// </summary>
    [Export] public float FallDuration = 0.5f;

    [ExportGroup("Distancias")]
    /// <summary>
    /// Distancia vertical a la que caerá la plataforma.
    /// </summary>
    [Export] public float FallDistance = 100f;

    [ExportGroup("Referencias de Nodos")]
    [Export] private CharacterBody2D platformBody;
    [Export] private CollisionShape2D collisionMain;
    [Export] private CollisionShape2D collisionSensor;
    [Export] private Sprite2D sprite;
    [Export] private Timer respawnTimer;

    /// <summary>
    /// Posición inicial de la plataforma, guardada al iniciar la escena.
    /// </summary>
    private Vector2 startPosition;

    /// <summary>
    /// Bandera para evitar múltiples activaciones simultáneas de la caída.
    /// </summary>
    private bool activada = false;

    public override void _Ready()
    {
        startPosition = platformBody.GlobalPosition;
    }

    /// <summary>
    /// Llamado cuando un cuerpo entra en el área de detección.
    /// Si el cuerpo es el jugador, se inicia la caída.
    /// </summary>
    /// <param name="body">Cuerpo que ha entrado en contacto con el sensor.</param>
    private async void OnBodyEntered(Node body)
    {
        // Prevenir múltiples activaciones
        if (activada) return;
        activada = true;

        // Validar que el cuerpo es el jugador o un nodo hijo de este
        if (body is not Player)
            return;

        // Esperar antes de comenzar a vibrar y caer
        await ToSignal(GetTree().CreateTimer(FallDelay), "timeout");

        // Animación de vibración previa a la caída
        await VibrarAntesDeCaer();

        // Desactivar colisiones para dejar caer al jugador
        collisionMain.Disabled = true;
        collisionSensor.Disabled = true;

        // Calcular posición de destino
        var endPosition = startPosition + new Vector2(0, FallDistance);

        // Crear tween de caída
        var tween = GetTree().CreateTween();
        tween.TweenProperty(
            platformBody,
            "global_position",
            endPosition,
            FallDuration
        );

        // Esperar a que termine la caída
        await ToSignal(tween, "finished");
        // Ocultar sprite tras la caída
        sprite.Visible = false;
        // Iniciar el temporizador para el respawn
        respawnTimer.Start(RespawnDelay);
    }

    //// <summary>
    /// Restaura la plataforma a su posición original, reactivando colisiones y visibilidad.
    /// </summary>
    private void OnRespawn()
    {
        platformBody.GlobalPosition = startPosition;
        collisionMain.Disabled = false;
        collisionSensor.Disabled = false;
        sprite.Visible = true;
        activada = false;
    }

    /// <summary>
    /// Realiza una animación rápida de vibración antes de que la plataforma caiga.
    /// Mueve el cuerpo levemente hacia arriba y abajo repetidamente.
    /// </summary>
    private async Task VibrarAntesDeCaer()
    {
        var vibracionTween = GetTree().CreateTween();
        int repeticiones = 4;
        float amplitud = 1f;
        float duracion = 0.05f;

        for (int i = 0; i < repeticiones; i++)
        {
            vibracionTween.TweenProperty(
                platformBody,
                "position:y",
                platformBody.Position.Y - amplitud,
                duracion
            );
            vibracionTween.TweenProperty(
                platformBody,
                "position:y",
                platformBody.Position.Y + amplitud,
                duracion
            );
        }

        vibracionTween.TweenProperty(
            platformBody,
            "position:y",
            0,
            duracion
        );

        await ToSignal(vibracionTween, "finished");
    }
}
