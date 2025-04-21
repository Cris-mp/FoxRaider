using Godot;
using System;

public partial class Spike : Sprite2D
{
    [Export] public CollisionShape2D shape;
    public override void _Ready()
    {    
        RectangleShape2D rectShape = new RectangleShape2D();
        rectShape.Size = this.RegionRect.Size;
        shape.Shape = rectShape;
    }

}
