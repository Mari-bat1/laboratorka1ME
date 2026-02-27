using System;
using System.Collections.Generic;
using System.Text;

namespace BEGUSHIY_CHEL
{
    public class GameSettings
    {
        // Размеры поля и объектов
        public int FieldWidth { get; set; } = 800;      // ширина игрового поля
        public int FieldHeight { get; set; } = 400;     // высота игрового поля
        public int GroundLevel { get; set; } = 350;     // Y-координата земли
        public int PlayerSize { get; set; } = 30;       // размер игрока
        public int ObstacleWidth { get; set; } = 20;    // ширина препятствия
        public int ObstacleHeight { get; set; } = 30;   // высота препятствия

        // Физика
        public double Gravity { get; set; } = 0.8;      // гравитация
        public double JumpForce { get; set; } = -15;    // сила прыжка 
        public double RunSpeed { get; set; } = 5;       // скорость бега

        // Сложность
        public double ObstacleSpawnIntervalMs { get; set; } = 2000;  // интервал спавна 
        public double MinSpawnInterval { get; set; } = 1200;  
        public double DifficultyIncreaseRate { get; set; } = 0.2;    // ускорение со временем

        // Очки
        public int ScorePerFrame { get; set; } = 1; 
}
