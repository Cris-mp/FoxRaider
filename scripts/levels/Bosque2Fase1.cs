using Godot;
using System;

// <summary>
/// Script del nodo raíz de la escena "Bosque2Fase1" que corresponde a la escena final.
/// Informa de que habrá mas niveles en el futuro y despues de unos segundos vuelve al menu principal.
/// </summary>
public partial class Bosque2Fase1 : Control
{
    [Export] public float TiempoEspera = 5f;

    public override void _Ready()
    {
        // Espera unos segundos y vuelve al menú
        GetTree().CreateTimer(TiempoEspera).Timeout += () =>
        {
            GetTree().ChangeSceneToFile("res://scenes/ui/main_menu.tscn");
        };
    }
}