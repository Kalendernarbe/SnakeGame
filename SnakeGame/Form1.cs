﻿using System;
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
        private int windowWidth = 700;
        private int windowHeight = 610;
        private int blockSize = 30;

        private Label scoreLabel;
        private int score = 0;

        // Генерация игрового поля
        private void generateMap()
        {
            for (int i = 0; i <= windowWidth / blockSize - 4; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(0, blockSize * i);
                pictureBox.Size = new Size(windowWidth - 100, 1);
                this.Controls.Add(pictureBox);
            }
            for (int i = 0; i <= windowHeight / blockSize; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(blockSize * i, 0);
                pictureBox.Size = new Size(1, windowWidth - 130);
                this.Controls.Add(pictureBox);
            }
        }

        // Генерация фрукта в рандомном месте
        private void generateFruit()
        {
            Random rand = new Random();
            randI = rand.Next(0, windowWidth - 100 - blockSize);
            int tempI = randI % blockSize;
            randI -= tempI;
            randJ = rand.Next(0, windowWidth - 100 - blockSize);
            int tempJ = randJ % blockSize;
            randJ -= tempJ;
            fruit.Location = new Point(randI, randJ);
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
            scoreLabel.Size = new Size(50, 200);
            scoreLabel.Location = new Point(630, 150);
            scoreLabel.ForeColor = Color.MediumPurple;
            scoreLabel.Font = new Font(scoreLabel.Font.FontFamily, 14, scoreLabel.Font.Style);
            this.Controls.Add(scoreLabel);
        }

        public Form1()
        {
            InitializeComponent();
            this.Width = windowWidth;
            this.Height = windowHeight;
            dirX = 1;
            dirY = 0;

            setScoreLabel();
            createElemSnake(0, 300, 300);
            this.Controls.Add(snake[0]);

            fruit = new PictureBox();
            fruit.BackColor = Color.Red;
            fruit.Size = new Size(blockSize, blockSize);
            generateMap();
            generateFruit();

            // Добавление таймера
            timer.Tick += new EventHandler(update);
            timer.Interval = 300;
            timer.Start();

            // Задание свойства Region PictureBox
            this.KeyDown += new KeyEventHandler(OKP);

            fruit.Region = new Region(getGraphicsPath(blockSize, blockSize));
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

        // Изменение положения всей структуры змеи
        private void moveSnake()
        {
            for ( int i = score; i >= 0; i--)
            {
                snake[i].Location = new Point(snake[i].Location.X + dirX * blockSize, snake[i].Location.Y + dirY * blockSize);
            }
        }

        // Обовление положения змейки
        private void update(Object sender, EventArgs eventArgs)
        {
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
