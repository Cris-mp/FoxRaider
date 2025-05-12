using Godot;
using System;

public partial class Murcielago : Enemies
{

    [Export] public AnimationTree animationTree;
    [Export] public Area2D FrontHitbox;

    [Export] public float ActivacionDistancia = 100f;
    [Export] public float AlturaDescenso = 32f;
    [Export] public float Velocidad = 50f;
    [Export] public Player player;

    private AnimationTree animTree;
    private AnimationNodeStateMachinePlayback animState;

    private Vector2 puntoInicial;
    private Vector2 objetivoAltura;
   

    private int direction = 1;
    public override void _Ready()
    {
        base._Ready();
        puntoInicial = GlobalPosition;
        objetivoAltura = puntoInicial + new Vector2(0, AlturaDescenso);       
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isDead) return;
        Vector2 velocity = Velocity;

        if (GlobalPosition.DistanceTo(player.GlobalPosition) < ActivacionDistancia)
        {
            SetAnimationState("bajando");
            velocity.Y = Velocidad;
            if (GlobalPosition.Y >= objetivoAltura.Y)
            {
                GlobalPosition = new Vector2(GlobalPosition.X, objetivoAltura.Y);
                velocity.Y = 0;
                SetAnimationState("fly");
                float dir = Math.Sign(player.GlobalPosition.X - GlobalPosition.X);
                velocity.X = dir * Velocidad;

                // Si se aleja demasiado, volver
                if (GlobalPosition.DistanceTo(player.GlobalPosition) > ActivacionDistancia * 2)
                {
                    velocity = (puntoInicial - GlobalPosition).Normalized() * Velocidad;
                    if (GlobalPosition.DistanceTo(puntoInicial) < 4f)
                    {
                        GlobalPosition = puntoInicial;
                        velocity = Vector2.Zero;
                        SetAnimationState("idle");
                    }
                }
            }
        }
        Velocity = velocity;
        MoveAndSlide();
    }

    protected override void OnDie()
    {
        SetAnimationState("Muerto");
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