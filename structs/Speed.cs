using Microsoft.Xna.Framework;
    
struct Speed
{
    public Vector2 direction;
    public float abs;

    public Speed(Vector2 _direction, float _abs)
    {
        direction = _direction;
        abs = _abs;
    }
}

