using Godot;
using System;

// <summary>
/// Representa el HUD (interfaz de usuario) del juego, mostrando la vida,
/// el puntaje y el estado del objeto "Teer" en función de los fragmentos recolectados.
/// </summary>
public partial class Hud : CanvasLayer
{
     [ExportGroup("Referencias de Nodo")]
    // <summary>
    /// Sprite animado que muestra la cantidad de vida del jugador.
    /// Cada frame representa un estado de salud.
    /// </summary>
    [Export] private AnimatedSprite2D lifeSprite;

    /// <summary>
    /// Sprite que representa visualmente el estado actual del objeto "Teer".
    /// Su textura cambia según los fragmentos recolectados.
    /// </summary>
    [Export] private Sprite2D teerSprite;

    /// <summary>
    /// Etiqueta que muestra el puntaje actual del jugador.
    /// </summary>
    [Export] private Label scoreLabel;
    
    /// <summary>
    /// Arreglo de texturas que representan los posibles estados de "Teer"
    /// combinando los fragmentos rojo, verde y azul.
    /// </summary>    
    [Export] private Texture2D[] teerStates;

    /// <summary>
    /// Puntaje actual del jugador.
    /// </summary>
    private int score = 0;

    /// <summary>
    /// Método llamado cuando el nodo entra en la escena.
    /// Inicializa el HUD con el estado inicial del Teer, puntaje y vida.
    /// </summary>
    public override void _Ready()
    {
        UpdateTeer();
        UpdateScore(0);
        UpdateLife(6);
    }

    /// <summary>
    /// Actualiza el sprite de vida del jugador.
    /// Ajusta el frame del sprite en función de la salud actual,
    /// limitada entre 0 y 6.
    /// </summary>
    /// <param name="currentHealth">Salud actual del jugador.</param>
    public void UpdateLife(int currentHealth)
    {
        int clamped = Mathf.Clamp(currentHealth, 0, 6);

        if (!lifeSprite.IsPlaying())
            lifeSprite.Animation = "vida";

        lifeSprite.Frame = clamped;
    }

    /// <summary>
    /// Suma puntos al puntaje actual y actualiza el texto mostrado en el HUD.
    /// </summary>
    /// <param name="points">Cantidad de puntos a agregar.</param>
    public void UpdateScore(int points)
    {
        score += points;
        scoreLabel.Text = $"score: {score:0000}";
    }

    /// <summary>
    /// Actualiza el estado visual del objeto "Teer" en función
    /// de los fragmentos recolectados. Usa bits para determinar el índice:
    /// rojo (bit 0), verde (bit 1), azul (bit 2).
    /// </summary>
    public void UpdateTeer()
    {
        int index = 0;

        if (GameState.HasRedFragment)
            index |= 1; // bit 0

        if (GameState.HasGreenFragment)
            index |= 2; // bit 1

        if (GameState.HasBlueFragment)
            index |= 4; // bit 2

        teerSprite.Texture = teerStates[index];
    }
}