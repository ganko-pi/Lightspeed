using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Lightspeed;

public class Background
{
    public Texture2D Texture { get; set; }
    public Sprite[] TexturesToDraw { get; private set; }
    private int previousOffset = 0;

    public void Update(Vector2 screenLeftTop, SizeF screenSize)
    {
        float scale = screenSize.Width / Texture.Width;

        float wholeTextureOffsetY = screenLeftTop.Y / (Texture.Height * scale) - 0.5f;
        if (previousOffset != (int)wholeTextureOffsetY)
        {
            previousOffset = (int)wholeTextureOffsetY;
        }

        TexturesToDraw = new Sprite[2];
        for (int y = 0; y < 2; ++y)
        {
            Sprite sprite = new(Texture, frameCount: 1, fps: 1)
            {
                Scale = scale
            };
            sprite.Position.X = screenLeftTop.X + (sprite.Size.X * sprite.Scale / 2);
            sprite.Position.Y = ((int)wholeTextureOffsetY + y) * (Texture.Height * sprite.Scale);

            TexturesToDraw[y] = sprite;
        }
    }
}
