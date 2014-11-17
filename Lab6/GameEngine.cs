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
        
        // check for game over
        public bool isGameOver;

        // check for comp turn
        public bool isComputersTurn;

        // default constructor
        public GameEngine()
        {
            turnCount = 0;
            isGameOver = false;
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
            if (grid[i, j] == CellSelection.N && !current.isComputersTurn && !isGameOver)
            {
                // set X on left click
                if (e.Button == MouseButtons.Left)
                    grid[i, j] = CellSelection.X;

                // increment turn count
                current.turnCount++;

                // check for winner
                if (this.checkForWinner()) return current;

                // set computer's turn
                current.isComputersTurn = true;

                if (current.isComputersTurn)
                    // computer make move
                    this.computerMove(current);

                return current;
            }

            else if (!isGameOver)
            {
                // display bad move alert
                MessageBox.Show("Invalid move.");
                return current;
            }

            else return current;
        }

        // handle the computer moves
        public void computerMove(GameEngine current)
        {
            // make O move
            bool madeMove = false;
            for (int i = 0; i < 3; ++i)
            { 
                for (int j = 0; j < 3; ++j)
                {
                    if (current.grid[i, j] == GameEngine.CellSelection.X)
                    {
                        // check for winning move cases


                        // check for defending move cases


                        // check standard move cases
                        // up case
                        if (j>=1)
                            if (current.grid[i, j - 1] == GameEngine.CellSelection.N)
                            {
                                current.grid[i, j - 1] = GameEngine.CellSelection.O;
                                madeMove = true;
                                break;
                            }
                        // down case
                        if (j <= 1)
                            if (current.grid[i, j + 1] == GameEngine.CellSelection.N)
                            {
                                current.grid[i, j + 1] = GameEngine.CellSelection.O;
                                madeMove = true;
                                break;
                            }

                        // left case
                        if (i >= 1)
                            if (current.grid[i - 1, j] == GameEngine.CellSelection.N)
                            {
                                current.grid[i - 1, j] = GameEngine.CellSelection.O;
                                madeMove = true;
                                break;
                            }
                        // right case
                        if (i <= 1)
                            if (current.grid[i + 1, j] == GameEngine.CellSelection.N)
                            {
                                current.grid[i + 1, j] = GameEngine.CellSelection.O;
                                madeMove = true;
                                break;
                            }
                    }
                }
                if (madeMove) break;
            }


            // check for winner
            if (this.checkForWinner()) return;

            // increment count
            current.turnCount++;
            Console.WriteLine("Turn: {0}", current.turnCount);

            // set user's turn
            current.isComputersTurn = false;
        }

        // check for winner
        public bool checkForWinner()
        {
            return isGameOver;
        }
    }
}
