using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    #region === CONFIGURACIÓN EXPORTADA ===
    [ExportGroup("Movimiento")]
    [Export] public float Speed = 100f;
    [Export] public float JumpForce = 200f;
    [Export] public float Gravity = 500f;
    [Export] public float RollSpeed = 150f;
    [Export] public float WallClimbSpeed = 50f;

    [ExportGroup("Muerte y Vida")]
    [Export] public int MaxHealth = 6;
    [Export] public float DeathYThreshold = 500f;

    [ExportGroup("Límites del Nivel")]
    [Export] public float LeftLimit;
    [Export] public float RightLimit;

    [ExportGroup("Referencias de Nodo")]
    [Export] public AnimatedSprite2D anim;
    [Export] public Timer rollTimer;
    [Export] public AudioStreamPlayer jumpSound;
    [Export] public AudioStreamPlayer hitSound;
    [Export] public CollisionShape2D Collision;
    [Export] public CollisionShape2D CollisionRoll;
    [Export] public Area2D HitPoint;
    #endregion

    #region === ESTADO INTERNO ===
    private int currentHealth;
    private bool doubleJump = false;
    private bool isRolling = false;
    private bool isDead = false;
    private bool wasOnFloor = false;
    private Vector2 velocity;

    // Habilidades desbloqueables
    private bool canDoubleJump = GameState.HasDoubleJump;
    private bool canWallGrab = GameState.HasWallGrab;

    private Vector2 respawnPoint;

    private float moveDirection;
    private bool jumpPressed;
    private bool rollPressed;
    #endregion

    #region === SEÑALES ===
    [Signal] public delegate void HealthChangedEventHandler(int newHealth);
    #endregion

    #region === READY ===
    public override void _Ready()
    {
        currentHealth = MaxHealth;
        respawnPoint = GlobalPosition;
        HitPoint.AreaEntered += OnHitEnemy;
    }
    #endregion

    #region === BUCLE PRINCIPAL ===
    public override void _PhysicsProcess(double delta)
    {
        if (isDead) return;

        ReadInput();
        UpdateGroundState();
        ApplyGravity(delta);
        HandleMovement();
        HandleJump();
        HandleWallGrab();
        HandleRoll();
        CheckDeathZone();
        ClampToLevelBounds();
        ApplyVelocity();
    }

    public override void _Process(double delta)
    {
        if (isDead) return;
        UpdateAnimation();
    }
    #endregion

    #region === ENTRADA ===
    private void ReadInput()
    {
        moveDirection = Input.GetAxis("move_left", "move_right");
        jumpPressed = Input.IsActionJustPressed("jump");
        rollPressed = Input.IsActionJustPressed("roll");
    }
    #endregion

    #region === MOVIMIENTO Y GRAVEDAD ===
    private void UpdateGroundState()
    {
        if (IsOnFloor() && !wasOnFloor)
            doubleJump = true;

        wasOnFloor = IsOnFloor();
    }

    private void ApplyGravity(double delta)
{
    if (!IsOnFloor() && !(IsOnWall() && canWallGrab))
    {      

        velocity.Y += Gravity * (float)delta;

        // Limitar la velocidad de caída máxima
        float maxFallSpeed = 800f;
        velocity.Y = Mathf.Min(velocity.Y, maxFallSpeed);
    }
}

    private void HandleMovement()
    {
        if (!isRolling)
            velocity.X = moveDirection * Speed;
    }

    private void ApplyVelocity()
    {
        Velocity = new Vector2(Mathf.Round(velocity.X), Mathf.Round(velocity.Y));
        MoveAndSlide();
    }
    #endregion

    #region === SALTO Y PARED ===
    private void HandleJump()
    {
        if (!jumpPressed) return;

        if (IsOnFloor())
        {
            velocity.Y = -JumpForce;
            doubleJump = true;
            jumpSound?.Play();
        }
        else if (canDoubleJump && doubleJump)
        {
            velocity.Y = -JumpForce;
            doubleJump = false;
            jumpSound?.Play();
        }
        else if (IsOnWall() && canWallGrab)
        {
            velocity.Y = -JumpForce;
            velocity.X = anim.FlipH ? Speed : -Speed;

            canWallGrab = false;
            GetTree().CreateTimer(0.2f).Timeout += () => canWallGrab = true;

            jumpSound?.Play();
        }
    }

    private void HandleWallGrab()
    {
        if (!IsOnWall() || !canWallGrab) return;

        if (!Input.IsActionPressed("move_up") && !Input.IsActionPressed("move_down"))
        {
            velocity.Y = Gravity * 0.1f;
        }

        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y = -WallClimbSpeed;
        }
    }
    #endregion

    #region === ROLL / DESLIZAMIENTO ===
    private void HandleRoll()
    {
        if (!rollPressed || isRolling || !IsOnFloor()) return;

        isRolling = true;
        velocity.X = RollSpeed * (moveDirection != 0 ? moveDirection : 1);
        Collision.Disabled = true;
        CollisionRoll.Disabled = false;
        rollTimer.Start();
    }

    private void _OnRollTimerTimeout()
    {
        isRolling = false;

        // Intentar restaurar la colisión grande solo si hay espacio
        CollisionRoll.Disabled = true;
        Collision.Disabled = false;

        if (TestMove(Transform, Vector2.Zero))
        {
            Collision.Disabled = true;
            CollisionRoll.Disabled = false;
        }
    }
    #endregion

    #region === LÍMITES DEL NIVEL ===
    private void ClampToLevelBounds()
    {
        Vector2 position = GlobalPosition;
        position.X = Mathf.Clamp(position.X, LeftLimit, RightLimit);
        GlobalPosition = position;
    }

    private void CheckDeathZone()
    {
        if (GlobalPosition.Y > DeathYThreshold)
        {
            TakeDamage(MaxHealth); // muerte instantánea
        }
    }

    public void SetLimits(float left, float right)
    {
        LeftLimit = left;
        RightLimit = right;
    }
    #endregion

    #region === ANIMACIONES ===
    private void UpdateAnimation()
    {
        if (isRolling)
            anim.Play("Roll");
        else if (IsOnWall() && canWallGrab)
            anim.Play("Wall_grab");
        else if (!IsOnFloor())
            anim.Play(velocity.Y > 0 ? "Fall" : "Jump");
        else if (velocity.X != 0)
            anim.Play("Run");
        else
            anim.Play("Idle");

        if (velocity.X != 0)
            anim.FlipH = velocity.X < 0;
    }
    #endregion

    #region === DAÑO Y VIDA ===
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        hitSound?.Play();
        EmitSignal(SignalName.HealthChanged, currentHealth);
        GD.Print($"Foxy recibió daño, vida actual: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, MaxHealth);
        GD.Print($"Foxy se curó, vida actual: {currentHealth}");
    }

    private void Die()
    {
        isDead = true;
        Velocity = Vector2.Zero;
        anim.Play("Death");
        GetTree().CreateTimer(1.5f).Timeout += GameOver;
    }

    private void GameOver()
    {
        GlobalPosition = respawnPoint;
        currentHealth = MaxHealth;
        EmitSignal(SignalName.HealthChanged, currentHealth);
        isDead = false;
        anim.Play("Idle");
    }
    #endregion

    #region === ENEMIGOS ===
    private void OnHitEnemy(Area2D area)
    {
        if (area.Owner is Enemies enemy && !enemy.IsDead())
        {
            enemy.TakeDamage(1);
            velocity.Y = -JumpForce / 1.5f; // Rebote suave
        }
    }
    #endregion

    #region === CHECKPOINT ===
    public void SetRespawnPoint(Vector2 position)
    {
        respawnPoint = position;
    }
    #endregion
}
