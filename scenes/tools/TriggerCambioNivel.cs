using Godot;

public partial class TriggerCambioNivel : Area2D
{
    [Export(PropertyHint.File, "*.tscn")]
    public string ScenePath = "";

    [Export] public bool desbloquearDobleSalto = false;
    [Export] public bool desbloquearPared = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

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

    private void CambiarEscena()
    {
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

