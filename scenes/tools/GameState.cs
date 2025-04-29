using Godot;

public partial class GameState : Node
{
    // Variables de progreso del juego
    public static bool HasDoubleJump { get; set; } = false;
    public static bool HasWallGrab { get; set; } = false;

    public string LastLevelPath { get; set; } = "res://scenes/levels/bosque1_fase1.tscn";

    public void ResetProgress()
    {
        HasDoubleJump = false;
        HasWallGrab = false;
        LastLevelPath = "res://scenes/levels/bosque1_fase1.tscn";
    }

    // Fragmentos de colores
    public static bool HasRedFragment = false;
    public static bool HasGreenFragment = false;
    public static bool HasBlueFragment = false;

    public static void CollectFragment(string color)
    {
        switch (color.ToLower())
        {
            case "red": HasRedFragment = true; break;
            case "green": HasGreenFragment = true; break;
            case "blue": HasBlueFragment = true; break;
        }
    }

    public static string GetFragmentStateKey()
    {
        string key = "";
        if (HasRedFragment) key += "r";
        if (HasGreenFragment) key += "g";
        if (HasBlueFragment) key += "b";
        return key;
    }
}

