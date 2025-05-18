using Godot;
using System;

/// <summary>
/// Murciélago enemigo que permanece inactivo hasta que el jugador se acerca,
/// desciende, persigue y luego regresa a su posición inicial.
/// </summary>
public partial class Murcielago : Enemies
{
    [ExportGroup("Referencias de Nodo")]
    [Export] public AnimationTree animationTree;
    [Export] public Sprite2D sprite;
    [Export] public Area2D FrontHitbox;
    [Export] public Player player;
    [Export] private AudioStreamPlayer DeathSound;

    [ExportGroup("Movimiento")]
    [Export] public float ActivacionDistancia = 100f;
    [Export] public float AlturaDescenso = 32f;
    [Export] public float Velocidad = 50f;

    private Vector2 puntoInicial;
    private Vector2 objetivoAltura;
    private int facingDirection = 1;
    private int direction = 1;

    /// <summary>Estados posibles del murciélago.</summary>
    private enum State { Idle, Bajando, Persiguiendo, Volviendo }
    private State estadoActual = State.Idle;

    public override void _Ready()
    {
        base._Ready();
        puntoInicial = GlobalPosition;
        objetivoAltura = puntoInicial + new Vector2(0, AlturaDescenso);
        SetAnimationState("idle");
    }

    /// <summary>
    /// Ejecuta la lógica de movimiento y cambio de estado cada frame de física.
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        if (isDead) return;

        switch (estadoActual)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Bajando:
                HandleBajando(delta);
                break;
            case State.Persiguiendo:
                HandlePersiguiendo(delta);
                break;
            case State.Volviendo:
                HandleVolver(delta);
                break;
        }

        MoveAndSlide();
    }

    /// <summary>
    /// Estado inactivo: espera que el jugador se acerque.
    /// </summary>
    private void HandleIdle()
    {
        if (GlobalPosition.DistanceTo(player.GlobalPosition) < ActivacionDistancia)
        {
            estadoActual = State.Bajando;
            SetAnimationState("bajando");
        }
    }

    /// <summary>
    /// Baja suavemente hasta una altura determinada antes de comenzar la persecución.
    /// </summary>
    /// <param name="delta">Tiempo del frame actual.</param>
    private void HandleBajando(double delta)
    {
        Vector2 velocity = Velocity;
        velocity.Y += Velocidad * (float)delta;

        if (GlobalPosition.Y >= objetivoAltura.Y)
        {
            GlobalPosition = new Vector2(GlobalPosition.X, objetivoAltura.Y);
            velocity.Y = 0;

            estadoActual = State.Persiguiendo;
            SetAnimationState("fly");
        }

        Velocity = velocity;
    }

    /// <summary>
    /// Persigue al jugador si está dentro del rango. Cambia la dirección visual si es necesario.
    /// </summary>
    /// <param name="delta">Tiempo del frame actual.</param>
    private void HandlePersiguiendo(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 toPlayer = player.GlobalPosition - GlobalPosition;

        if (toPlayer.Length() > ActivacionDistancia * 2)
        {
            estadoActual = State.Volviendo;
            SetAnimationState("fly");
            return;
        }

        float dirX = toPlayer.X;

        // Umbral para evitar microajustes cuando el jugador está casi encima
        if (Mathf.Abs(dirX) > 2f)
        {
            int newFacing = (int)Mathf.Sign(dirX);
            velocity.X = newFacing * Velocidad;

            if (newFacing != facingDirection)
            {
                facingDirection = newFacing;
                ChangeDirection();
            }
        }
        else
        {
            velocity.X = 0;
        }

        Velocity = velocity;
    }

    /// <summary>
    /// Vuelve a la posición inicial si se aleja demasiado del jugador.
    /// </summary>
    /// <param name="delta">Tiempo del frame actual.</param>
    private void HandleVolver(double delta)
    {
        Vector2 toStart = puntoInicial - GlobalPosition;

        if (toStart.Length() < 4f)
        {
            GlobalPosition = puntoInicial;
            Velocity = Vector2.Zero;
            estadoActual = State.Idle;
            SetAnimationState("idle");
            return;
        }

        Vector2 velocity = toStart.Normalized() * Velocidad;
        Velocity = velocity;

        // Forzar flip visual incluso con pequeños valores de X
        if (Mathf.Abs(toStart.X) > 0.01f) // no uses velocity.X, usa toStart.X directamente
        {
            int newFacing = (int)Mathf.Sign(toStart.X);

            if (newFacing != facingDirection)
            {
                facingDirection = newFacing;
                ChangeDirection();
            }
        }
    }

    /// <summary>
    /// Lógica que se ejecuta cuando el murciélago muere.
    /// </summary>
    protected override void OnDie()
    {
        DeathSound?.Play();
        SetAnimationState("Dead");
    }

    /// <summary>
    /// Daña al jugador si entra en contacto con la hitbox frontal del murciélago.
    /// </summary>
    /// <param name="body">Nodo con el que colisionó.</param>
    private void OnFrontHit(Node body)
    {

        if (isDead) return;

        if (body is Player player)
            player.TakeDamage(1);
    }

    /// <summary>
    /// Elimina al murciélago de la escena cuando la animación de muerte termina.
    /// </summary>
    /// <param name="animName">Nombre de la animación que ha finalizado.</param>
    private void OnAnimationFinished(StringName animName)
    {


        if (isDead && animName == "Dead")
            QueueFree();
    }

    /// <summary>
    /// Cambia el estado actual del árbol de animaciones.
    /// </summary>
    /// <param name="stateName">Nombre del estado de animación.</param>
    private void SetAnimationState(string stateName)
    {
        var stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        if (stateMachine.GetCurrentNode() != stateName)
            stateMachine.Travel(stateName);
    }

    /// <summary>
    /// Cambia la dirección del sprite (horizontalmente).
    /// </summary>
    private void ChangeDirection()
    {
        direction *= -1;
        Scale = new Vector2(-Scale.X, Scale.Y); // Invertir sprite horizontalmente
    }
}
