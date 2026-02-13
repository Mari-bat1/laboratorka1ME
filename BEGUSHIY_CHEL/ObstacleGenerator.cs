using System;
using System.Collections.Generic;
using System.Text;

namespace BEGUSHIY_CHEL
{
    public class ObstacleGenerator
    {
        private readonly GameSettings _settings;
        private readonly Random _random;
        private readonly List<Obstacle> _obstacles;
        private double _timeSinceLastSpawn;
        private double _currentSpawnInterval;

        public IReadOnlyList<Obstacle> Obstacles => _obstacles.AsReadOnly();

        internal ObstacleGenerator(GameSettings settings)
        {
            _settings = settings;
            _random = new Random();
            _obstacles = new List<Obstacle>();
            _currentSpawnInterval = settings.ObstacleSpawnIntervalMs;
        }

        internal void Update(double deltaTime, double currentSpeed)
        {
            _currentSpawnInterval = Math.Max(
                _settings.MinSpawnInterval,
                _currentSpawnInterval - _settings.DifficultyIncreaseRate * deltaTime
            );

            _timeSinceLastSpawn += deltaTime;

            double randomFactor = new Random().NextDouble() * 1.4 + 0.8;
            double actualInterval = _currentSpawnInterval * randomFactor;

            if (_timeSinceLastSpawn >= _currentSpawnInterval)
            {
                SpawnObstacle();
                _timeSinceLastSpawn = 0;
            }

            for (int i = _obstacles.Count - 1; i >= 0; i--)
            {
                _obstacles[i].Update(currentSpeed);
                if (!_obstacles[i].IsActive)
                {
                    _obstacles.RemoveAt(i);
                }
            }
        }

        private void SpawnObstacle()
        {
            var obstacle = new Obstacle(_settings, _settings.FieldWidth);
            _obstacles.Add(obstacle);
        }

        internal void Reset()
        {
            _obstacles.Clear();
            _timeSinceLastSpawn = 0;
            _currentSpawnInterval = _settings.ObstacleSpawnIntervalMs;
        }
    }
}
