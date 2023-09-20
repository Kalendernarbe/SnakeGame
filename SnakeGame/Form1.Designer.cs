namespace SnakeGame
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.snake = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.snake)).BeginInit();
            this.SuspendLayout();
            // 
            // snake
            // 
            this.snake.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.snake.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.snake.Location = new System.Drawing.Point(0, 0);
            this.snake.Name = "snake";
            this.snake.Size = new System.Drawing.Size(35, 35);
            this.snake.TabIndex = 0;
            this.snake.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 450);
            this.Controls.Add(this.snake);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Name = "Form1";
            this.Text = "Game \"Snake\"";
            ((System.ComponentModel.ISupportInitialize)(this.snake)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox snake;
    }
}

