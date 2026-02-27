using System;
using System.Collections.Generic;
using System.Text;

namespace BEGUSHIY_CHEL
{
    public class GameEngine 
    {
        private readonly GameSettings _settings;
        private readonly Player _player;
        private readonly ObstacleGenerator _obstacleGenerator;

        private int _score;
        private bool _isGameOver;
        private double _currentSpeed;
        private DateTime _lastUpdateTime;

        public int Score => _score;
        public bool IsGameOver => _isGameOver;
        public Player Player => _player;
        public IReadOnlyList<Obstacle> Obstacles => _obstacleGenerator.Obstacles;

        public event Action<int> ScoreChanged;
        public event Action GameOver;

        public GameEngine()
        {
            _settings = new GameSettings();
            _player = new Player(_settings, 100);
            _obstacleGenerator = new ObstacleGenerator(_settings);
            ResetGame();
        }


        public void Update()
        {
            if (_isGameOver) return;

            var now = DateTime.Now;
            double deltaTime = (now - _lastUpdateTime).TotalMilliseconds;
            _lastUpdateTime = now;

            _currentSpeed = _settings.RunSpeed * (1 + _score / 1000.0);

            _player.Update();
            _obstacleGenerator.Update(deltaTime, _currentSpeed);

            _score += _settings.ScorePerFrame;
            ScoreChanged?.Invoke(_score);

            CheckCollisions();
        }

        public void Jump()
        {
            if (!_isGameOver)
            {
                _player.Jump();
            }
        }

        public void ResetGame()
        {
            _player.Reset();
            _obstacleGenerator.Reset();
            _score = 0;
            _isGameOver = false;
            _currentSpeed = _settings.RunSpeed;
            _lastUpdateTime = DateTime.Now;

            ScoreChanged?.Invoke(_score);
        }

        private void CheckCollisions()
        {
            var playerBounds = _player.GetBounds();

            foreach (var obstacle in _obstacleGenerator.Obstacles)
            {
                if (playerBounds.IntersectsWith(obstacle.GetBounds()))
                {
                    _isGameOver = true;
                    GameOver?.Invoke();
                    break;
                }
            }
        }


        public System.Drawing.Point GetPlayerPosition()
        {
            return new System.Drawing.Point(_player.X, _player.Y);
        }

    }
}
