using Godot;

public partial class GameState : Node
{
    public static bool HasDoubleJump { get; set; } = false;
    public static bool HasWallGrab { get; set; } = false;

    public string LastLevelPath { get; set; } = "res://scenes/levels/bosque1_fase1.tscn";

    public void ResetProgress()
    {
        HasDoubleJump = false;
        HasWallGrab = false;
        LastLevelPath = "res://scenes/levels/bosque1_fase1.tscn";
    }
}

