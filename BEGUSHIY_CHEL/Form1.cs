namespace BEGUSHIY_CHEL
{
    public partial class Form1 : Form
    {
        private Image _runmanImage;
        private Image _dangerImage;
        private Image _fonImage;

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

            // Загружаем картинки
            try
            {
                // Убедись, что имена файлов совпадают с теми, что ты добавил(а)
                _runmanImage = Image.FromFile("runman1.png");
                _dangerImage = Image.FromFile("danger.png");
                _fonImage = Image.FromFile("fon.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить картинки: {ex.Message}. Будут использоваться заглушки.");
                // Если картинок нет, создадим заглушки, чтобы программа не падала
                _runmanImage = new Bitmap(1, 1);
                _dangerImage = new Bitmap(1, 1);
                _fonImage = new Bitmap(1, 1);
            }

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
                Size = new Size(100, 20),
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_game == null) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Сглаживание
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor; // Для пиксель-арта

            // РИСУЕМ ФОН (задний план)
            if (_fonImage != null && _fonImage.Width > 1)
            {
                // Рисуем фон на весь экран (можно и с тайлингом, если картинка маленькая)
                g.DrawImage(_fonImage, 0, 0, this.Width, this.Height);
            }
            else
            {
                // Заглушка: заливаем всё небом, а землю рисуем линией
                using (var brush = new SolidBrush(Color.SkyBlue))
                {
                    g.FillRectangle(brush, 0, 0, this.Width, _game.Player.Y + _game.Player.Height); // Небо до земли
                }
                using (var brush = new SolidBrush(Color.Green))
                {
                    g.FillRectangle(brush, 0, _game.Player.Y + _game.Player.Height, this.Width, this.Height); // Земля
                }
            }

            // Получаем уровень земли (из настроек или высчитываем)
            int groundLevel = _game.Player.Y + _game.Player.Height; // Можно и так, но лучше брать из _settings

            // РИСУЕМ ПРЕПЯТСТВИЯ
            if (_dangerImage != null && _dangerImage.Width > 1)
            {
                foreach (var obstacle in _game.Obstacles)
                {
                    // Рисуем картинку препятствия. Она может быть больше/меньше,
                    // поэтому растягиваем её под размеры препятствия.
                    g.DrawImage(_dangerImage,
                        new Rectangle(obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height));
                }
            }
            else
            {
                // Заглушка: рисуем синие квадраты
                using (var brush = new SolidBrush(Color.Blue))
                {
                    foreach (var obstacle in _game.Obstacles)
                    {
                        g.FillRectangle(brush, obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height);
                    }
                }
            }

            // РИСУЕМ ИГРОКА (поверх препятствий)
            if (_runmanImage != null && _runmanImage.Width > 1)
            {
                g.DrawImage(_runmanImage,
                    new Rectangle(_game.Player.X, _game.Player.Y, _game.Player.Width, _game.Player.Height));
            }
            else
            {
                // Заглушка: рисуем красный квадрат
                using (var brush = new SolidBrush(Color.Red))
                {
                    g.FillRectangle(brush, _game.Player.X, _game.Player.Y, _game.Player.Width, _game.Player.Height);
                }
            }

            // Отладочная информация
            _debugLabel.Text = $"Счет: {_game.Score}";
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
