using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

//Kashish Dhanoa
//January 7th 2020 
//ICS3U- Mr. T
//Simple air hockey game.
namespace AirHockeySummative
{
    public partial class Form1 : Form
    {
        //global variables 

        int striker1X = 20;//striker 1 info
        int striker1Y = 120;
        int player1Score = 0;

        int striker2X = 355;//striker 2 info
        int striker2Y = 120;
        int player2Score = 0;

        int strikerHeight = 25;//striker dimensions + speed
        int strikerWidth = 25;
        int strikerSpeed = 5;

        int puckX = 195;//puck location, dimensions & speed
        int puckY = 120;
        int puckWidth = 20;
        int puckHeight = 20;
        int puckXSpeed = 5;
        int puckYSpeed = -5;

        //P2 keys
        bool wDown = false;
        bool sDown = false;
        bool aDown = false;
        bool dDown = false;
        //P2 keys
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftArrowDown = false;
        bool rightArrowDown = false;

        Pen borderPen = new Pen(Color.White, 5);
        Pen netPen = new Pen(Color.White, 15);
        SolidBrush tealBrush = new SolidBrush(Color.Teal);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush grayBrush = new SolidBrush(Color.Gray);
        Font screenFont = new Font("Consolas", 12);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SoundPlayer player;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //switch code to get each key true when pressed
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;//up
                    break;
                case Keys.S:
                    sDown = true;//down
                    break;
                case Keys.A:
                    aDown = true;//left
                    break;
                case Keys.D:
                    dDown = true;//right
                    break;
                case Keys.Up:
                    upArrowDown = true;//p2 up
                    break;
                case Keys.Down:
                    downArrowDown = true;// p2 down
                    break;
                case Keys.Left:
                    leftArrowDown = true;//p2 left
                    break;
                case Keys.Right:
                    rightArrowDown = true;//p2 right
                    break;

            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;//up
                    break;
                case Keys.S:
                    sDown = false;//down
                    break;
                case Keys.A:
                    aDown = false;//left
                    break;
                case Keys.D:
                    dDown = false;//right
                    break;
                case Keys.Up:
                    upArrowDown = false;//p2 up
                    break;
                case Keys.Down:
                    downArrowDown = false;// p2 down
                    break;
                case Keys.Left:
                    leftArrowDown = false;//p2 left
                    break;
                case Keys.Right:
                    rightArrowDown = false;//p2 right
                    break;
            }
        }
        
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move puck
            puckX += puckXSpeed;
            puckY += puckYSpeed;

            //move player 1 (TEAL)
            if (wDown == true && striker1Y > 0)
            {
                striker1Y -= strikerSpeed;//move player 1 up
            }

            if (sDown == true && striker1Y < this.Height - strikerHeight)
            {
                striker1Y += strikerSpeed;//move player 1 down
            }

            if (aDown == true && striker1X > 0)
            {
                striker1X -= strikerSpeed;//move player 1 left
            }

            if (dDown == true && striker1X < this.Width - strikerWidth - 200)// P1 (teal) can't move past half
            {
                striker1X += strikerSpeed;//move player 1 right
            }

            //move player 2 (RED)
            if (upArrowDown == true && striker2Y > 0)
            {
                striker2Y -= strikerSpeed;//move player 2 up
            }

            if (downArrowDown == true && striker2Y < this.Height - strikerHeight)
            {
                striker2Y += strikerSpeed;//move player 2 down
            }

            if (leftArrowDown == true && striker2X > 200)// P2 (red) can't move past half
            {
                striker2X -= strikerSpeed;//move player 2 left
            }

            if (rightArrowDown == true && striker2X < this.Width - strikerWidth)
            {
                striker2X += strikerSpeed;//move player 2 right
            }

            //top and bottom wall collision
            if (puckY < 0 || puckY > this.Height - puckHeight)
            {
                puckYSpeed *= -1;  // or: ballYSpeed = -ballYSpeed; directions- negative is left, positive is right 
                player = new SoundPlayer(Properties.Resources.collision);
                player.Play();
            }
            //create Rectangles of objects on screen to be used for collision detection
            Rectangle striker1Rec = new Rectangle(striker1X, striker1Y, strikerWidth, strikerHeight);
            Rectangle striker2Rec = new Rectangle(striker2X, striker2Y, strikerWidth, strikerHeight);
            Rectangle puckRec = new Rectangle(puckX, puckY, puckWidth, puckHeight);


            //striker intersecting with ball
            if (striker1Rec.IntersectsWith(puckRec))
            {
                puckXSpeed *= -1;
                puckX = striker1X + strikerWidth + 1;
                player = new SoundPlayer(Properties.Resources.intersect);
                player.Play();
            }
            else if (striker2Rec.IntersectsWith(puckRec))
            {
                puckXSpeed *= -1;
                puckX = striker2X - strikerWidth - 1;
                player = new SoundPlayer(Properties.Resources.intersect);
                player.Play();
            }

            //puck intersecting with left "net" and passing with increase in score
            //puck boucing off the corners
            if (puckX < 0)
            {
                puckXSpeed *= -1;
                player = new SoundPlayer(Properties.Resources.collision);
                player.Play();
            }
            if (puckX < 0 && puckY >= 50 && puckY <= 225)
            {
                player = new SoundPlayer(Properties.Resources.GOAL);
                player.Play(); 
                
                player2Score++;

                puckX = 195;
                puckY = 120;

                striker1Y = 120;
                striker2Y = 120;
            }

            //puck intersecting with right "net" and passing with increase in score
            //puck boucing off the corners

            if (puckX > 380)
            {
                puckXSpeed *= -1;
                player = new SoundPlayer(Properties.Resources.collision);
                player.Play();
            }
            if (puckX > 380 && puckY >= 50 && puckY <= 225)// net areas
            {
                player = new SoundPlayer(Properties.Resources.GOAL);
                player.Play();
                player1Score++;

                puckX = 195;
                puckY = 120;

                striker1Y = 120;
                striker2Y = 120;
            }


            if (player1Score == 3 || player2Score == 3)
            {
                puckX = 195;
                puckY = 120;

                striker1Y = 120;
                striker2Y = 120; 
                
                gameTimer.Enabled = false;
                 
                if (player1Score == 3)
                {
                    player = new SoundPlayer(Properties.Resources.GOAL);
                    player.Play();
                    winLabel.Text = "BLUE WINS!";
                }
                else if (player2Score == 3)
                {
                    player = new SoundPlayer(Properties.Resources.GOAL);
                    player.Play();
                    winLabel.Text = "RED WINS!";
                }
            }

            Refresh();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw player 1 and 2 with white borders
            e.Graphics.FillEllipse(tealBrush, striker1X, striker1Y, strikerWidth, strikerHeight);
            e.Graphics.FillEllipse(redBrush, striker2X, striker2Y, strikerWidth, strikerHeight);
            e.Graphics.DrawEllipse(borderPen, striker1X, striker1Y, strikerWidth, strikerHeight);
            e.Graphics.DrawEllipse(borderPen, striker2X, striker2Y, strikerWidth, strikerHeight);
            
            //draw nets and center line
            e.Graphics.DrawLine(borderPen, 200, 0, 200, 400);
            e.Graphics.DrawLine(netPen, 398, 50, 398, 225);
            e.Graphics.DrawLine(netPen, 2, 50, 2, 225);

            //draw puck
            e.Graphics.FillEllipse(grayBrush, puckX, puckY, puckWidth, puckHeight);//fill
           
            //draw score
            e.Graphics.DrawString($"{player1Score}", screenFont, whiteBrush, 175, 10);
            e.Graphics.DrawString($"{player2Score}", screenFont, whiteBrush, 210, 10);


        }
    }
}
