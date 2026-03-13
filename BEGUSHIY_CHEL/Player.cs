using System;
using System.Collections.Generic;
using System.Text;

namespace BEGUSHIY_CHEL
{
    public class Player 
    {
        private readonly GameSettings _settings;
        private double _yPosition;
        private double _yVelocity;
        private bool _isOnGround;

 
        public int X { get; private set; }
        public int Y => (int)_yPosition;
        public int Width => _settings.PlayerSize;
        public int Height => _settings.PlayerSize;

        internal Player(GameSettings settings, int startX)
        {
            _settings = settings;
            X = startX;
            _yPosition = settings.GroundLevel - settings.PlayerSize;
            _yVelocity = 0;
            _isOnGround = true;
        }

        internal void Jump()
        {
            if (_isOnGround)
            {
                _yVelocity = _settings.JumpForce;
                _isOnGround = false;
            }
        }

        internal void Update()
        {
            _yVelocity += _settings.Gravity;
            _yPosition += _yVelocity;

            double groundY = _settings.GroundLevel - _settings.PlayerSize;
            if (_yPosition >= groundY)
            {
                _yPosition = groundY;
                _yVelocity = 0;
                _isOnGround = true;
            }
        }

        //проверка столкновений
        internal Rectangle GetBounds()
        {
            return new Rectangle(X, (int)_yPosition, Width, Height);
        }

        internal void Reset()
        {
            _yPosition = _settings.GroundLevel - _settings.PlayerSize;
            _yVelocity = 0;
            _isOnGround = true;
        }

        public bool IsJumping => !_isOnGround;
    }
}
