using Godot;

public partial class Player : CharacterBody2D
{
    // Parámetros configurables
    [Export] public float Speed = 100f;
    [Export] public float JumpForce = 200f;
    [Export] public float Gravity = 500f;
    [Export] public float RollSpeed = 150f;
    [Export] public float DeathYThreshold = 500f;
    [Export] public float WallClimbSpeed = 50f;
    [Export] public int MaxHealth = 6;

    // Referencias asignadas en el editor
    [Export] public AnimatedSprite2D anim;
    [Export] public Timer rollTimer;
    [Export] public Node audioNode;

    // Audios
    private AudioStreamPlayer2D jumpSound;
    private AudioStreamPlayer2D hitSound;

    // Estado del personaje
    private int currentHealth;
    private bool canDoubleJump = false;
    private bool doubleJump = false;
    private bool canWallGrab = true;
    private bool isRolling = false;
    private bool isDead = false;
    private bool isWallJumping = false;

    public override void _Ready()
    {
        currentHealth = MaxHealth;
        jumpSound = audioNode.GetNode<AudioStreamPlayer2D>("Jump");
        hitSound = audioNode.GetNode<AudioStreamPlayer2D>("Hit");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isDead) return;

        Vector2 velocity = Velocity;
        bool isOnFloor = IsOnFloor();
        bool isOnWall = IsOnWall();
        float direction = Input.GetAxis("move_left", "move_right");

        // Aplicar gravedad
        if (!isOnFloor && !(isOnWall && canWallGrab))
            velocity.Y += Gravity * (float)delta;

        // Movimiento horizontal (excepto si está rodando)
        if (!isRolling)
            velocity.X = direction * Speed;

        // Salto y doble salto
        if (Input.IsActionJustPressed("jump"))
        {
            if (isOnFloor) //Salto normal desde el suelo
            {
                velocity.Y = -JumpForce;
                doubleJump = true;
                jumpSound?.Play();
            }
            else if (!isOnFloor && canDoubleJump && doubleJump) //Permite el doble salto SOLO si la habilidad está desbloqueada y ha echo solo un salto
            {
                velocity.Y = -JumpForce;
                doubleJump = false; //Evita más de un doble salto
                jumpSound?.Play();
            }
            else if (isOnWall && canWallGrab) // Salto en la pared
            {
                velocity.Y = -JumpForce;  // Salto hacia arriba
                velocity.X = direction != 0 ? direction * Speed : -Speed; // Salto hacia la pared
                isWallJumping = true;
                jumpSound?.Play();
            }
        }

        // Agarrarse a la pared (sin escalar automáticamente)
        // Agarrarse a la pared (si la habilidad está desbloqueada)
        if (isOnWall && canWallGrab)
        {
            // Si no se está presionando "arriba" ni "abajo", Foxy se desliza hacia abajo con gravedad controlada
            if (!Input.IsActionPressed("move_up") && !Input.IsActionPressed("move_down"))
            {
                velocity.Y = Gravity * 0.3f; // Deslizarse lentamente hacia abajo
            }

            // Si el jugador presiona "arriba", sube lentamente
            if (Input.IsActionPressed("move_up"))
            {
                velocity.Y = -WallClimbSpeed; // Sube lentamente
            }
            if (!isOnWall)
            {
                isWallJumping = false; //creo que no me entra aqui
            }
        }

        // Rodar (Dash rápido en el suelo)
        if (Input.IsActionJustPressed("roll") && isOnFloor && !isRolling)
        {
            isRolling = true;
            velocity.X = RollSpeed * (direction != 0 ? direction : 1);
            rollTimer.Start();
        }

        // Detectar caída fuera del nivel
        if (GlobalPosition.Y > DeathYThreshold)
        {
            TakeDamage(6); // Pierde toda la vida
        }

        Velocity = new Vector2(Mathf.Round(velocity.X), Mathf.Round(velocity.Y));
        MoveAndSlide();
    }

    public override void _Process(double delta)
    {
        if (isDead) return;

        // Selección de animaciones
        if (isRolling)
            anim.Play("Roll");
        else if (IsOnWall() && canWallGrab)
            anim.Play("Wall_grab"); // 🔹 Nueva animación para agarrarse a la pared
        else if (!IsOnFloor() && !IsOnWall())
        {
            if (Velocity.Y > 0)
                anim.Play("Fall"); // Animación cuando está cayendo
            else
                anim.Play("Jump");
        }
        else if (Velocity.X != 0)
            anim.Play("Run");
        else
            anim.Play("Idle");

        // Voltear sprite según dirección de movimiento
        if (Velocity.X != 0)
            anim.FlipH = Velocity.X < 0;
    }

    // Manejo de daño
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        hitSound?.Play(); // Reproducir sonido de daño
        GD.Print($"Foxy recibió daño, vida actual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Curar a Foxy (opcional)
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
        GetTree().ReloadCurrentScene();
    }

    // Fin de roll (se activa cuando el Timer termina)
    private void _OnRollTimerTimeout()
    {
        isRolling = false;
    }

    // Desbloquear habilidades
    public void UnlockDoubleJump() => canDoubleJump = true;
    public void UnlockWallGrab() => canWallGrab = true;
}
