using Godot;

/// <summary>
/// Nodo de área que actúa como disparador de cambio de nivel.
/// Al detectar al jugador, puede desbloquear habilidades, registrar progreso
/// y cargar una nueva escena según lo configurado en el editor.
/// </summary>
public partial class TriggerCambioNivel : Area2D
{
    /// <summary>
    /// Ruta a la escena que se cargará cuando el jugador entre en el trigger.
    /// Debe ser un archivo `.tscn`.
    /// </summary>
    [Export(PropertyHint.File, "*.tscn")]
    public string ScenePath = "";

    [ExportGroup("Habilidades desbloqueanbles")]
    /// <summary>
    /// Si está activado, se desbloqueará la habilidad de doble salto al entrar.
    /// </summary>
    [Export] public bool desbloquearDobleSalto = false;

    /// <summary>
    /// Si está activado, se desbloqueará la habilidad de agarrarse a paredes al entrar.
    /// </summary>
    [Export] public bool desbloquearPared = false;

    [ExportGroup("Nivel")]
    /// <summary>
    /// Nivel que se marcará como desbloqueado si aún no lo está.
    /// </summary>
    [Export] public int NivelDesbloquear = 1;
    
    /// <summary>
    /// Evento que se dispara cuando un cuerpo entra en el área.
    /// Si el cuerpo es el jugador, se aplican mejoras y se inicia el cambio de escena.
    /// </summary>
    /// <param name="body">Nodo que ha entrado en el área. Se espera que sea un jugador.</param>
    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            if (desbloquearDobleSalto)
                GameState.HasDoubleJump = true;

            if (desbloquearPared)
                GameState.HasWallGrab = true;

            if (!string.IsNullOrEmpty(ScenePath))
            {
                CallDeferred(nameof(CambiarEscena));
            }
            else
            {
                GD.PrintErr("⚠️ No se ha asignado la ruta de la escena al trigger.");
            }
        }
    }
    /// <summary>
    /// Lógica que cambia a una nueva escena, guarda el progreso del jugador,
    /// y actualiza el nivel máximo desbloqueado si es necesario.
    /// </summary>
    private void CambiarEscena()
    {
        if (NivelDesbloquear > GameState.MaxLevelUnlocked)
        {
            GameState.MaxLevelUnlocked = NivelDesbloquear;
        }

        GameState.SaveGame();

        var scene = ResourceLoader.Load<PackedScene>(ScenePath);

        if (scene != null)
        {
            GetTree().ChangeSceneToPacked(scene);
        }
        else
        {
            GD.PrintErr($"❌ Error al cargar la escena desde: {ScenePath}");
        }
    }
}

