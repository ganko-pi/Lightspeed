using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Lightspeed;

public class Score
{
    private readonly float _PointsPerDistanceUnit = 1f;

    public int CurrentScore { get; set; } = 0;
    public BitmapFont ScoreFont { get; set; }

    public void Reset()
    {
        CurrentScore = 0;
    }

    public void Update(GameTime gameTime, Vector2 screenLeftTop, SizeF screenSize)
    {
        int distanceFromStart = -(int)(screenLeftTop.Y / 1000f);
        CurrentScore = (int)(distanceFromStart * _PointsPerDistanceUnit);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 screenLeftTop, SizeF screenSize)
    {
        string scoreString = CurrentScore.ToString();
        float scale = screenSize.Width / 500f;

        spriteBatch.DrawString(ScoreFont, scoreString, screenLeftTop, Color.White, rotation: 0, Vector2.Zero, scale, SpriteEffects.None, layerDepth: 0f);
    }
}
