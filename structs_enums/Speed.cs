using Microsoft.Xna.Framework;
    
struct Speed
{
    public Vector2 direction;
    public float abs;
    public bool hadCollision;

    public Speed(Vector2 _direction, float _abs, bool _hadCollision = false)
    {
        direction = _direction;
        abs = _abs;
        hadCollision = _hadCollision;
    }
}

