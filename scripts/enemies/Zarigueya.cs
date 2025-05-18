using Godot;

// <summary>
/// Enemigo que camina de lado y cambia de dirección al detectar paredes o bordes.
/// </summary>
public partial class Zarigueya : Enemies
{
    [Export] public float Speed = 40f;

    [ExportGroup("Referencias de Nodo")]
    [Export] public AnimationTree animationTree;
    [Export] public Sprite2D mySprite;
    [Export] public RayCast2D WallCheck;
    [Export] public RayCast2D GroundCheck;
    [Export] public Area2D FrontHitbox;
    [Export] private AudioStreamPlayer DeathSound;

    private int direction = 1;

    /// <summary>
    /// Ejecuta la lógica de movimiento.
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        if (isDead) return;

        Vector2 velocity = Velocity;

        // Aplicar gravedad si no está en el suelo
        velocity.Y = IsOnFloor() ? 0 : velocity.Y + Gravity * (float)delta;
        // Movimiento horizontal
        velocity.X = direction * -Speed;
        Velocity = velocity;

        MoveAndSlide();

        // Cambio de dirección si detecta pared o si no hay suelo
        if (WallCheck.IsColliding() || !GroundCheck.IsColliding())
            ChangeDirection();

        // Si colisiona con otro enemigo, también cambia de dirección
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            if (collision.GetCollider() is Enemies enemy && enemy != this)
            {
                ChangeDirection();
                break;
            }
        }
        SetAnimationState("Run");
    }

    /// <summary>Cambia la animación al estado "Dead" al morir.</summary>
    protected override void OnDie()
    {
        DeathSound?.Play();
        SetAnimationState("Dead");
    }

    /// <summary>Inflige daño al jugador si lo toca desde el frente y cambia de dirección.</summary>
    private void OnFrontHit(Node body)
    {
        if (isDead) return;

        if (body is Player player)
        {
            player.TakeDamage(1);
            ChangeDirection();
        }
    }

    /// <summary>Libera el nodo después de que termina la animación de muerte.</summary>
    private void OnAnimationFinished(StringName animName)
    {
        if (isDead && animName == "Dead")
            QueueFree();
    }

    /// <summary>Cambia el estado del árbol de animación si es diferente al actual.</summary>
    private void SetAnimationState(string stateName)
    {
        var stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

        if (stateMachine.GetCurrentNode() != stateName)
            stateMachine.Travel(stateName);
    }

    /// <summary>Cambia la dirección del movimiento y voltea el sprite visualmente.</summary>
    private void ChangeDirection()
    {
        direction *= -1;
        Scale = new Vector2(-Scale.X, Scale.Y);
    }
}
