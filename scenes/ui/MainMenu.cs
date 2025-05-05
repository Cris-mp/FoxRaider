using Godot;
using System;

public partial class MainMenu : Control
{
    VBoxContainer menu;
    VBoxContainer niveles;

    public override void _Ready()
    {
        GameState.LoadGame();
        
        menu = GetNode<VBoxContainer>("ctdBotones");
        niveles = GetNode<VBoxContainer>("ctdNiveles");
        menu.GetNode<Button>("btnJugar").Pressed += OnStartButtonPressed;
        menu.GetNode<Button>("btnSalir").Pressed += OnExitButtonPressed;

        var bosque1 = niveles.GetNode<HBoxContainer>("HBoxContainer");
        bosque1.GetNode<TextureButton>("nivel1").Pressed += () => OnStartButtonPressed_lvl("nivel1");
        bosque1.GetNode<TextureButton>("nivel2").Pressed += () => OnStartButtonPressed_lvl("nivel2");
        bosque1.GetNode<TextureButton>("nivel3").Pressed += () => OnStartButtonPressed_lvl("nivel3");
        var bosque2 = niveles.GetNode<HBoxContainer>("HBoxContainer2");
        bosque2.GetNode<TextureButton>("nivel4").Pressed += () => OnStartButtonPressed_lvl("nivel4");
        bosque2.GetNode<TextureButton>("nivel5").Pressed += () => OnStartButtonPressed_lvl("nivel5");
        bosque2.GetNode<TextureButton>("nivel6").Pressed += () => OnStartButtonPressed_lvl("nivel6");
        ActualizarBotonesNiveles();
    }

    private void OnStartButtonPressed()
    {            
        menu.Visible = false;
        niveles.Visible = true;
    }

    private void OnStartButtonPressed_lvl(string nivel)
    {
        PackedScene scene = null;
        switch (nivel)
        {
            case "nivel1":
                scene = ResourceLoader.Load<PackedScene>("res://scenes/levels/bosque1_fase1.tscn");
                break;
            case "nivel2":
                scene = ResourceLoader.Load<PackedScene>("res://scenes/levels/bosque1_fase2.tscn");
                break;
            case "nivel3":
                scene = ResourceLoader.Load<PackedScene>("res://scenes/levels/bosque1_fase3.tscn");
                break;
            case "nivel4":
                scene = ResourceLoader.Load<PackedScene>("res://scenes/levels/bosque2_fase1.tscn");
                break;
            case "nivel5":
                scene = ResourceLoader.Load<PackedScene>("res://scenes/levels/bosque2_fase2.tscn");
                break;
            case "nivel6":
                scene = ResourceLoader.Load<PackedScene>("res://scenes/levels/bosque2_fase3.tscn");
                break;
        }
        GetTree().ChangeSceneToPacked(scene);
    }

    private void ActualizarBotonesNiveles()
    {
        for (int i = 1; i <= 6; i++)
        {
            string nivelName = $"nivel{i}";
            TextureButton btn = null;

            if (i <= 3)
                btn = GetNode<TextureButton>($"ctdNiveles/HBoxContainer/{nivelName}");
            else
                btn = GetNode<TextureButton>($"ctdNiveles/HBoxContainer2/{nivelName}");

            if (btn != null)
            {
                bool habilitado = i <= GameState.MaxLevelUnlocked;
                btn.Disabled = !habilitado;
                btn.Modulate = habilitado ? Colors.White : new Color(1, 1, 1, 0.5f); // Opaco si está bloqueado
            }
        }
    }

    private void OnExitButtonPressed()
    {      
        GetTree().Quit();
    }
}
