using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Lightspeed;

public class CannonCreator
{
    private float _LastCannonCreation = -10000f;
    private float _CannonCreationInterval = 5000f;

    public List<Cannon> CreateCannons(GameTime gameTime, Vector2 screenLeftTop, SizeF screenSize)
    {
        List<Cannon> cannons = [];
        while ((_LastCannonCreation - _CannonCreationInterval) > screenLeftTop.Y)
        {
            Cannon cannon = new(gameTime)
            {
                Position = screenLeftTop,
                ProjectileSpeed = new Vector2(10000f, 0f),
                ShotInterval = TimeSpan.FromSeconds(0.5),
            };

            cannons.Add(cannon);

            _LastCannonCreation -= _CannonCreationInterval;
        }

        return cannons;
    }
}
