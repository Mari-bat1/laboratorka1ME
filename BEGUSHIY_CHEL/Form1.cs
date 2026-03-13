namespace BEGUSHIY_CHEL
{
    public partial class Form1 : Form
    {
        private Image _runmanImage;
        private Image _dangerImage;
        private Image _fonImage;

        private Label _scoreLabel;

        private Image _gameOverImage;
        private bool _showGameOverScreen = false;

        private Image[] _runmanFrames;      // кадры
        private int _currentFrame = 0;      // текущий кадр
        private int _frameCounter = 0;      // счетчик
        private const int FramesPerSecond = 10; // фпс

        private GameEngine _game;

        private System.Windows.Forms.Timer _gameTimer;



        public Form1()
        {
            InitializeComponent();
            this.Text = "БЕГУЩИЙ ЧЕЛ";
            this.Size = new Size(800, 500);
            this.DoubleBuffered = true; // убирает мерцание
            this.KeyPreview = true;

            _game = new GameEngine();

            _gameTimer = new System.Windows.Forms.Timer();
            _gameTimer.Interval = 16;
            _gameTimer.Tick += GameLoop;
            _gameTimer.Start();

            try
            {
                _runmanFrames = new Image[]
                {
                    Image.FromFile("runman1.png"),
                    Image.FromFile("runman2.png")
                };

                _dangerImage = Image.FromFile("danger.png");
                _fonImage = Image.FromFile("fon.png");

                _runmanImage = _runmanFrames[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить картинки: {ex.Message}. Будут использоваться заглушки.");
                //заглушки
                _dangerImage = new Bitmap(1, 1);
                _fonImage = new Bitmap(1, 1);

                MessageBox.Show($"Не удалось загрузить кадры анимации: {ex.Message}");
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
                    _showGameOverScreen = true;
                    this.Invalidate();
                });
            };

            var resetButton = new Button
            {
                Text = "Новая игра",
                Location = new System.Drawing.Point(10, 10),
                Size = new Size(100, 30)
            };
            resetButton.Click += (s, e) =>
            {
                _game.ResetGame();
                _showGameOverScreen = false;
                this.Invalidate();
            };
            this.Controls.Add(resetButton);

            _scoreLabel = new Label
            {
                Location = new System.Drawing.Point(550, 20),
                Size = new Size(200, 80),
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Score: 0"
            };
            this.Controls.Add(_scoreLabel);

            _scoreLabel.BringToFront();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (_game != null && !_game.IsGameOver)
            {
                _game.Update();

                if (!_game.IsGameOver && !_game.Player.IsJumping)
                {
                    _frameCounter++;
                    if (_frameCounter >= 60 / FramesPerSecond)
                    {
                        _frameCounter = 0;
                        _currentFrame = (_currentFrame + 1) % _runmanFrames.Length;
                        _runmanImage = _runmanFrames[_currentFrame];
                    }
                }
                else
                {
                    _runmanImage = _runmanFrames[0];
                }

                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_game == null) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            //фон
            if (_fonImage != null && _fonImage.Width > 1)
            {
                g.DrawImage(_fonImage, 0, 0, this.Width, this.Height);
            }
            else
            {
                using (var brush = new SolidBrush(Color.SkyBlue))
                {
                    g.FillRectangle(brush, 0, 0, this.Width, _game.Player.Y + _game.Player.Height);
                }
                using (var brush = new SolidBrush(Color.Green))
                {
                    g.FillRectangle(brush, 0, _game.Player.Y + _game.Player.Height, this.Width, this.Height);
                }
            }

            int groundLevel = _game.Player.Y + _game.Player.Height;

            //препятсвия
            if (_dangerImage != null && _dangerImage.Width > 1)
            {
                foreach (var obstacle in _game.Obstacles)
                {
                    g.DrawImage(_dangerImage,
                        new Rectangle(obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height));
                }
            }
            else
            {
                using (var brush = new SolidBrush(Color.Blue))
                {
                    foreach (var obstacle in _game.Obstacles)
                    {
                        g.FillRectangle(brush, obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height);
                    }
                }
            }

            //игрок
            if (_runmanImage != null && _runmanImage.Width > 1)
            {
                g.DrawImage(_runmanImage,
                    new Rectangle(_game.Player.X, _game.Player.Y, _game.Player.Width, _game.Player.Height));
            }
            else
            {
                using (var brush = new SolidBrush(Color.Red))
                {
                    g.FillRectangle(brush, _game.Player.X, _game.Player.Y, _game.Player.Width, _game.Player.Height);
                }
            }

            if (_showGameOverScreen)
            {
                using (var brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                {
                    g.FillRectangle(brush, 0, 0, this.Width, this.Height);
                }

                if (_gameOverImage != null)
                {
                    int imgX = (this.Width - _gameOverImage.Width) / 2;
                    int imgY = (this.Height - _gameOverImage.Height) / 2 - 50;
                    g.DrawImage(_gameOverImage, imgX, imgY);
                }

                using (var font = new Font("Arial", 36, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    string text = "GAME OVER";
                    SizeF textSize = g.MeasureString(text, font);
                    float textX = (this.Width - textSize.Width) / 2;
                    float textY = (this.Height - textSize.Height) / 2;

                    g.DrawString(text, font, new SolidBrush(Color.Black), textX + 3, textY + 3);
                    g.DrawString(text, font, brush, textX, textY);
                }

                using (var font = new Font("Arial", 14))
                {
                    string smallText = "Нажми 'Новая игра'";
                    SizeF smallSize = g.MeasureString(smallText, font);
                    g.DrawString(smallText, font, new SolidBrush(Color.LightGray),
                        (this.Width - smallSize.Width) / 2, this.Height / 2 + 50);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _game.Jump();
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }
    }
}
