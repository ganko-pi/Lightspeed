using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Lightspeed;

public class Player
{
    private Vector2 _Position = new(50, 100);
    private readonly Dictionary<Speed, int> _SpeedMap = new()
    {
        { Speed.Slow, 500 },
        { Speed.Fast, 1000 },
    };

    public Speed Speed { get; set; } = Speed.Fast;

    public Sprite Texture {get; set; }

    public Vector2 Position { get => _Position; }

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
    }
}
