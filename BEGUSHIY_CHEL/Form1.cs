namespace BEGUSHIY_CHEL
{
    public partial class Form1 : Form
    {
        // Твой GameEngine
        private GameEngine _game;

        // Таймер для обновления
        private System.Windows.Forms.Timer _gameTimer;

        // Для отладки
        private Label _debugLabel;

        public Form1()
        {
            InitializeComponent();
            this.Text = "ТЕСТ RUNNER (временный интерфейс)";
            this.Size = new Size(800, 500);
            this.DoubleBuffered = true; // убирает мерцание
            this.KeyPreview = true; // чтобы форма ловила клавиши

            // Создаем твой движок!
            _game = new GameEngine();

            // Настраиваем таймер (обновление 60 раз в секунду)
            _gameTimer = new System.Windows.Forms.Timer();
            _gameTimer.Interval = 16; // ~60 FPS
            _gameTimer.Tick += GameLoop;
            _gameTimer.Start();

            // Подписываемся на события
            _game.ScoreChanged += (score) =>
            {
                // Обновляем UI в потоке формы
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = $"Очки: {score}";
                });
            };

            _game.GameOver += () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("Game Over!");
                });
            };

            // Добавим кнопку сброса
            var resetButton = new Button
            {
                Text = "Новая игра",
                Location = new System.Drawing.Point(10, 10),
                Size = new Size(100, 30)
            };
            resetButton.Click += (s, e) => _game.ResetGame();
            this.Controls.Add(resetButton);

            // Добавим информационную метку
            _debugLabel = new Label
            {
                Location = new System.Drawing.Point(10, 50),
                Size = new Size(400, 100),
                Text = "Нажми пробел для прыжка"
            };
            this.Controls.Add(_debugLabel);
        }

        // Игровой цикл (вызывается каждый тик таймера)
        private void GameLoop(object sender, EventArgs e)
        {
            if (_game != null && !_game.IsGameOver)
            {
                _game.Update(); // ТВОЙ ГЛАВНЫЙ МЕТОД!

                // Перерисовываем форму
                this.Invalidate();
            }
        }

        // Отрисовка (временная, чтобы увидеть, что работает)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_game == null) return;

            var g = e.Graphics;

            // Получаем доступ к настройкам через рефлексию? 
            // Нет, лучше добавить свойство в GameEngine!
            // Но пока просто используем константу для теста
            int groundLevel = 350; // временно, потом исправим

            // Рисуем землю
            using (var pen = new Pen(Color.Black))
            {
                g.DrawLine(pen, 0, groundLevel, this.Width, groundLevel);
            }

            // Рисуем игрока (красный квадрат)
            var player = _game.Player;
            using (var brush = new SolidBrush(Color.Red))
            {
                g.FillRectangle(brush, player.X, player.Y, player.Width, player.Height);
            }

            // Рисуем препятствия (синие квадраты)
            using (var brush = new SolidBrush(Color.Blue))
            {
                foreach (var obstacle in _game.Obstacles)
                {
                    g.FillRectangle(brush, obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height);
                }
            }

            // Отладочная информация
            _debugLabel.Text = $"Игрок: Y={player.Y}, На земле?={!player.IsJumping}\n" +
                              $"Препятствий: {_game.Obstacles.Count}\n" +
                              $"Счет: {_game.Score}";
        }

        // Обработка клавиш
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _game.Jump(); // ТВОЙ МЕТОД ПРЫЖКА!
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }
    }
}
