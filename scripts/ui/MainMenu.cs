using Godot;
using System;

/// <summary>
/// Controla el menú principal, carga niveles desbloqueados, 
/// y permite al jugador seleccionar niveles disponibles.
/// </summary>
public partial class MainMenu : Control
{
	[Export] public NivelData[] Niveles; // Datos de cada nivel (nombre y ruta)
	[Export] public TextureButton[] BotonesNiveles; // Botones asociados en el menú

	[ExportGroup("Referencias de UI")]
	[Export] private VBoxContainer menu;
	[Export] private VBoxContainer selectorNiveles;
	[Export] private Button btnJugar;
	[Export] private Button btnSalir;

	public override void _Ready()
	{
		GameState.LoadGame();           

		// Configurar botones de nivel dinámicamente
		for (int i = 0; i < Niveles.Length; i++)
		{

			var nivel = Niveles[i];
			var boton = BotonesNiveles[i];

			bool habilitado = (i + 1) <= GameState.MaxLevelUnlocked;
			boton.Disabled = !habilitado;
			boton.Modulate = habilitado ? Colors.White : new Color(1, 1, 1, 0.5f);

			boton.Pressed += () => CargarNivel(nivel.RutaEscena);
		}
	}

	/// <summary>
	/// Oculta el menú principal y muestra la selección de niveles.
	/// </summary>
	private void OnStartButtonPressed()
	{
		menu.Visible = false;
		selectorNiveles.Visible = true;
	}

	/// <summary>
	/// Carga la escena del nivel especificado.
	/// </summary>
	/// <param name="ruta">Ruta del archivo .tscn de la escena a cargar.</param>
	private void CargarNivel(string ruta)
	{
		var scene = ResourceLoader.Load<PackedScene>(ruta);
		if (scene != null)
		{
			GetTree().ChangeSceneToPacked(scene);
		}
		else
		{
			GD.PrintErr($"❌ No se pudo cargar la escena: {ruta}");
		}
	}

	/// <summary>
	/// Sale del juego.
	/// </summary>
	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
