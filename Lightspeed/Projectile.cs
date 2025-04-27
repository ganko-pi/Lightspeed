using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Lightspeed;

public class Projectile
{
    public Vector2 Position { get; set; }
    public SizeF Size { get; set; }
    public Vector2 Speed { get; set; }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Update(deltaTime);
    }

    public void Update(float deltaTime)
    {
        Position += Speed * deltaTime;

        Position.Round();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        RectangleF rectangle = new(Position, Size);
        spriteBatch.DrawRectangle(rectangle, Color.White, thickness: 100f);
    }
}
