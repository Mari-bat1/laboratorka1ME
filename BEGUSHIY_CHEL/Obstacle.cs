using System;
using System.Collections.Generic;
using System.Text;

namespace BEGUSHIY_CHEL
{
    public class Obstacle
    {
        private readonly GameSettings _settings;

        public int X { get; private set; }
        public int Y => _settings.GroundLevel - _settings.ObstacleHeight;
        public int Width => _settings.ObstacleWidth;
        public int Height => _settings.ObstacleHeight;

        internal bool IsActive { get; private set; } = true;

        internal Obstacle(GameSettings settings, int startX)
        {
            _settings = settings;
            X = startX;
        }

        internal void Update(double speed)
        {
            X -= (int)speed;

            if (X + Width < 0)
            {
                IsActive = false;
            }
        }

        internal Rectangle GetBounds()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    
    }
}
