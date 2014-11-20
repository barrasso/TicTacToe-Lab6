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

        // check for first move
        public bool isComputerFirstMove;
        
        // check for game over/stalemate
        public bool isGameOver;
        public bool isStalemate;
        
        // check for PC || User win
        public bool compWins;
        public bool userWins;

        // check for comp turn
        public bool isComputersTurn;

        // counters
        public int mid0;
        public int mid1;
        public int mid2;
        public int mid3;
        public int col0;
        public int col1;
        public int col2;
        public int row0;
        public int row1;
        public int row2;
        public int diag0;
        public int diag1;

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
                if (this.checkForWinner(current))
                {
                    isGameOver = true;
                    return current;
                }

                if (!isGameOver)
                {
                    // set computer's turn
                    current.isComputersTurn = true;

                    if (current.isComputersTurn)
                        // computer make move
                        this.computerMove(current);
                }

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
            // check for first move
            if (isComputerFirstMove)
            {
                // make first move
                if (current.grid[1, 1] == GameEngine.CellSelection.N)
                    current.grid[1, 1] = GameEngine.CellSelection.O;

                isComputerFirstMove = false;
            }

            else if (!isComputerFirstMove)
            {
                // scan + map board
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        // check if O
                        if (current.grid[i, j] == GameEngine.CellSelection.O)
                        {
                            // mark mid 2,3
                            if (i == 1 && j == 1)
                            { mid2++; mid3++; } 

                            // bottom left to top right
                            if (i == 0 && j == 2) mid2++;
                            if (i == 2 && j == 0) mid2++;

                            // top left to bottom right
                            if (i == 0 && j == 0) mid3++;
                            if (i == 2 && j == 2) mid3++;

                            // mark mid 0
                            if (i == 0 && j == 1) mid0++;
                            if (i == 1 && j == 1) mid0++;
                            if (i == 2 && j == 1) mid0++;

                            // mark mid 1
                            if (i == 1 && j == 0) mid1++;
                            if (i == 1 && j == 1) mid1++;
                            if (i == 1 && j == 2) mid1++;
                        }

                        // check if X
                        if (current.grid[i, j] == GameEngine.CellSelection.X)
                        {
                            // mark diags
                            if (i == 1 && j == 1)
                            { diag0++; diag1++; } 

                            // bottom left to top right
                            if (i == 0 && j == 2) diag0++;
                            if (i == 2 && j == 0) diag0++;

                            // top left to bottom right
                            if (i == 0 && j == 0) diag1++;
                            if (i == 2 && j == 2) diag1++;

                            // mark cols
                            switch (i)
                            {
                                case 0:
                                    col0++;
                                    Console.WriteLine("I see an X at {0},{1}", i, j);
                                    break;
                                case 1:
                                    col1++;
                                    Console.WriteLine("I see an X at {0},{1}", i, j);
                                    break;
                                case 2:
                                    col2++;
                                    Console.WriteLine("I see an X at {0},{1}", i, j);
                                    break;
                            }
                            // mark rows
                            switch (j)
                            {

                                case 0:
                                    row0++;
                                    Console.WriteLine("I see an X at {0},{1}", i, j);
                                    break;
                                case 1:
                                    row1++;
                                    Console.WriteLine("I see an X at {0},{1}", i, j);
                                    break;
                                case 2:
                                    row2++;
                                    Console.WriteLine("I see an X at {0},{1}", i, j);
                                    break;
                            }
                        }
                    }
                }


                Console.WriteLine("Making move...");
                // make best possible O move
                bool madeMove = false;
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        // the best defense is the best offense
                        if (current.grid[1, 1] == GameEngine.CellSelection.O)
                        {
                            if (current.grid[0, 0] == GameEngine.CellSelection.O && current.grid[2, 2] == GameEngine.CellSelection.N) { current.grid[2, 2] = GameEngine.CellSelection.O; madeMove = true; break; }
                            if (current.grid[0, 0] == GameEngine.CellSelection.N && current.grid[2, 2] == GameEngine.CellSelection.O) { current.grid[0, 0] = GameEngine.CellSelection.O; madeMove = true; break; }

                            if (current.grid[0, 2] == GameEngine.CellSelection.O && current.grid[2, 0] == GameEngine.CellSelection.N) { current.grid[2, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                            if (current.grid[0, 2] == GameEngine.CellSelection.N && current.grid[2, 0] == GameEngine.CellSelection.O) { current.grid[0, 2] = GameEngine.CellSelection.O; madeMove = true; break; }
                        }
                        if (mid0 == 2 && current.grid[0, 1] == GameEngine.CellSelection.N) { current.grid[0, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (mid0 == 2 && current.grid[2, 1] == GameEngine.CellSelection.N) { current.grid[2, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (mid1 == 2 && current.grid[1, 0] == GameEngine.CellSelection.N) { current.grid[1, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (mid1 == 2 && current.grid[1, 2] == GameEngine.CellSelection.N) { current.grid[1, 2] = GameEngine.CellSelection.O; madeMove = true; break; }

                        // check for win
                        if (diag0 == 2 && current.grid[0, 2] == GameEngine.CellSelection.N) { current.grid[0, 2] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (diag0 == 2 && current.grid[1, 1] == GameEngine.CellSelection.N) { current.grid[1, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (diag0 == 2 && current.grid[2, 0] == GameEngine.CellSelection.N) { current.grid[2, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (diag1 == 2 && current.grid[0, 0] == GameEngine.CellSelection.N) { current.grid[0, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (diag1 == 2 && current.grid[1, 1] == GameEngine.CellSelection.N) { current.grid[1, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (diag1 == 2 && current.grid[2, 2] == GameEngine.CellSelection.N) { current.grid[2, 2] = GameEngine.CellSelection.O; madeMove = true; break; }

                        if (row0 == 2 && current.grid[0, 0] == GameEngine.CellSelection.N) { current.grid[0, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (row0 == 2 && current.grid[1, 0] == GameEngine.CellSelection.N) { current.grid[1, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (row0 == 2 && current.grid[2, 0] == GameEngine.CellSelection.N) { current.grid[2, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (row1 == 2 && current.grid[0, 1] == GameEngine.CellSelection.N) { current.grid[0, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (row1 == 2 && current.grid[1, 1] == GameEngine.CellSelection.N) { current.grid[1, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (row1 == 2 && current.grid[2, 1] == GameEngine.CellSelection.N) { current.grid[2, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (row2 == 2 && current.grid[0, 2] == GameEngine.CellSelection.N) { current.grid[0, 2] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (row2 == 2 && current.grid[1, 2] == GameEngine.CellSelection.N) { current.grid[1, 2] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (row2 == 2 && current.grid[2, 2] == GameEngine.CellSelection.N) { current.grid[2, 2] = GameEngine.CellSelection.O; madeMove = true; break; }

                        if (col0 == 2 && current.grid[0, 0] == GameEngine.CellSelection.N) { current.grid[0, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (col0 == 2 && current.grid[0, 1] == GameEngine.CellSelection.N) { current.grid[0, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (col0 == 2 && current.grid[0, 2] == GameEngine.CellSelection.N) { current.grid[0, 2] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (col1 == 2 && current.grid[1, 0] == GameEngine.CellSelection.N) { current.grid[1, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (col1 == 2 && current.grid[1, 1] == GameEngine.CellSelection.N) { current.grid[1, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (col1 == 2 && current.grid[1, 2] == GameEngine.CellSelection.N) { current.grid[1, 2] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (col2 == 2 && current.grid[2, 0] == GameEngine.CellSelection.N) { current.grid[2, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (col2 == 2 && current.grid[2, 1] == GameEngine.CellSelection.N) { current.grid[2, 1] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (col2 == 2 && current.grid[2, 2] == GameEngine.CellSelection.N) { current.grid[2, 2] = GameEngine.CellSelection.O; madeMove = true; break; }

                        // check for empty center
                        if (current.grid[1, 1] == GameEngine.CellSelection.N) { current.grid[1, 1] = GameEngine.CellSelection.O; madeMove = true; break; }

                        // check for empty corners
                        if (current.grid[0, 0] == GameEngine.CellSelection.N) { current.grid[0, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (current.grid[0, 2] == GameEngine.CellSelection.N) { current.grid[0, 2] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (current.grid[2, 0] == GameEngine.CellSelection.N) { current.grid[2, 0] = GameEngine.CellSelection.O; madeMove = true; break; }
                        if (current.grid[2, 2] == GameEngine.CellSelection.N) { current.grid[2, 2] = GameEngine.CellSelection.O; madeMove = true; break; }

                        // default move
                        if (current.grid[i, j] == GameEngine.CellSelection.N) { current.grid[i, j] = GameEngine.CellSelection.O; madeMove = true; break; }
                    }
                    if (madeMove) break;
                }
            }

               
            // check for winner
            if (this.checkForWinner(current))
            {
                // game over
                isGameOver = true;
                return;
            }

            // increment count
            current.turnCount++;
            Console.WriteLine("Turn: {0}", current.turnCount);

            // reset counters
            this.resetCounters();
            
            // set user's turn
            current.isComputersTurn = false;
        }

        public void resetCounters()
        {
            mid0 = 0;
            mid1 = 0;
            col0 = 0;
            col1 = 0;
            col2 = 0;
            row0 = 0;
            row1 = 0;
            row2 = 0;
            diag0 = 0;
            diag1 = 0;
        }

        // check for winner
        public bool checkForWinner(GameEngine current)
        {
            // check rows for X
            if (current.grid[0, 0] == GameEngine.CellSelection.X && current.grid[1, 0] == GameEngine.CellSelection.X && current.grid[2, 0] == GameEngine.CellSelection.X)
            {
                userWins = true;
                this.gameOver(userWins);
                return userWins;
            }
            if (current.grid[0, 1] == GameEngine.CellSelection.X && current.grid[1, 1] == GameEngine.CellSelection.X && current.grid[2, 1] == GameEngine.CellSelection.X)
            {
                userWins = true;
                this.gameOver(userWins);
                return userWins;
            }
            if (current.grid[0, 2] == GameEngine.CellSelection.X && current.grid[1, 2] == GameEngine.CellSelection.X && current.grid[2, 2] == GameEngine.CellSelection.X)
            {
                userWins = true;
                this.gameOver(userWins);
                return userWins;
            }
            // check rows for O
            if (current.grid[0, 0] == GameEngine.CellSelection.O && current.grid[1, 0] == GameEngine.CellSelection.O && current.grid[2, 0] == GameEngine.CellSelection.O)
            {
                compWins = true;
                this.gameOver(compWins);
                return compWins;
            }
            if (current.grid[0, 1] == GameEngine.CellSelection.O && current.grid[1, 1] == GameEngine.CellSelection.O && current.grid[2, 1] == GameEngine.CellSelection.O)
            {
                compWins = true;
                this.gameOver(compWins);
                return compWins;
            }
            if (current.grid[0, 2] == GameEngine.CellSelection.O && current.grid[1, 2] == GameEngine.CellSelection.O && current.grid[2, 2] == GameEngine.CellSelection.O)
            {
                compWins = true;
                this.gameOver(compWins);
                return compWins;
            }

            // check columns for X
            if (current.grid[0, 0] == GameEngine.CellSelection.X && current.grid[0, 1] == GameEngine.CellSelection.X && current.grid[0, 2] == GameEngine.CellSelection.X)
            {
                userWins = true;
                this.gameOver(userWins);
                return userWins;
            }
            if (current.grid[1, 0] == GameEngine.CellSelection.X && current.grid[1, 1] == GameEngine.CellSelection.X && current.grid[1, 2] == GameEngine.CellSelection.X)
            {
                userWins = true;
                this.gameOver(userWins);
                return userWins;
            }
            if (current.grid[2, 0] == GameEngine.CellSelection.X && current.grid[2, 1] == GameEngine.CellSelection.X && current.grid[2, 2] == GameEngine.CellSelection.X)
            {
                userWins = true;
                this.gameOver(userWins);
                return userWins;
            }
            // check columns for O
            if (current.grid[0, 0] == GameEngine.CellSelection.O && current.grid[0, 1] == GameEngine.CellSelection.O && current.grid[0, 2] == GameEngine.CellSelection.O)
            {
                compWins = true;
                this.gameOver(compWins);
                return compWins;
            }
            if (current.grid[1, 0] == GameEngine.CellSelection.O && current.grid[1, 1] == GameEngine.CellSelection.O && current.grid[1, 2] == GameEngine.CellSelection.O)
            {
                compWins = true;
                this.gameOver(compWins);
                return compWins;
            }
            if (current.grid[2, 0] == GameEngine.CellSelection.O && current.grid[2, 1] == GameEngine.CellSelection.O && current.grid[2, 2] == GameEngine.CellSelection.O)
            {
                compWins = true;
                this.gameOver(compWins);
                return compWins;
            }

            // check diagonals for X
            if (current.grid[0, 0] == GameEngine.CellSelection.X && current.grid[1, 1] == GameEngine.CellSelection.X && current.grid[2, 2] == GameEngine.CellSelection.X)
            {
                userWins = true;
                this.gameOver(userWins);
                return userWins;
            }
            if (current.grid[0, 2] == GameEngine.CellSelection.X && current.grid[1, 1] == GameEngine.CellSelection.X && current.grid[2, 0] == GameEngine.CellSelection.X)
            {
                userWins = true;
                this.gameOver(userWins);
                return userWins;
            }
            // check diagonals for O
            if (current.grid[0, 0] == GameEngine.CellSelection.O && current.grid[1, 1] == GameEngine.CellSelection.O && current.grid[2, 2] == GameEngine.CellSelection.O)
            {
                compWins = true;
                this.gameOver(compWins);
                return compWins;
            }
            if (current.grid[0, 2] == GameEngine.CellSelection.O && current.grid[1, 1] == GameEngine.CellSelection.O && current.grid[2, 0] == GameEngine.CellSelection.O)
            {
                compWins = true;
                this.gameOver(compWins);
                return compWins;
            }

            // stalemate check
            if(checkForStalemate(current))
            {
                this.gameOver(isStalemate);
                return isStalemate;
            }

            // else
            return false;
        }

        public bool checkForStalemate(GameEngine current)
        {
            // check for stalemate
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    // if there is an empty space
                    if (current.grid[i, j] == GameEngine.CellSelection.N)
                    {
                        // no stalemate
                        isStalemate = false;
                        return false;
                    }
                }
            }

            // would have returned by now if a space was empty
            // if no spaces are empty
            isStalemate = true;
            return isStalemate;
        }

        public void gameOver(bool winner)
        {
            if (userWins)
                MessageBox.Show("You win!");
            else if (compWins)
                MessageBox.Show("You lose!");
            else if (isStalemate)
                MessageBox.Show("Draw!");
        }
    }
}
