namespace interfaceforrunninngman
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dinosaur = new System.Windows.Forms.Label();
            this.cactus = new System.Windows.Forms.Label();
            this.ground = new System.Windows.Forms.Label();
            this.timer1 = new System.Timers.Timer();
            this.scores_label = new System.Windows.Forms.Label();
            this.lose_label = new System.Windows.Forms.Label();
            this.lose_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
            this.SuspendLayout();
            // 
            // dinosaur
            // 
            this.dinosaur.BackColor = System.Drawing.Color.LightSlateGray;
            this.dinosaur.Image = ((System.Drawing.Image)(resources.GetObject("dinosaur.Image")));
            this.dinosaur.Location = new System.Drawing.Point(10, 170);
            this.dinosaur.Name = "dinosaur";
            this.dinosaur.Size = new System.Drawing.Size(50, 50);
            this.dinosaur.TabIndex = 0;
            this.dinosaur.Click += new System.EventHandler(this.dinosaur_Click);
            // 
            // cactus
            // 
            this.cactus.BackColor = System.Drawing.Color.ForestGreen;
            this.cactus.Image = ((System.Drawing.Image)(resources.GetObject("cactus.Image")));
            this.cactus.Location = new System.Drawing.Point(590, 190);
            this.cactus.Name = "cactus";
            this.cactus.Size = new System.Drawing.Size(20, 30);
            this.cactus.TabIndex = 1;
            this.cactus.Click += new System.EventHandler(this.cactus_Click);
            // 
            // ground
            // 
            this.ground.BackColor = System.Drawing.Color.SaddleBrown;
            this.ground.Image = ((System.Drawing.Image)(resources.GetObject("ground.Image")));
            this.ground.Location = new System.Drawing.Point(10, 220);
            this.ground.Name = "ground";
            this.ground.Size = new System.Drawing.Size(600, 30);
            this.ground.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Interval = 10D;
            this.timer1.SynchronizingObject = this;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
            // 
            // scores_label
            // 
            this.scores_label.Location = new System.Drawing.Point(569, 9);
            this.scores_label.Name = "scores_label";
            this.scores_label.Size = new System.Drawing.Size(40, 20);
            this.scores_label.TabIndex = 3;
            // 
            // lose_label
            // 
            this.lose_label.ForeColor = System.Drawing.Color.Black;
            this.lose_label.Location = new System.Drawing.Point(10, 10);
            this.lose_label.Name = "lose_label";
            this.lose_label.Size = new System.Drawing.Size(600, 240);
            this.lose_label.TabIndex = 4;
            this.lose_label.Text = "Вы проиграли!";
            this.lose_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lose_button
            // 
            this.lose_button.CausesValidation = false;
            this.lose_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lose_button.ForeColor = System.Drawing.Color.Black;
            this.lose_button.Location = new System.Drawing.Point(10, 220);
            this.lose_button.Name = "lose_button";
            this.lose_button.Size = new System.Drawing.Size(600, 30);
            this.lose_button.TabIndex = 5;
            this.lose_button.TabStop = false;
            this.lose_button.Text = "Повторить";
            this.lose_button.UseVisualStyleBackColor = true;
            this.lose_button.Click += new System.EventHandler(this.lose_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 262);
            this.Controls.Add(this.lose_button);
            this.Controls.Add(this.lose_label);
            this.Controls.Add(this.scores_label);
            this.Controls.Add(this.ground);
            this.Controls.Add(this.cactus);
            this.Controls.Add(this.dinosaur);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "Form1";
            this.Text = "Dino Run";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button lose_button;

        private System.Windows.Forms.Label lose_label;

        private System.Windows.Forms.Label scores_label;

        private System.Timers.Timer timer1;

        private System.Windows.Forms.Label cactus;
        private System.Windows.Forms.Label ground;

        private System.Windows.Forms.Label dinosaur;

        #endregion
    }
}
