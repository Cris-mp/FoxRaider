using Godot;
using System;

/// <summary>
/// Clase que gestiona el estado persistente del juego, como habilidades desbloqueadas,
/// niveles alcanzados y fragmentos recolectados. También maneja la carga y guardado de datos.
/// </summary>
public partial class GameState : Node
{
    /// <summary>
    /// Indica si el jugador ha desbloqueado el doble salto.    
    /// </summary>
    public static bool HasDoubleJump { get; set; } = false;

    /// <summary>
    /// Indica si el jugador ha desbloqueado la habilidad de sujetarse a muros.
    /// </summary>
    public static bool HasWallGrab { get; set; } = false;

    /// <summary>
    /// Indica si el jugador ha recolectado el fragmento rojo.    
    /// </summary>
    public static bool HasRedFragment = false;

    /// <summary>
    /// Indica si el jugador ha recolectado el fragmento verde.
    /// </summary>
    public static bool HasGreenFragment = false;

    /// <summary>
    /// Indica si el jugador ha recolectado el fragmento azul.
    /// </summary>
    public static bool HasBlueFragment = false;

    /// <summary>
    /// Nivel más alto desbloqueado por el jugador.
    /// </summary>
    public static int MaxLevelUnlocked { get; set; } = 1;

    /// <summary>
    /// Ruta del último nivel jugado.
    /// </summary>
    public string LastLevelPath { get; set; } = "res://scenes/levels/bosque1_fase1.tscn";

    /// <summary>
    /// Ruta del archivo de guardado.
    /// </summary>
    private static string SavePath => "user://savegame.json";

    /// <summary>
    /// Guarda el estado actual del juego en un archivo JSON.
    /// Incluye habilidades, fragmentos y nivel desbloqueado.
    /// </summary>
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

    /// <summary>
    /// Carga el estado del juego desde el archivo de guardado.
    /// Si el archivo no existe o no es válido, se usan los valores por defecto.
    /// </summary>
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

    /// <summary>
    /// Restablece el progreso del jugador a los valores iniciales.
    /// Elimina habilidades y fragmentos, y vuelve al primer nivel desbloqueado.
    /// Guarda el estado actualizado.
    /// </summary>
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

    /// <summary>
    /// Marca un fragmento de color como recolectado, guarda el juego
    /// e intenta actualizar el HUD si está presente en la escena.
    /// </summary>
    /// <param name="color">Color del fragmento recolectado ("red", "green", "blue").</param>
    /// <param name="nivel">Ruta relativa al nodo del HUD dentro del árbol de la escena.</param>
    public static void CollectFragment(string color, string nivel)
    {
        switch (color.ToLower())
        {
            case "red": HasRedFragment = true; break;
            case "green": HasGreenFragment = true; break;
            case "blue": HasBlueFragment = true; break;
        }

        SaveGame();

        var tree = (SceneTree)Engine.GetMainLoop();
        tree.Root.PrintTreePretty();
        var hud = (Hud)tree.Root.GetNodeOrNull(nivel + "/Hud");
        hud?.UpdateTeer();
    }

}