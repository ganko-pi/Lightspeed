using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Lightspeed;

public class Player
{
    private Vector2 _Position = new();
    private readonly Dictionary<Speed, int> _SpeedMap = new()
    {
        { Speed.Slow, 1000 },
        { Speed.Fast, 4000 },
    };

    public Speed Speed { get; set; } = Speed.Fast;

    public Sprite Texture {get; set; }

    public Vector2 Position { get => _Position; }

    public CircleF Hitbox { get; set; }

    public Player()
    {
        Reset();
    }

    public void Reset()
    {
        _Position = new();
        Speed = Speed.Fast;
        Hitbox = new CircleF();
    }

    public void Update(GameTime gameTime)
    {
        MovePlayer(gameTime);
        UpdateSpritePosition();
    }

    private void MovePlayer(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _Position.Y -= _SpeedMap[Speed] * deltaTime;

        _Position.Round();
    }

    private void UpdateSpritePosition()
    {
        Texture.Position = new Vector2(_Position.X - (Texture.Size.X - Texture.Origin.X), _Position.Y - (Texture.Size.Y - Texture.Origin.Y));
        Hitbox = new CircleF(Texture.Position, Texture.Size.X / 8 * 3);
    }
}
