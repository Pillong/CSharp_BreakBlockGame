using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout_Game
{

    public partial class Form1 : Form
    {
        int blockCount = 72;
        bool moveLeft;
        bool moveRight;
        bool isGameOver;
        int score, ballX, ballY, playerSpeed;

        Random random = new Random();

        PictureBox[] blockArray;


        public Form1()
        {
            InitializeComponent();
            PlaceBlocks();
        }


        private void setupGame()
        {
            
            isGameOver = false;
            score = 0;
            ballX = 5;
            ballY = 5;
            playerSpeed = 12;
            txtScore.Text = "Score : " + score;

            Ball.Left = 376;
            Ball.Top = 250;
            Player.Left = 347;
            gameTimer.Start();

            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "blocks")
                {
                    x.BackColor = Color.FromArgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));
                }
            }
        }

        private void gameOver()
        {
            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = "Score : " + score;
        }

        private void PlaceBlocks()
        {
            
            blockArray = new PictureBox[blockCount];
            int a = 0;
            int top = 10;
            int left = 100;

            for (int i = 0; i < blockArray.Length; i++)
            {
                blockArray[i] = new PictureBox();
                blockArray[i].Height = 25;
                blockArray[i].Width = 81;
                blockArray[i].Tag = "blocks";
                blockArray[i].BackColor = Color.White;

                if (a == 9)
                {
                    top = top + 30;
                    left = 100;
                    a = 0;
                }

                if (a < 9)
                {
                    a++;
                    blockArray[i].Left = left;
                    blockArray[i].Top = top;
                    this.Controls.Add(blockArray[i]);
                    left = left + 100;
                }
            }
            setupGame();
        }

        private void removeBlocks()
        {
            foreach(PictureBox x in blockArray)
            {
                this.Controls.Remove(x);
            }
        }

        private void Ball_Click(object sender, EventArgs e)
        {

        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score : " + score;

            if(moveLeft == true &&  Player.Left > 0)
            {
                Player.Left -= playerSpeed;
            }
            if(moveRight == true &&  Player.Left < 700)
            {
                Player.Left += playerSpeed;
            }

            Ball.Left += ballX;
            Ball.Top += ballY;

            if(Ball.Left < 0 || Ball.Left > 775)
            {
                ballX = -ballX;
            }
            if(Ball.Top < 0)
            {
                ballY = -ballY;
            }

            if(Ball.Bounds.IntersectsWith(Player.Bounds))
            {

                Ball.Top = 390;

                ballY = random.Next(8, 12) * -1;
                
                if(ballX < 0)
                {
                    ballX = random.Next(8, 12) * -1;
                }
                else
                {
                    ballX = random.Next(-8, 12);
                }
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    if(Ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1;
                        ballY = -ballY;
                        this.Controls.Remove(x);

                    }
                }
            }

            if(score == blockCount)
            {
                gameOver();
                MessageBox.Show("Your Score : " + score + ".\nPress 'Enter' to play Again", "You Win!!");

            }
            if(Ball.Top > 470)
            {
                gameOver();
                MessageBox.Show("Your Score : " + score + ".\nPress 'Enter' to try Again", "You Lose..");
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                moveLeft = true;
            }
            if(e.KeyCode == Keys.Right)
            {
                moveRight = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                moveRight = false;
            }
            if(e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeBlocks();
                PlaceBlocks();
            }
        }
    }
}
