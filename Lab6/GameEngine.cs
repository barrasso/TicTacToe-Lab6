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
                // randomize starting position to make Ai less predictable
                Random rando = new Random();
                int startSpot = rando.Next(4);

                switch (startSpot)
                {
                    case 0:
                        // make first move
                        if (current.grid[0, 0] == GameEngine.CellSelection.N)
                            current.grid[0, 0] = GameEngine.CellSelection.O;
                        break;
                    case 1:
                        // make first move
                        if (current.grid[2, 0] == GameEngine.CellSelection.N)
                            current.grid[2, 0] = GameEngine.CellSelection.O;
                        break;
                    case 2:
                        // make first move
                        if (current.grid[0, 2] == GameEngine.CellSelection.N)
                            current.grid[0, 2] = GameEngine.CellSelection.O;
                        break;
                    case 3:
                        // make first move
                        if (current.grid[2, 2] == GameEngine.CellSelection.N)
                            current.grid[2, 2] = GameEngine.CellSelection.O;
                        break;
                    default:
                        // make first move
                        if (current.grid[0, 0] == GameEngine.CellSelection.N)
                            current.grid[0, 0] = GameEngine.CellSelection.O;
                        break;
                }
                isComputerFirstMove = false;
            }

            else if (!isComputerFirstMove)
            {
                // make O move
                bool madeMove = false;
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {

                        // check against X
                        if (current.grid[i, j] == GameEngine.CellSelection.X)
                        {
                            // go for winning move cases


                            // go for defending move cases


                            // check standard move cases
                            // up case
                            if (j >= 1)
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

            // set user's turn
            current.isComputersTurn = false;
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
