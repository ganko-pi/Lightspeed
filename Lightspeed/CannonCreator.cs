using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Lightspeed;

public class CannonCreator
{
    private float _LastCannonCreation = -10000f;
    private float _CannonCreationInterval = 5000f;
    private int _CannonCreationIntervalOffset = 10000;

    public void Reset()
    {
        _LastCannonCreation = -10000f;
    }

    public List<Cannon> CreateCannons(GameTime gameTime, Vector2 screenLeftTop, SizeF screenSize)
    {
        List<Cannon> cannons = [];
        float cannonCreationInterval = _CannonCreationInterval + Random.Shared.Next(_CannonCreationIntervalOffset);
        while ((_LastCannonCreation - cannonCreationInterval) > screenLeftTop.Y)
        {
            Cannon cannon = new(gameTime)
            {
                Position = new Vector2(screenLeftTop.X, _LastCannonCreation - cannonCreationInterval),
                ProjectileSpeed = new Vector2(10000f, 0f),
                ShotInterval = TimeSpan.FromSeconds(0.5),
            };

            cannons.Add(cannon);

            _LastCannonCreation -= cannonCreationInterval;
        }

        return cannons;
    }
}
