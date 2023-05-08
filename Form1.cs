using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulletHellGameJam
{
    public partial class Form1 : Form
    {
        public Bitmap canvas;
        Graphics canvasG;

        Boolean paused = false;

        GameState currentState = GameState.TitleScreen;
       
        World world = new World();

        public Form1()
        {
            InitializeComponent();
            canvas = new Bitmap(256, 240);
            canvasG = Graphics.FromImage(canvas);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            DoubleBuffered = true;

            ClientSize = new Size(512, 480);
            CenterToScreen();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.Black);

            int drawHeight = Math.Min(ClientSize.Width, ClientSize.Height);
            int drawX, drawY;
            drawX = (ClientSize.Width - drawHeight) / 2;
            drawY = (ClientSize.Height - drawHeight) / 2;

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;

            e.Graphics.DrawRectangle(Pens.LightGreen, drawX - 1, drawY - 1, drawHeight + 1, drawHeight + 1);
            e.Graphics.DrawImage(canvas, drawX, drawY, drawHeight, drawHeight);

            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (currentState == GameState.TitleScreen)
            {
                if(e.KeyCode == Keys.Space)
                {
                    currentState = GameState.Game;
                }
            }
            if (currentState == GameState.GameOver)
            {
                if(e.KeyCode == Keys.Escape)
                {
                    world = new World();
                    currentState = GameState.TitleScreen;
                }
            }
            if (currentState == GameState.Game)
            {
     world.player.KeyDown(e);
            }
       
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currentState == GameState.Game)
            {
world.Update();
            }
            if(world.isGameOver)
            {
                currentState = GameState.GameOver;
            }

            RenderCanvas();

            Invalidate();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(currentState == GameState.Game)
            {
                world.player.KeyUp(e);
            }
           
        }

        void RenderCanvas()
        {
            canvasG.Clear(Color.Black);
            if (currentState == GameState.TitleScreen)
            {
                canvasG.DrawString("Press [SPACE BAR] to start! " + Environment.NewLine + "Use WASD to move, [SPACE BAR] to shot.", SystemFonts.DefaultFont, Brushes.White, 8, 8);
            }
            if (currentState == GameState.Game)
            {
                world.Render(canvasG);
            }
            if (currentState == GameState.GameOver)
            {
                canvasG.DrawString("Game Over! " + Environment.NewLine + "Press [ESCAPE] to start again.", SystemFonts.DefaultFont, Brushes.White, 8, 8);
            }
           
            
        }
    }

    

    internal enum BulletSource {
        Player, 
        Enemy
}
    internal enum GameState
    {
        TitleScreen,
        Game,
        GameOver
    }
    
   
}
