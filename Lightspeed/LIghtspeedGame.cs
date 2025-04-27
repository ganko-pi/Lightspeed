using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.ViewportAdapters;

namespace Lightspeed;

public class LightspeedGame : Game
{
    private GraphicsDeviceManager _Graphics;
    private SpriteBatch _SpriteBatch;
    private BitmapFont _Font;
    private OrthographicCamera _Camera;
    private Background _Background = new();
    private Score _Score = new();
    private Player _Player = new();
    private CannonCreator _CannonCreator = new();
    private List<Cannon> _Cannons = [];
    private Vector2 _PlayerPositionRelativeToCamera = new(0.5f, 0.825f);
    private bool _GameOver;
    private bool _ButtonReleasedAfterGameOver = false;

    public LightspeedGame()
    {
        _Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _Graphics.IsFullScreen = false;
        _Graphics.PreferredBackBufferWidth = 1600;
        _Graphics.PreferredBackBufferHeight = 900;
        _Graphics.ApplyChanges();

        ViewportAdapter viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 16000, 9000);
        _Camera = new OrthographicCamera(viewportAdapter);
        Reset();

        base.Initialize();
    }

    private void Reset()
    {
        _GameOver = false;

        _Player.Reset();
        _Camera.LookAt(_Player.Position);

        _CannonCreator.Reset();

        _Cannons.Clear();
    }

    protected override void LoadContent()
    {
        _SpriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D backgroundTexture = Content.Load<Texture2D>("Background/starfield");
        _Background.Texture = backgroundTexture;

        Texture2D playerTexture = Content.Load<Texture2D>("Player/player");
        _Player.Texture = new Sprite(playerTexture, frameCount: 4, fps: 8);

        _Font = Content.Load<BitmapFont>("Font/score_font");
        _Score.ScoreFont = _Font;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        if (!_GameOver)
        {
            _GameOver = IsGameOver();
            _ButtonReleasedAfterGameOver = false;
        }

        if (_GameOver &&
            GamePad.GetState(PlayerIndex.One).Buttons.A != ButtonState.Pressed &&
            !Keyboard.GetState().IsKeyDown(Keys.Space) &&
            !Keyboard.GetState().IsKeyDown(Keys.U))
        {
            _ButtonReleasedAfterGameOver = true;
            return;
        }

        if (_GameOver && !_ButtonReleasedAfterGameOver)
        {
            return;
        }

        if (_GameOver &&
            _ButtonReleasedAfterGameOver &&
            (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Space) ||
            Keyboard.GetState().IsKeyDown(Keys.U)))
        {
            Reset();
        }

        if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Space) ||
            Keyboard.GetState().IsKeyDown(Keys.U))
        {
            _Player.Speed = Speed.Slow;
        }
        else
        {
            _Player.Speed = Speed.Fast;
        }

        _Score.Update(gameTime, _Camera.Position, _Camera.BoundingRectangle.Size);

        _Player.Update(gameTime);

        SizeF cameraViewport = _Camera.BoundingRectangle.Size;
        Vector2 playerTopLeftCornerRelativeToCameraTopLeftCorner = new(
            cameraViewport.Width * _PlayerPositionRelativeToCamera.X + (_Player.Texture.Size.X * _Player.Texture.Scale) / 2,
            cameraViewport.Height * _PlayerPositionRelativeToCamera.Y + (_Player.Texture.Size.Y * _Player.Texture.Scale) / 2);
        _Camera.Position = _Player.Position - playerTopLeftCornerRelativeToCameraTopLeftCorner;

        List<Cannon> cannonsToRemove = [];
        foreach (Cannon cannon in _Cannons)
        {
            cannon.Update(gameTime, _Camera.Position, _Camera.BoundingRectangle.Size);
            if (cannon.Position.Y > (_Camera.Position.Y + _Camera.BoundingRectangle.Size.Height))
            {
                cannonsToRemove.Add(cannon);
            }
        }

        foreach (Cannon cannonToRemove in cannonsToRemove)
        {
            _Cannons.Remove(cannonToRemove);
        }

        List<Cannon> newCannons = _CannonCreator.CreateCannons(gameTime, _Camera.Position, _Camera.BoundingRectangle.Size);
        _Cannons.AddRange(newCannons);

        _Background.Update(_Camera.Position, _Camera.BoundingRectangle.Size);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Matrix transformMatrix = _Camera.GetViewMatrix();
        _SpriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp);
        foreach (Sprite backgroundPart in _Background.TexturesToDraw)
        {
            backgroundPart.Draw(_SpriteBatch);
        }

        _Score.Draw(_SpriteBatch, _Camera.Position, _Camera.BoundingRectangle.Size);
        _Player.Texture.Draw(_SpriteBatch);
        foreach (Cannon cannon in _Cannons)
        {
            cannon.Draw(_SpriteBatch);
        }

        if (_GameOver)
        {
            string gameOverText = "Game Over";
            float scale = _Camera.BoundingRectangle.Size.Width / 100f;

            Vector2 fontOrigin = _Font.MeasureString(gameOverText) / 2;
            Vector2 screenCenter = new(_Camera.Position.X + (_Camera.BoundingRectangle.Size.Width / 2),
                _Camera.Position.Y + (_Camera.BoundingRectangle.Size.Height / 2));

            _SpriteBatch.DrawString(_Font, gameOverText, screenCenter, Color.White, rotation: 0, fontOrigin, scale, SpriteEffects.None, layerDepth: 0f);
        }

        _SpriteBatch.End();

        base.Draw(gameTime);
    }

    private bool IsGameOver()
    {
        foreach (Projectile projectile in _Cannons.SelectMany(cannon => cannon.Projectiles))
        {
            RectangleF projectileHitbox = new(projectile.Position, projectile.Size);
            if (_Player.Hitbox.Intersects(projectileHitbox.BoundingRectangle))
            {
                return true;
            }
        }

        return false;
    }
}
