using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {

        private int windowWidth = 615;
        private int windowHeight = 610;
        private int blockSize = 30;

        public Form1()
        {
            InitializeComponent();
            this.Width = windowWidth;
            this.Height = windowHeight;
            generateMap();

            int blockWidth = snake.Width - 5;
            int blockHeight = snake.Height - 5;

            // Задание свойства Region PictureBox
            snake.Region = new Region(getGraphicsPath(blockWidth, blockHeight));
            
            this.KeyDown += new KeyEventHandler(OKP);
        }

        // Возвращает графический путь для круглой формы
        private GraphicsPath getGraphicsPath(int width, int height)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, width, height);
            return path;
        }

        // Генерация игрового поля
        private void generateMap()
        {
            for (int i = 0; i <= windowWidth/blockSize - 1; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(0, blockSize * i);
                pictureBox.Size = new Size(windowWidth - 15, 1);
                this.Controls.Add(pictureBox);
            }
            for (int i = 0; i <= windowHeight / blockSize; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(blockSize * i, 0);
                pictureBox.Size = new Size(1, windowWidth - 45);
                this.Controls.Add(pictureBox);
            }
        }

        private void OKP(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "W":
                case "Up":
                    snake.Location = new Point(snake.Location.X, snake.Location.Y - blockSize);
                    break;
                case "A":
                case "Left":
                    snake.Location = new Point(snake.Location.X - blockSize, snake.Location.Y);
                    break;
                case "D":
                case "Right":
                    snake.Location = new Point(snake.Location.X + blockSize, snake.Location.Y);
                    break;
                case "S":
                case "Down":
                    snake.Location = new Point(snake.Location.X, snake.Location.Y + blockSize);
                    break;
            }
        }
    }
}
