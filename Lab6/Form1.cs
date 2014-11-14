using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        // board dimensions
        private const float clientSize = 100;
        private const float lineLength = 80;

        // width and height of the cell
        private const float block = lineLength / 3;

        // position of upper left hand corner of board
        private const float offset = 10;

        // inside border of X or O
        private const float delta = 5;

        // init 2d array and instantiate it
        private enum CellSelection { N, O, X };
        private CellSelection[,] grid = new CellSelection[3, 3];

        // scale factor
        private float scale;

        public Form1()
        {
            InitializeComponent();

            // resize redraw
            ResizeRedraw = true;

            // get status of grid from game engine
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // get graphics object
            Graphics g = e.Graphics;

            // apply client transform
            ApplyTransform(g);

            // draw the board
             g.DrawLine(Pens.Black, block, 0, block, lineLength); 
             g.DrawLine(Pens.Black, 2*block, 0, 2*block, lineLength); 
             g.DrawLine(Pens.Black, 0, block, lineLength, block); 
             g.DrawLine(Pens.Black, 0, 2*block, lineLength, 2*block); 
             
            // draw O's and X's at specified coords within 3x3 grid
            for (int i = 0; i < 3; ++i) 
                for (int j = 0; j < 3; ++j) 
                    if (grid[i, j] == CellSelection.O) DrawO(i, j, g);
                    else if (grid[i, j] == CellSelection.X) DrawX(i, j,
                    g); 
        }

        // Draw X Method
        private void DrawX(int i, int j, Graphics g)
        {
            g.DrawLine(Pens.Black, i * block + delta, j * block + delta,
            (i * block) + block - delta, (j * block) + block - delta);
            g.DrawLine(Pens.Black, (i * block) + block - delta,
            j * block + delta, (i * block) + delta, (j * block) + block - delta);
        }

        // Draw O Method
        private void DrawO(int i, int j, Graphics g)
        {
            g.DrawEllipse(Pens.Black, i * block + delta, j * block + delta,
            block - 2 * delta, block - 2 * delta);
        } 

        // Apply Transform
        private void ApplyTransform(Graphics g)
        {
            // adjust board size based on client size
            scale = Math.Min(ClientRectangle.Width / clientSize,
            ClientRectangle.Height / clientSize);
            if (scale == 0f) return;
            g.ScaleTransform(scale, scale);
            g.TranslateTransform(offset, offset);
        } 
    }
}
