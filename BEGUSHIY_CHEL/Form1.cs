namespace BEGUSHIY_CHEL
{
    public partial class Form1 : Form
    {
        private Image _runmanImage;
        private Image _dangerImage;
        private Image _fonImage;

        private Label _scoreLabel;

        private Image _gameOverImage; // Картинка для Game Over (можно не использовать)
        private bool _showGameOverScreen = false;

        // Для анимации
        private Image[] _runmanFrames;      // Массив кадров
        private int _currentFrame = 0;      // Текущий кадр
        private int _frameCounter = 0;      // Счетчик для смены кадров
        private const int FramesPerSecond = 10; // Сколько раз в секунду менять кадр

        // Твой GameEngine
        private GameEngine _game;

        // Таймер для обновления
        private System.Windows.Forms.Timer _gameTimer;



        public Form1()
        {
            InitializeComponent();
            this.Text = "БЕГУЩИЙ ЧЕЛ";
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

            try
            {
                // Загружаем кадры анимации
                _runmanFrames = new Image[]
                {
                    Image.FromFile("runman1.png"),
                    Image.FromFile("runman2.png")
                };

                _dangerImage = Image.FromFile("danger.png");
                _fonImage = Image.FromFile("fon.png");

                // Для совместимости с остальным кодом, если где-то еще используется _runmanImage
                _runmanImage = _runmanFrames[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить картинки: {ex.Message}. Будут использоваться заглушки.");
                // Если картинок нет, создадим заглушки, чтобы программа не падала
                _dangerImage = new Bitmap(1, 1);
                _fonImage = new Bitmap(1, 1);

                MessageBox.Show($"Не удалось загрузить кадры анимации: {ex.Message}");
                // Заглушка
                _runmanFrames = new Image[] { new Bitmap(1, 1) };
                _runmanImage = _runmanFrames[0];
            }

            try
            {
                _gameOverImage = Image.FromFile("gameover.png");
            }
            catch
            {
                _gameOverImage = null;
            }

            _game.ScoreChanged += (score) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    _scoreLabel.Text = $"Score: {score}";
                });
            };

            _game.GameOver += () => {
                this.Invoke((MethodInvoker)delegate
                {
                    // Убираем MessageBox
                    _showGameOverScreen = true;
                    this.Invalidate(); // Перерисовать форму сразу
                });
            };

            // Добавим кнопку сброса
            var resetButton = new Button
            {
                Text = "Новая игра",
                Location = new System.Drawing.Point(10, 10),
                Size = new Size(100, 30)
            };
            resetButton.Click += (s, e) =>
            {
                _game.ResetGame();
                _showGameOverScreen = false; // Убираем экран Game Over
                this.Invalidate(); // Перерисовываем
            };
            this.Controls.Add(resetButton);

            // Создаем красивый счет
            _scoreLabel = new Label
            {
                Location = new System.Drawing.Point(550, 20),  // Положение (правый верхний угол)
                Size = new Size(200, 80),
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,         // Можно сделать прозрачным, если фон не мешает
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Score: 0"
            };
            this.Controls.Add(_scoreLabel);

            // Чтобы лейбл был поверх всего
            _scoreLabel.BringToFront();
        }

        // Игровой цикл (вызывается каждый тик таймера)
        private void GameLoop(object sender, EventArgs e)
        {
            if (_game != null && !_game.IsGameOver)
            {
                _game.Update(); // ТВОЙ ГЛАВНЫЙ МЕТОД!

                // Анимация игрока (только если игра не окончена и игрок на земле — бежит)
                if (!_game.IsGameOver && !_game.Player.IsJumping)
                {
                    _frameCounter++;
                    // Меняем кадр каждые (60 / FramesPerSecond) тиков
                    if (_frameCounter >= 60 / FramesPerSecond)
                    {
                        _frameCounter = 0;
                        _currentFrame = (_currentFrame + 1) % _runmanFrames.Length;
                        _runmanImage = _runmanFrames[_currentFrame];
                    }
                }
                else
                {
                    // Если игрок в прыжке или игра окончена, можно показывать статичный кадр
                    _runmanImage = _runmanFrames[0]; // или специальный кадр прыжка
                }

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

            // Если игра окончена, рисуем затемнение и надпись
            if (_showGameOverScreen)
            {
                // Полупрозрачный черный фон
                using (var brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                {
                    g.FillRectangle(brush, 0, 0, this.Width, this.Height);
                }

                // Рисуем картинку Game Over (если есть)
                if (_gameOverImage != null)
                {
                    int imgX = (this.Width - _gameOverImage.Width) / 2;
                    int imgY = (this.Height - _gameOverImage.Height) / 2 - 50;
                    g.DrawImage(_gameOverImage, imgX, imgY);
                }

                // Рисуем текст (можно поверх картинки или отдельно)
                using (var font = new Font("Arial", 36, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    string text = "GAME OVER";
                    SizeF textSize = g.MeasureString(text, font);
                    float textX = (this.Width - textSize.Width) / 2;
                    float textY = (this.Height - textSize.Height) / 2;

                    // Тень
                    g.DrawString(text, font, new SolidBrush(Color.Black), textX + 3, textY + 3);
                    // Текст
                    g.DrawString(text, font, brush, textX, textY);
                }

                // Можно добавить инструкцию
                using (var font = new Font("Arial", 14))
                {
                    string smallText = "Нажми 'Новая игра'";
                    SizeF smallSize = g.MeasureString(smallText, font);
                    g.DrawString(smallText, font, new SolidBrush(Color.LightGray),
                        (this.Width - smallSize.Width) / 2, this.Height / 2 + 50);
                }
            }
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
