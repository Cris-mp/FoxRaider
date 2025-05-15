using Godot;

public partial class Enemies : CharacterBody2D
{
    [Export] public int MaxHealth = 1;
    [Export] public float Gravity = 500f;

    protected int currentHealth;
    protected bool isDead = false;

    public override void _Ready()
    {
        currentHealth = MaxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            isDead = true;
            OnDie(); // Hook para clases hijas
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        SetPhysicsProcess(false);
    }
    protected virtual void OnDie() { }

    public bool IsDead() => isDead;
}


