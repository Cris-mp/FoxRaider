using Godot;

/// <summary>
/// Zona de control que oculta o muestra una capa de TileMap (TileMapLayer) 
/// al detectar la entrada o salida del jugador.
/// </summary>
public partial class ZonaControl : Area2D
{
    /// <summary>
    /// Referencia exportada a la capa de TileMap que será activada o desactivada
    /// al entrar o salir de la zona.
    /// </summary>
    [Export] public TileMapLayer tilemapTarget;

    /// <summary>
    /// Método que se ejecuta al inicializar el nodo.
    /// Conecta las señales BodyEntered y BodyExited a sus respectivos métodos.
    /// </summary>
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    /// <summary>
    /// Evento llamado cuando un cuerpo entra en la zona.
    /// Si el cuerpo es un jugador y existe una capa TileMap asociada,
    /// esta se desactiva (se oculta).
    /// </summary>
    /// <param name="body">Nodo que ha entrado en la zona.</param>
    private void OnBodyEntered(Node body)
    {
        if (body is Player && tilemapTarget != null)
        {
            tilemapTarget.Visible = false;
        }
    }
    
    // <summary>
    /// Evento llamado cuando un cuerpo sale de la zona.
    /// Si el cuerpo es un jugador y existe una capa TileMap asociada,
    /// esta se reactiva (se muestra).
    /// </summary>
    /// <param name="body">Nodo que ha salido de la zona.</param>
    private void OnBodyExited(Node body)
    {
        if (body is Player && tilemapTarget != null)
        {
            tilemapTarget.Visible = true;
        }
    }
}

