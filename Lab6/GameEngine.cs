using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms; 

namespace Lab6
{
    public class GameEngine
    {
        // width and height of the cell
        private const float block = 80 / 3;

        // init 2d array and instantiate it
        public enum CellSelection { N, O, X };
        public CellSelection[,] grid = new CellSelection[3, 3];

        // turn count
        public int turnCount;

        // check for comp turn
        public bool isComputersTurn;

        // default constructor
        public GameEngine()
        {
            turnCount = 0;
            isComputersTurn = false;
        }

        // check user click
        public GameEngine checkUserClick(MouseEventArgs e, PointF[] p, GameEngine current)
        {
            // handle on and off board clicks
            if (p[0].X < 0 || p[0].Y < 0) return current;
            int i = (int)(p[0].X / block);
            int j = (int)(p[0].Y / block);
            if (i > 2 || j > 2) return current;

            Console.WriteLine("{0}, {1}", i, j);

            // only allow setting empty cells on user turn
            if (grid[i, j] == CellSelection.N && !current.isComputersTurn)
            {
                // set X on left click
                if (e.Button == MouseButtons.Left)
                    grid[i, j] = CellSelection.X;

                // increment turn count
                current.turnCount++;

                // check for winner
                this.checkForWinner();

                // set computer's turn
                current.isComputersTurn = true;

                return current;
            }

            else
            {
                // display bad move alert
                MessageBox.Show("Invalid move.");
                return current;
            }
        }

        // handle the computer moves
        //public void computerMove
        //{

        //}

        // check for winner
        public bool checkForWinner()
        {
            return false;
        }
    }
}
