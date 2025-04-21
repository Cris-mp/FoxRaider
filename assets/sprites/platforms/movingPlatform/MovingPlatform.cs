using Godot;

public partial class MovingPlatform : Node2D
{
    [Export] public Vector2 RelativeOffset = new Vector2(200, 0); // Distancia hacia donde se moverá
    [Export] public float Speed = 50f;

    private Vector2 PointA;
    private Vector2 PointB;
    private bool goingToB = true;
    private Node2D playerOnPlatform = null;

    public override void _Ready()
    {
        PointA = GlobalPosition;                 
        PointB = PointA + RelativeOffset;         
    }

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
