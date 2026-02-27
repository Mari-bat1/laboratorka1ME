using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Dino_Run
{
  public partial class Form1 : Form
  {
    public Random rnd = new Random();
    public int cacti_x;
    public int dino_y;
    public int scores;
    
    public Form1()
    {
      InitializeComponent();
    }
    private void Form1_Load(object sender, EventArgs e)
    {
      lose_label.Visible = false;
      lose_button.Visible = false;
      lose_button.Enabled = false;
      timer1.Enabled = true;
      scores = 0;
      cacti_x = rnd.Next(480, 590);
      cactus.Location = new Point(cacti_x, 190);
      dino_y = 170;
    }

    private void timer1_Elapsed(object sender, ElapsedEventArgs e)
    {
      // Движение кактуса, переспавн кактуса и засчёт очков.
      if (cactus.Location.X < ground.Location.X)
      {
        cacti_x = rnd.Next(550, 590);
        scores += 1;
        scores_label.Text = Convert.ToString(scores);
      }
      else
        cacti_x -= 5;
      cactus.Location = new Point(cacti_x, 190);
      
      // Прыжок динозавра.
      if (dinosaur.Location.Y == 170)
        if (key == Keys.Space)
          dino_y = 110;
      if (dinosaur.Location.Y < 170)
        dino_y += 1;
      dinosaur.Location = new Point(10, dino_y);

      // Проверка столкновения
      if (cacti_x < 60 && dino_y + 50 > 190)
      {
        lose_label.Text += "\n" + "Ваши очки: " + Convert.ToString(scores) + ".";
        lose_label.Visible = true;
        lose_button.Visible = true;
        lose_button.Enabled = true;
        timer1.Enabled = false;
      }
    }

    Keys key;
    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
      key = e.KeyCode;
    }
    private void Form1_KeyUp(object sender, KeyEventArgs e)
    {
      key = Keys.None;
    }
    private void dinosaur_Click(object sender, EventArgs e){}
    private void cactus_Click(object sender, EventArgs e){}

    private void lose_button_Click(object sender, EventArgs e)
    {
      lose_button.Visible = false;
      lose_button.Enabled = false;
      lose_label.Text = "Вы проиграли!";
      lose_label.Visible = false;
      timer1.Enabled = true;
      scores = 0;
      scores_label.Text = Convert.ToString(scores);
      cacti_x = rnd.Next(550, 590);
      dino_y = 170;
    }
  }
}
