using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Lightspeed;

public class Cannon
{
    private TimeSpan _LastShot;

    public Cannon(GameTime creationTime)
    {
        _LastShot = creationTime.TotalGameTime;
    }

    public Vector2 Position { get; set; }
    public TimeSpan ShotInterval { get; set; }
    public Vector2 ProjectileSpeed { get; set; }
    public List<Projectile> Projectiles { get; set; } = [];

    public void Update(GameTime gameTime, Vector2 screenLeftTop, SizeF screenSize)
    {
        List<Projectile> projectilesToRemove = [];
        foreach (Projectile projectile in Projectiles)
        {
            projectile.Update(gameTime);
            if ((projectile.Position.X > (screenLeftTop.X + screenSize.Width)) ||
                ((projectile.Position.X + projectile.Size.Width) < screenLeftTop.X))
            {
                projectilesToRemove.Add(projectile);
            }
        }

        foreach (Projectile projectileToRemove in projectilesToRemove)
        {
            Projectiles.Remove(projectileToRemove);
        }

        if (gameTime.TotalGameTime < (_LastShot + ShotInterval))
        {
            return;
        }

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        while ((_LastShot + ShotInterval) < gameTime.TotalGameTime)
        {
            Projectile projectile = new()
            {
                Position = Position,
                Size = new SizeF(50, 100),
                Speed = ProjectileSpeed,
            };

            projectile.Update(deltaTime);

            Projectiles.Add(projectile);

            _LastShot += ShotInterval;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Projectile projectile in Projectiles)
        {
            projectile.Draw(spriteBatch);
        }
    }
}
