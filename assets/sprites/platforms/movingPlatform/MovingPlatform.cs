using Godot;

/// <summary>
/// Plataforma que se mueve entre dos puntos con una velocidad constante.
/// Si un jugador está sobre la plataforma, se mueve junto con ella.
/// </summary>
public partial class MovingPlatform : Node2D
{
    /// <summary>
    /// Distancia relativa desde la posición inicial hacia donde se moverá la plataforma.
    /// </summary>
    [Export] public Vector2 RelativeOffset = new Vector2(200, 0);
    
    [Export] public float Speed = 50f;
 
    private Vector2 PointA;   
    private Vector2 PointB;   
    private bool goingToB = true;    
    private Node2D playerOnPlatform = null;

    /// <summary>
    /// Inicializa los puntos A y B en base a la posición global y el offset.
    /// </summary>
    public override void _Ready()
    {
        PointA = GlobalPosition;
        PointB = PointA + RelativeOffset;
    }

    /// <summary>
    /// Se llama cada frame. Mueve la plataforma entre los puntos A y B,
    /// y mueve al jugador si está montado en la plataforma.
    /// </summary>
    /// <param name="delta">Tiempo transcurrido desde el último frame.</param>
    public override void _Process(double delta)
    {
        Vector2 target = goingToB ? PointB : PointA;
        Vector2 previousPosition = GlobalPosition;

        // Mover la plataforma hacia el punto destino
        GlobalPosition = GlobalPosition.MoveToward(target, Speed * (float)delta);

        // Si la plataforma está en movimiento, mover al jugador con ella
        if (playerOnPlatform != null)
        {
            Vector2 movementDelta = GlobalPosition - previousPosition;
            playerOnPlatform.GlobalPosition += movementDelta;
        }

        // Si la plataforma ha llegado al punto de destino, cambiar de dirección
        if (GlobalPosition.DistanceTo(target) < 1f)
            goingToB = !goingToB;
    }


}
