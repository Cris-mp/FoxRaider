using Godot;
using System;


public partial class GameState : Node
{
    public static bool HasDoubleJump { get; set; } = false;
    public static bool HasWallGrab { get; set; } = false;

    public static bool HasRedFragment = false;
    public static bool HasGreenFragment = false;
    public static bool HasBlueFragment = false;

    public static int MaxLevelUnlocked { get; set; } = 1;

    public string LastLevelPath { get; set; } = "res://scenes/levels/bosque1_fase1.tscn";

    private static string SavePath => "user://savegame.json";

    public static void SaveGame()
    {
        var saveData = new Godot.Collections.Dictionary<string, Variant>
        {
            { "MaxLevelUnlocked", MaxLevelUnlocked },
            { "HasDoubleJump", HasDoubleJump },
            { "HasWallGrab", HasWallGrab },
            { "HasRedFragment", HasRedFragment },
            { "HasGreenFragment", HasGreenFragment },
            { "HasBlueFragment", HasBlueFragment }
        };

        var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
        file.StoreString(Json.Stringify(saveData));
        file.Close();
    }

    public static void LoadGame()
    {
        if (!FileAccess.FileExists(SavePath))
        {
            GD.Print("No save file found, using defaults.");
            return;
        }

        var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
        string json = file.GetAsText();
        file.Close();

        var parsed = Json.ParseString(json);
        if (parsed.VariantType != Variant.Type.Dictionary)
        {
            GD.PrintErr("Parsed data is not a dictionary.");
            return;
        }

        var result = (Godot.Collections.Dictionary<string, Variant>)parsed;
        if (result == null)
        {
            GD.PrintErr("Failed to parse save file.");
            return;
        }

        MaxLevelUnlocked = result.ContainsKey("MaxLevelUnlocked") ? (int)result["MaxLevelUnlocked"] : 1;
        HasDoubleJump = result.ContainsKey("HasDoubleJump") ? (bool)result["HasDoubleJump"] : false;
        HasWallGrab = result.ContainsKey("HasWallGrab") ? (bool)result["HasWallGrab"] : false;
        HasRedFragment = result.ContainsKey("HasRedFragment") ? (bool)result["HasRedFragment"] : false;
        HasGreenFragment = result.ContainsKey("HasGreenFragment") ? (bool)result["HasGreenFragment"] : false;
        HasBlueFragment = result.ContainsKey("HasBlueFragment") ? (bool)result["HasBlueFragment"] : false;
    }

    public void ResetProgress()
    {
        HasDoubleJump = false;
        HasWallGrab = false;
        HasRedFragment = false;
        HasGreenFragment = false;
        HasBlueFragment = false;
        MaxLevelUnlocked = 1;
        SaveGame(); // guardar después de resetear
    }

    public static void CollectFragment(string color,string nivel)
    {
        GD.Print($"Foxy recibió cogie el fragmento gs");
        switch (color.ToLower())
        {
            case "red": HasRedFragment = true; break;
            case "green": HasGreenFragment = true; break;
            case "blue": HasBlueFragment = true; break;
        }
        GD.Print($"Foxy recibió cogie el fragmento g {color}{HasRedFragment} {HasGreenFragment} {HasBlueFragment}");
        SaveGame(); // guardar después de recolectar

        var tree = (SceneTree)Engine.GetMainLoop();
        tree.Root.PrintTreePretty();
        var hud = (Hud)tree.Root.GetNodeOrNull(nivel+"/Hud");
        hud?.UpdateTeer();
GD.Print($"[GameState] HUD instanciado: {hud}, está en escena: {hud?.IsInsideTree()}");
        
    }

}