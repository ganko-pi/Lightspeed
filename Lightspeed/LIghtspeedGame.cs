using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Lightspeed;

public class LightspeedGame : Game
{
    private GraphicsDeviceManager _Graphics;
    private SpriteBatch _SpriteBatch;
    private OrthographicCamera _Camera;
    private Background _Background = new();
    private Player _Player = new();
    private Vector2 _PlayerPositionRelativeToCamera = new(0.5f, 0.825f);

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
        _Camera.LookAt(_Player.Position);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _SpriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D backgroundTexture = Content.Load<Texture2D>("Background/starfield");
        _Background.Texture = backgroundTexture;

        Texture2D playerTexture = Content.Load<Texture2D>("Player/player");
        _Player.Texture = new Sprite(playerTexture, frameCount: 4, fps: 8);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        _Player.Update(gameTime);

        SizeF cameraViewport = _Camera.BoundingRectangle.Size;
        Vector2 playerTopLeftCornerRelativeToCameraTopLeftCorner = new(
            cameraViewport.Width * _PlayerPositionRelativeToCamera.X + (_Player.Texture.Size.X * _Player.Texture.Scale) / 2,
            cameraViewport.Height * _PlayerPositionRelativeToCamera.Y + (_Player.Texture.Size.Y * _Player.Texture.Scale) / 2);
        _Camera.Position = _Player.Position - playerTopLeftCornerRelativeToCameraTopLeftCorner;

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

        _Player.Texture.Draw(_SpriteBatch);
        _SpriteBatch.End();

        base.Draw(gameTime);
    }
}
