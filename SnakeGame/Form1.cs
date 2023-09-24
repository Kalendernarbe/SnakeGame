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
        private Color[] snakeColors = { Color.SaddleBrown, Color.DimGray, Color.SlateGray };

        private int dirX, dirY;
        private int windowWidth = 720;
        private int windowHeight = 610;
        private int blockSize = 30;

        private Label scoreLabel, topScoreLabel;
        private int score = 0;
        private int topScore = 0;

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

        // Генерация фрукта в свободном месте
        private void generateFruit()
        {
            Random rand = new Random();
            bool intersection = true;

            while (intersection)
            {
                randI = rand.Next(0, windowWidth - 120 - blockSize);
                int tempI = randI % blockSize;
                randI -= tempI;
                randJ = rand.Next(0, windowWidth - 150 - blockSize);
                int tempJ = randJ % blockSize;
                randJ -= tempJ;

                // Проверка пересеченияя с каждым элементом змейки
                intersection = false;
                for (int i = 0; i <= score; i++)
                {
                    if (snake[i].Location.X == randI && snake[i].Location.Y == randJ)
                    {
                        intersection = true;
                        break;
                    }
                }
            }

            fruit.Location = new Point(randI + 5, randJ + 5);
            fruit.Region = new Region(getGraphicsPath(blockSize - 10, blockSize - 10));
            this.Controls.Add(fruit);
        }

        // Возвращает графический путь для круглой формы
        private GraphicsPath getGraphicsPath(int width, int height)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, width, height);
            return path;
        }

        // Добавление в snake нового элемента змейки
        private void createElemSnake(int index, int x, int y)
        {
            snake[index] = new PictureBox();
            snake[index].Location = new Point(x, y);
            snake[index].Size = new Size(blockSize, blockSize);
            snake[index].BackColor = snakeColors[index % 3];
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

        // Настройка показателя логотипа игры
        private void addPicture()
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Location = new Point(605, 25);
            pictureBox.Size = new Size(100, 100);
            pictureBox.Image = Image.FromFile("../../snake.jpg");
            pictureBox.BackColor = Color.Transparent;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(pictureBox);
        }

        // Настройка показателя кубка с рекордом игры
        private void addPictureCup()
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Location = new Point(605, 445);
            pictureBox.Size = new Size(95, 95);
            pictureBox.Image = Image.FromFile("../../cup.jpg");
            pictureBox.BackColor = Color.Transparent;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(pictureBox);
        }

        // Настройка показателя рекорда
        private void setTopScore()
        {
            topScoreLabel  = new Label();
            topScoreLabel.Text = topScore.ToString();
            topScoreLabel.TextAlign = ContentAlignment.MiddleCenter;
            topScoreLabel.Size = new Size(33, 33);
            topScoreLabel.Location = new Point(636, 460);
            topScoreLabel.ForeColor = Color.DarkRed;
            topScoreLabel.BackColor = Color.Orange;
            topScoreLabel.Font = new Font(scoreLabel.Font.FontFamily, 12, FontStyle.Bold);
            this.Controls.Add(topScoreLabel);
        }

        // Настройка заднего фона формы
        private void SetFormBackgroundColor(Color[] colors, int x, int y, int width, int height, int lineHeight)
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                int lineWidth = colors.Length;

                // Заливка прямоугольников разными цветами
                for (int i = 0; i < lineWidth * 10; i++)
                {
                    Rectangle rectangle = new Rectangle(x, i * lineHeight + y, width, y + lineHeight * i + lineHeight);
                    using (SolidBrush brush = new SolidBrush(colors[i % 2]))
                    {
                        graphics.FillRectangle(brush, rectangle);
                    }
                }
            }
            this.BackgroundImage = bitmap;
        }

        // Запуск действия игры (настройки и создание объектов)
        private void start()
        {
            createElemSnake(0, 300, 300);
            this.Controls.Add(snake[0]);

            generateFruit();
            timer.Start();

            fruit.Region = new Region(getGraphicsPath(blockSize - 10, blockSize - 10));
        }

        public Form1()
        {
            InitializeComponent();
            this.Text = "Snake Game";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.BackColor = Color.CadetBlue;
            this.BackColor = Color.White;
            this.Width = windowWidth;
            this.Height = windowHeight;

            dirX = 1;
            dirY = 0;

            Color[] colors = { Color.GreenYellow, Color.PaleGreen };
            SetFormBackgroundColor(colors, 0, 0, 600, 570, 30);
            setScoreLabel();
            addPicture();
            addPictureCup();
            setTopScore();
            topScoreLabel.BringToFront();

            fruit = new PictureBox();
            fruit.BackColor = Color.Red;
            generateMap();

            // Задание свойства Region PictureBox
            this.KeyDown += new KeyEventHandler(OKP);

            // Добавление таймера
            timer.Tick += new EventHandler(update);
            timer.Interval = 150;

            start();
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
                    {
                        this.Controls.Remove(snake[j]);
                    }
                    score = score - (score - i + 1);
                    scoreLabel.Text = "S\nC\nO\nR\nE\n. .\n\n" + score;
                }
            }
        }

        // Удаление всех элементов змейки
        private void removeElem()
        {
            for (int i = 0; i <= score; i++)
            {
                this.Controls.Remove(snake[i]);
            }
        }

        // Обновление результата
        private void updateScore()
        {
            if (score > topScore) topScore = score;
            topScoreLabel.Text = topScore.ToString();
            score = 0;
            scoreLabel.Text = "S\nC\nO\nR\nE\n. .\n\n" + score;
        }

        // Вывод сообщения с возможностью завершения игры
        private void showRestartMessageBox()
        {
            DialogResult result = MessageBox.Show("Game over! Your score: " + score + "\nDo you want to restart?", "Game Over", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                removeElem();
                start();
            }
            else if (result == DialogResult.No)
            {
                Application.Exit();
            }
        }

        // Проверка на выход за границы
        private void checkBorders()
        {
            if (snake[0].Location.X < 0)
            {
                timer.Stop();
                showRestartMessageBox();
                updateScore();
                dirX = 1;
            }
            if (snake[0].Location.X > windowHeight - blockSize)
            {
                timer.Stop();
                showRestartMessageBox();
                updateScore();
                dirX = -1;
            }
            if (snake[0].Location.Y < 0)
            {
                timer.Stop();
                showRestartMessageBox();
                updateScore();
                dirY = 1;
            }
            if (snake[0].Location.Y > windowHeight - blockSize * 2)
            {
                timer.Stop();
                showRestartMessageBox();
                updateScore();
                dirY = -1;
            }
        }

        // Изменение положения всей структуры змеи
        private void moveSnake()
        {
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
