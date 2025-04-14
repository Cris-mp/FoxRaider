using Godot;

public partial class Zarigueya : Enemies
{
    [Export] public float Speed = 40f;
    [Export] public AnimationTree animationTree;
    [Export] public RayCast2D WallCheck;
    [Export] public RayCast2D GroundCheck;
    [Export] public Area2D FrontHitbox;

    private int direction = 1;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isDead) return;

        Vector2 velocity = Velocity;

        velocity.Y = IsOnFloor() ? 0 : velocity.Y + Gravity * (float)delta;
        velocity.X = direction * -Speed;

        Velocity = velocity;

        if (WallCheck.IsColliding() || !GroundCheck.IsColliding())
            ChangeDirection();

        MoveAndSlide();
        SetAnimationState("Run");
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
