using Godot;

/// <summary>
/// Clase base para enemigos. Maneja salud, gravedad y lógica de muerte.
/// </summary>
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
	/// <summary>Aplica daño al enemigo. Llama a OnDie y Die si la vida llega a cero.</summary>
	/// <param name="amount">Cantidad de daño.</param>
	public virtual void TakeDamage(int amount)
	{
		if (isDead) return;

		currentHealth -= amount;

		if (currentHealth <= 0)
		{
			isDead = true;
			OnDie();
			Die();
		}
	}
	/// <summary>Desactiva la física al morir.</summary>
	protected virtual void Die()
	{
		isDead = true;
		SetPhysicsProcess(false);
	}
	/// <summary>Método sobrescribible llamado cuando el enemigo muere (por animaciones, efectos, etc).</summary>
	protected virtual void OnDie() { }

	/// <summary>Devuelve si el enemigo está muerto.</summary>
	public bool IsDead() => isDead;
}
