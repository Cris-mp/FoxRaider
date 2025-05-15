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
    [Export] public CollisionShape2D Collision;
    [Export] public CollisionShape2D CollisionRoll;
    [Export] public Area2D HitPoint;

    // Audios
    private AudioStreamPlayer jumpSound;
    private AudioStreamPlayer hitSound;

    // Estado del personaje
    private int currentHealth;
    private bool canDoubleJump = true;//GameState.HasDoubleJump;
    private bool doubleJump = false;
    private bool canWallGrab = GameState.HasWallGrab;
    private bool isRolling = false;
    private bool isDead = false;

    // Límites de movimiento
    public float LeftLimit;
    public float RightLimit;

    private Vector2 respawnPoint;

    [Signal] public delegate void HealthChangedEventHandler(int newHealth);

    public override void _Ready()
    {
        currentHealth = MaxHealth;
        respawnPoint = GlobalPosition;
        jumpSound = audioNode.GetNode<AudioStreamPlayer>("Jump");
        hitSound = audioNode.GetNode<AudioStreamPlayer>("Hit");
        HitPoint.AreaEntered += OnHitEnemy;
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
            if (isOnFloor) // Salto normal desde el suelo
            {
                velocity.Y = -JumpForce;
                doubleJump = true;
                jumpSound?.Play();
            }
            else if (canDoubleJump && doubleJump) // Doble salto si la habilidad está desbloqueada
            {
                velocity.Y = -JumpForce;
                doubleJump = false;
                jumpSound?.Play();
            }
            else if (isOnWall && canWallGrab) // Salto en la pared
            {
                velocity.Y = -JumpForce; // Salto hacia arriba

                if (anim.FlipH) // Si está mirando a la izquierda, salta hacia la derecha
                    velocity.X = Speed;
                else // Si está mirando a la derecha, salta hacia la izquierda
                    velocity.X = -Speed;
                canWallGrab = false; // Desactivar agarre momentáneamente para evitar que se pegue a la pared tras saltar
                GetTree().CreateTimer(0.2f).Timeout += () => canWallGrab = true; // Reactivar después de 0.2 segundos

                jumpSound?.Play();
            }
        }

        // Agarrarse a la pared (sin escalar automáticamente)
        // Agarrarse a la pared (si la habilidad está desbloqueada)
        if (isOnWall && canWallGrab)
        {
            if (!Input.IsActionPressed("move_up") && !Input.IsActionPressed("move_down"))
            {
                velocity.Y = Gravity * 0.1f; // Deslizarse lentamente hacia abajo
            }

            // Si el jugador presiona "arriba", sube lentamente
            if (Input.IsActionPressed("move_up"))
            {
                velocity.Y = -WallClimbSpeed; // Sube lentamente
            }

        }

        // Rodar (Dash rápido en el suelo)
        if (Input.IsActionJustPressed("roll") && isOnFloor && !isRolling)
        {
            isRolling = true;
            velocity.X = RollSpeed * (direction != 0 ? direction : 1);
            Collision.Disabled = true;  // Desactiva la colisión grande
            CollisionRoll.Disabled = false;   // Activa la colisión pequeña
            rollTimer.Start();
        }

        // Detectar caída fuera del nivel
        if (GlobalPosition.Y > DeathYThreshold)
        {
            TakeDamage(6); // Pierde toda la vida
        }

        Vector2 position = GlobalPosition;

        // Limitar a Foxy dentro de los bordes del nivel
        position.X = Mathf.Clamp(position.X, LeftLimit, RightLimit);

        GlobalPosition = position;

        Velocity = new Vector2(Mathf.Round(velocity.X), Mathf.Round(velocity.Y));
        MoveAndSlide();
    }

    public void SetLimits(float left, float right)
    {
        LeftLimit = left;
        RightLimit = right;
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
        EmitSignal(SignalName.HealthChanged, currentHealth);
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
        GlobalPosition = respawnPoint;
        currentHealth = MaxHealth;
        EmitSignal(SignalName.HealthChanged, currentHealth);
        isDead = false;
        anim.Play("Idle");
    }

    // Fin de roll (se activa cuando el Timer termina)
    private void _OnRollTimerTimeout()
    {
        isRolling = false;

        // 🔹 Restaurar colisión (grande activa, pequeña desactiva)
        Collision.Disabled = false;
        CollisionRoll.Disabled = true;
    }

    private void OnHitEnemy(Area2D area)
    {
        if (area.Owner is Enemies enemy && !enemy.IsDead())
        {
            enemy.TakeDamage(1);

            // Rebote del jugador al pisar
            Velocity = new Vector2(Velocity.X, -JumpForce / 1.5f); // Rebote más suave
        }
    }

    public void SetRespawnPoint(Vector2 position)
    {
        respawnPoint = position;
    }
}
