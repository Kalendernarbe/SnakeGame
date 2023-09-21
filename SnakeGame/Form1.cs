using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private int randI, randJ;
        private PictureBox fruit;
        private PictureBox[] snake = new PictureBox[400];

        private int dirX, dirY;
        private int windowWidth = 720;
        private int windowHeight = 610;
        private int blockSize = 30;

        private Label scoreLabel;
        private int score = 0;

        // Генерация игрового поля
        private void generateMap()
        {
            for (int i = 0; i <= windowWidth / blockSize - 5; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(0, blockSize * i);
                pictureBox.Size = new Size(windowWidth - 120, 1);
                this.Controls.Add(pictureBox);
            }
            for (int i = 0; i <= windowHeight / blockSize; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(blockSize * i, 0);
                pictureBox.Size = new Size(1, windowWidth - 150);
                this.Controls.Add(pictureBox);
            }
        }

        // Генерация фрукта в рандомном месте
        private void generateFruit()
        {
            Random rand = new Random();
            randI = rand.Next(0, windowWidth - 120 - blockSize);
            int tempI = randI % blockSize;
            randI -= tempI;
            randJ = rand.Next(0, windowWidth - 150 - blockSize);
            int tempJ = randJ % blockSize;
            randJ -= tempJ;
            fruit.Location = new Point(randI + 3, randJ + 3);
            fruit.Region = new Region(getGraphicsPath(blockSize - 6, blockSize - 6));
            this.Controls.Add(fruit);
        }

        // Возвращает графический путь для круглой формы
        private GraphicsPath getGraphicsPath(int width, int height)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, width, height);
            return path;
        }

        // Создание головы змейки (1-ый элемент)
        private void createElemSnake(int index, int x, int y)
        {
            snake[index] = new PictureBox();
            snake[index].Location = new Point(x, y);
            snake[index].Size = new Size(blockSize, blockSize);
            snake[index].BackColor = Color.CadetBlue;
            snake[index].Region = new Region(getGraphicsPath(blockSize, blockSize));
        }

        // Настройка показателя очков
        private void setScoreLabel()
        {
            scoreLabel = new Label();
            scoreLabel.Text = "S\nC\nO\nR\nE\n. .\n\n" + score;
            scoreLabel.TextAlign = ContentAlignment.MiddleCenter;
            scoreLabel.Size = new Size(50, 250);
            scoreLabel.Location = new Point(630, 150);
            scoreLabel.ForeColor = Color.DarkRed;
            scoreLabel.BackColor = Color.LightSkyBlue;
            scoreLabel.Font = new Font(scoreLabel.Font.FontFamily, 18, FontStyle.Bold);
            this.Controls.Add(scoreLabel);
        }

        // Настройка заднего фона общей формы
        private void SetFormBackgroundColor(Color color, int x, int y, int width, int height)
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Rectangle rectangle = new Rectangle(x, y, width, height);

                // Заливаем прямоугольник выбранным цветом
                using (SolidBrush brush = new SolidBrush(color))
                {
                    graphics.FillRectangle(brush, rectangle);
                }
            }
            this.BackgroundImage = bitmap;
        }

        public Form1()
        {
            InitializeComponent();
            this.Text = "Snake Game";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.CadetBlue;
            this.Width = windowWidth;
            this.Height = windowHeight;
            dirX = 1;
            dirY = 0;

            SetFormBackgroundColor(Color.LightYellow, 0, 0, 600, 570);
            setScoreLabel();
            createElemSnake(0, 300, 300);
            this.Controls.Add(snake[0]);

            fruit = new PictureBox();
            fruit.BackColor = Color.Red;

            generateMap();
            generateFruit();

            // Добавление таймера
            timer.Tick += new EventHandler(update);
            timer.Interval = 250;
            timer.Start();

            // Задание свойства Region PictureBox
            this.KeyDown += new KeyEventHandler(OKP);

            fruit.Region = new Region(getGraphicsPath(blockSize - 6, blockSize - 6));
        } 

        // Процесс поедания фрукта (увеличение score, генерация нового объекта )
        private void eatFruit()
        {
            if (snake[0].Location.X == randI && snake[0].Location.Y == randJ)
            {
                scoreLabel.Text = "S\nC\nO\nR\nE\n. .\n\n" + ++score;
                createElemSnake(score, snake[score - 1].Location.X + dirX * blockSize, snake[score - 1].Location.Y - dirY * blockSize);
                this.Controls.Add(snake[score]);
                generateFruit();
            }
        }

        // Поедание себя же
        private void eatItself()
        {
            for(int i = 1; i < score; i++)
            {
                if (snake[0].Location == snake[i].Location)
                {
                    for(int j = i;  j <= score; j++)
                        this.Controls.Remove(snake[j]);
                    score = score - (score - i + 1);
                    scoreLabel.Text = "S\nC\nO\nR\nE\n. .\n\n" + score;
                }
            }
        }

        // Проверка на выход за границы
        private void checkBorders()
        {
            if (snake[0].Location.X < 0)
            {
                for(int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                scoreLabel.Text = "S\nC\nO\nR\nE\n. .\n\n" + score;
                dirX = 1;
            }
            if (snake[0].Location.X > windowHeight - blockSize)
            {
                for(int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                scoreLabel.Text = "S\nC\nO\nR\nE\n. .\n\n" + score;
                dirX = -1;
            }
            if (snake[0].Location.Y < 0)
            {
                for(int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                scoreLabel.Text = "S\nC\nO\nR\nE\n. .\n\n" + score;
                dirY = 1;
            }
            if (snake[0].Location.Y > windowHeight - blockSize * 2)
            {
                for(int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                scoreLabel.Text = "S\nC\nO\nR\nE\n. .\n\n" + score;
                dirY = -1;
            }
        }

        // Изменение положения всей структуры змеи
        private void moveSnake()
        {
            //for ( int i = score; i >= 0; i--)
            //{
            //    snake[i].Location = new Point(snake[i].Location.X + dirX * blockSize, snake[i].Location.Y + dirY * blockSize);
            //}
            for ( int i = score; i >= 1; i--)
            {
                snake[i].Location =snake[i - 1].Location;
            }
            snake[0].Location = new Point(snake[0].Location.X + dirX * (blockSize), snake[0].Location.Y + dirY * blockSize);
            eatItself();
        }

        // Обновление положения змейки
        private void update(Object sender, EventArgs eventArgs)
        {
            checkBorders();
            eatFruit();
            moveSnake();
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
