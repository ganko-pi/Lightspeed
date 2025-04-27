using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Lightspeed;

public class Score
{
    private TimeSpan? _StartedAt = null;
    private readonly float _PointsPerSecond = 2f;

    public int CurrentScore { get; set; } = 0;
    public BitmapFont ScoreFont { get; set; }

    public void Start(GameTime gameTime)
    {
        if (_StartedAt != null)
        {
            return;
        }

        _StartedAt = gameTime.TotalGameTime;
    }

    public void Reset()
    {
        _StartedAt = null;
        CurrentScore = 0;
    }

    public void Update(GameTime gameTime)
    {
        if (_StartedAt == null)
        {
            return;
        }

        TimeSpan timeSinceStart = gameTime.TotalGameTime - (TimeSpan)_StartedAt;
        CurrentScore = (int)(timeSinceStart.TotalSeconds * _PointsPerSecond);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 screenLeftTop, SizeF screenSize)
    {
        string scoreString = CurrentScore.ToString();
        float scale = screenSize.Width / 500f;

        spriteBatch.DrawString(ScoreFont, scoreString, screenLeftTop, Color.White, rotation: 0, Vector2.Zero, scale, SpriteEffects.None, layerDepth: 0f);
    }
}
