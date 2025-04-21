using Godot;

public partial class ZonaControl : Area2D
{
    [Export] public TileMapLayer tilemapTarget;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player && tilemapTarget != null)
        {
            tilemapTarget.Visible = false;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Player && tilemapTarget != null)
        {
            tilemapTarget.Visible = true;
        }
    }
}

