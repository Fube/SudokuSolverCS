using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{

    class Grid
    {

        int[,] data;

        /**
         * Constructors should be self-explanatory.
         **/

        public Grid()
        {
            data = new int[9, 9];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    data[i, j] = 0;
        }

        public Grid(int[,] args)
        {
            data = new int[9, 9];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    data[i, j] = args[i, j];
        }

        public Grid(params int[] args)
        {
            data = new int[9, 9];
            for(int i = 0; i < args.Length; i++)
                data[i / 9, i % 9] = args[i];
            
        }

        /**
         * Could change this to accept a Tuple or Struct instead of 3 arguments.
         * Not sure if that's a good idea.
         **/
        bool IsPossible(int row, int col, int num)
        {

            /**
             * This gets the top right corner of the box that the value belongs to.
             * For example, row = 2, col = 6 (0 indexed) will belong to the 3rd box starting from the top left.
             * As such, boxRow will be 0 and boxCol will be 6
             **/
            int
                boxRow = (row / 3) * 3,
                boxCol = (col / 3) * 3;

            /**
             * In the loop that follows, we retrieve all of the values that are on the same axes as the given coordinate.
             * We will however be missing 4 values. The 4 corners of the box that the coordinate belongs to since they do not exist on the same axes.
             * Top left, top right, bottom left, and bottom left. In that order.
             * Pretty straightforward, right?
             **/
            HashSet<int> foo = new HashSet<int>() {  data[boxRow, boxCol], data[boxRow, boxCol + 2], data[boxRow + 2, boxCol], data[boxRow + 2, boxCol + 2] };
                        
            for (int i = 0; i < 9; i++)
            {
                foo.Add(data[row, i]);
                foo.Add(data[i, col]);
            }

            //If the value is not present in the row, column, or box it could be a valid value.
            return !foo.Contains(num);
        }

        bool IsSolved()
        {
            // If the sudoku has an empty spot, it is obviously not solved.
            foreach(int i in Enumerable.Range(0, 9))
                foreach(int j in Enumerable.Range(0, 9))
                    if(data[i, j] == 0)
                        return false;

            return true;
        }

        // Simple backtracking. Nothing spectacular.
        bool Helper(int row = 0, int col = 0)
        {
            if (IsSolved())
                return true;

            /**
             * We always need to move forward by 1 column right? But what happens when we are at the eigth column?
             * We will need to reset column to 0 and increment row by 1.
             * This does that.
             **/
            int 
                nextCol = (1 + col) % 9,
                nextRow = nextCol == 0 ? row + 1 : row;

            // Makes sure we aren't overwriting a value.
            if (data[row, col] != 0)
                return Helper(nextRow, nextCol);

            /**
             * Why 1 to 10?
             * These are all the legal values a cell in a sudoku board can hold (range(1, 9)).
             **/
            foreach(int i in Enumerable.Range(1, 9))
            {
                if (IsPossible(row, col, i))
                {
                    data[row, col] = i;

                    if (Helper(nextRow, nextCol))
                        return true;
                }
                data[row, col] = 0;
            }
            return false;
        }

        // Why not just do: Solve(int x = 0, int y = 0)?
        // So that the user can't do: Solve(1, 5)
        public bool Solve()
        {
            return Helper();
        }

        public override string ToString()
        {

            string toPrint = "";

            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 0)
                    toPrint += "\n";
                toPrint += "\t| ";
                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0 && j > 0)
                        toPrint += " | ";
                    toPrint += String.Format("{0} | ", data[i, j]);
                }
                toPrint += "\n";
            }

            return toPrint;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /**
             * There is probably a better to do this.
             * It is currently 12:48 AM. This is as good as it gets for now.
             * Btw, 0 means that it is an empty value.
             **/
            Grid sudodu = new Grid(new int[] {
                0, 0, 69,
                2, 6, 0,
                7, 0, 1,

                6, 8, 0,
                0, 7, 0,
                0, 9, 0,

                1, 9, 0,
                0, 0, 4,
                5, 0, 0,

                8, 2, 0,
                1, 0, 0,
                0, 4, 0,

                0, 0, 4,
                6, 0, 2,
                9, 0, 0,

                0, 5, 0,
                0, 0, 3,
                0, 2, 8,

                0, 0, 9,
                3, 0, 0,
                0, 7, 4,

                0, 4, 0,
                0, 5, 0,
                0, 3, 6,

                7, 0, 3,
                0, 1, 8,
                0, 0, 0 });

            Console.WriteLine(sudoku.Solve() ? sudoku.ToString() : "Was not able to solve sudoku");
            Console.ReadLine();
        }
    }
}
