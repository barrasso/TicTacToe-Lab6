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
    }
}
