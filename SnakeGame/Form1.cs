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
        private int randI, randJ;
        private PictureBox fruit;
        private int dirX, dirY;
        private int windowWidth = 615;
        private int windowHeight = 610;
        private int blockSize = 30;

        public Form1()
        {
            InitializeComponent();
            this.Width = windowWidth;
            this.Height = windowHeight;
            dirX = 1; 
            dirY = 0;
            fruit = new PictureBox();
            fruit.BackColor = Color.Red;
            fruit.Size = new Size(blockSize, blockSize);
            generateMap();
            generateFruit();

            // Добавление таймера
            timer.Tick += new EventHandler(Update);
            timer.Interval = 500;
            timer.Start();

            int blockWidth = snake.Width - 5;
            int blockHeight = snake.Height - 5;

            // Задание свойства Region PictureBox
            snake.Region = new Region(getGraphicsPath(blockWidth, blockHeight));
            
            this.KeyDown += new KeyEventHandler(OKP);

            fruit.Region = new Region(getGraphicsPath(blockWidth, blockHeight));
        }

        // Возвращает графический путь для круглой формы
        private GraphicsPath getGraphicsPath(int width, int height)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, width, height);
            return path;
        }

        // Генерация фрукта в рандомном месте
        private void generateFruit()
        {
            Random rand = new Random();
            randI = rand.Next(0, windowWidth - blockSize);
            int tempI = randI % blockSize;
            randI -= tempI;
            randJ = rand.Next(0, windowWidth - blockSize);
            int tempJ = randJ % blockSize;
            randJ -= tempJ;
            fruit.Location = new Point(randI, randJ);
            this.Controls.Add(fruit);
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

        // Обовление положения змейки
        private void Update(Object sender, EventArgs eventArgs)
        {
            snake.Location = new Point(snake.Location.X + dirX * blockSize, snake.Location.Y + dirY * blockSize);
        }

        private void OKP(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "W":
                case "Up":
                    dirX = 0;
                    dirY = -1;
                    break;
                case "A":
                case "Left":
                    dirX = -1;
                    dirY = 0;
                    break;
                case "D":
                case "Right":
                    dirY = 0;
                    dirX = 1;
                    break;
                case "S":
                case "Down":
                    dirY = 1;
                    dirX = 0;
                    break;
            }
        }
    }
}
