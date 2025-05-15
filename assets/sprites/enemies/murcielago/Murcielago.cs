using Godot;
using System;

public partial class Murcielago : Enemies
{
    [Export] public AnimationTree animationTree;
    [Export] public Sprite2D sprite;
    [Export] public Area2D FrontHitbox;
    [Export] public Player player;

    [Export] public float ActivacionDistancia = 100f;
    [Export] public float AlturaDescenso = 32f;
    [Export] public float Velocidad = 50f;

    private Vector2 puntoInicial;
    private Vector2 objetivoAltura;
    private int facingDirection = 1;
    private int direction = 1;

    private enum State { Idle, Bajando, Persiguiendo, Volviendo }
    private State estadoActual = State.Idle;

    public override void _Ready()
    {
        base._Ready();
        puntoInicial = GlobalPosition;
        objetivoAltura = puntoInicial + new Vector2(0, AlturaDescenso);
        SetAnimationState("idle");
    }

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

    private void HandleIdle()
    {
        if (GlobalPosition.DistanceTo(player.GlobalPosition) < ActivacionDistancia)
        {
            estadoActual = State.Bajando;
            SetAnimationState("bajando");
        }
    }

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

    protected override void OnDie()
    {
        SetAnimationState("Dead");
    }

    private void OnFrontHit(Node body)
    {

        if (isDead) return;

        if (body is Player player)
            player.TakeDamage(1);
    }

    private void OnAnimationFinished(StringName animName)
    {


        if (isDead && animName == "Dead")
            QueueFree();
    }

    private void SetAnimationState(string stateName)
    {
        var stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        if (stateMachine.GetCurrentNode() != stateName)
            stateMachine.Travel(stateName);
    }

   private void ChangeDirection()
{
    direction *= -1;
        Scale = new Vector2(-Scale.X, Scale.Y); // Invertir sprite horizontalmente
    
}
}
