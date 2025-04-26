using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lightspeed;

public class Sprite
{
    private float _timeElapsed;
    private float _timeToUpdate;

    protected Texture2D _spriteSheet;
    protected Rectangle[] _frames;

    public Vector2 Position = Vector2.Zero;
    public Color Color = Color.White;
    public Vector2 Size;
    public Vector2 Origin;
    public float Rotation = 0f;
    public float Scale = 1f;
    public SpriteEffects SpriteEffects;
    public int FrameIndex = 0;
    public bool IsLooping = true;

    public int FramesPerSecond
    {
        get => (int)Math.Round(1f / _timeToUpdate);
        set => _timeToUpdate = 1f / value;
    }

    public Sprite(Texture2D spriteSheet, int frameCount, int fps)
    {
        _spriteSheet = spriteSheet;
        _frames = new Rectangle[frameCount];
        FramesPerSecond = fps;

        int height = _spriteSheet.Height / _frames.Length;
        for (int i = 0; i < _frames.Length; ++i)
        {
            _frames[i] = new Rectangle(0, i * height, _spriteSheet.Width, height);
        }

        Size = new Vector2(_spriteSheet.Width, height);
        Origin = new Vector2(Size.X / 2, Size.Y / 2);
    }

    public void Update(GameTime gameTime)
    {
        _timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timeElapsed < _timeToUpdate)
        {
            return;
        }

        _timeElapsed -= _timeToUpdate;
        if (FrameIndex < _frames.Length - 1)
        {
            ++FrameIndex;
            return;
        }

        if (IsLooping)
        {
            FrameIndex = 0;
            return;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_spriteSheet, Position, _frames[FrameIndex], Color, Rotation, Origin, Scale, SpriteEffects, 0f);
    }
}
