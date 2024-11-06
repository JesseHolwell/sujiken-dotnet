using System;

namespace SujikenSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            int[][] puzzleGrid = {
                new int[] { 9 }, //9
                new int[] { 0, 0 }, //8
                new int[] { 0, 0, 0 }, //7
                new int[] { 0, 0, 0, 0 }, //6
                new int[] { 0, 0, 8, 0, 7 }, //5
                new int[] { 0, 0, 9, 0, 0, 0 }, //4
                new int[] { 5, 0, 0, 1, 6, 0, 0 }, //3
                new int[] { 4, 0, 0, 0, 0, 0, 0, 0 }, //2
                new int[] { 0, 6, 3, 0, 0, 0, 0, 0, 5 }, //1
            };

            var tryThis = puzzleGrid;

            Console.WriteLine("Thinking ...");


            if (SolveSujiken(tryThis))
            {
                Console.WriteLine("Oh my god.");
                PrintGrid(tryThis);
            }
            else
            {
                Console.WriteLine("No solution found.");
                // PrintGrid(tryThis);
            }
        }

        static bool SolveSujiken(int[][] grid)
        {
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    if (grid[row][col] == 0)  // Empty cell
                    {
                        for (int num = 1; num <= 9; num++)  // Adjust range based on puzzle rules
                        {
                            if (IsValid(grid, row, col, num))
                            {
                                grid[row][col] = num;

                                if (SolveSujiken(grid))  // Recursive call
                                    return true;

                                grid[row][col] = 0;  // Backtrack
                            }
                        }

                        // PrintGrid(grid);
                        return false;  // No valid number found, need to backtrack
                    }
                }
            }
            return true;  // Solution found
        }

        static bool IsValid(int[][] grid, int row, int col, int num)
        {
            // Check row uniqueness
            for (int i = 0; i < grid[row].Length; i++)
            {
                if (grid[row][i] == num) return false;
            }

            // Check column uniqueness
            for (int i = 0; i < grid.Length; i++)
            {
                if (col < grid[i].Length && grid[i][col] == num) return false;
            }

            // Check top-left to bottom-right diagonal (positive slope)
            int r = row, c = col;
            while (r >= 0 && c >= 0)
            {
                if (grid[r][c] == num) return false;
                r--; c--;
            }
            r = row + 1; c = col + 1;
            while (r < grid.Length && c < grid[r].Length)
            {
                if (grid[r][c] == num) return false;
                r++; c++;
            }

            // // Check top-right to bottom-left diagonal (negative slope)
            r = row; c = col;
            while (r >= 0 && c < grid[r].Length)
            {
                if (grid[r][c] == num) return false;
                r--; c++;
            }
            r = row + 1; c = col - 1;
            while (r < grid.Length && c >= 0)
            {
                if (grid[r][c] == num) return false;
                r++; c--;
            }

            // Check 3x3 box constraint specifically for triangular grid
            if (!IsUniqueIn3x3Box(grid, row, col, num))
            {
                return false;
            }

            return true;
        }
        static bool IsUniqueIn3x3Box(int[][] grid, int row, int col, int num)
        {
            // Determine the starting position of the 3x3 box
            int boxRowStart = (row / 3) * 3;
            int boxColStart = (col / 3) * 3;

            // Check all cells in the 3x3 box
            for (int r = boxRowStart; r < boxRowStart + 3; r++)
            {
                for (int c = boxColStart; c < boxColStart + 3; c++)
                {
                    if (r < grid.Length && c < grid[r].Length && grid[r][c] == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }



        static void PrintGrid(int[][] grid)
        {
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    // Determine if the cell is in a "dark" or "light" box position
                    bool isDark = ((row / 3) % 2 == (col / 3) % 2);
                    Console.BackgroundColor = isDark ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;  // Use black text for contrast

                    // Print the number with padding for alignment
                    Console.Write(grid[row][col] == 0 ? " . " : $" {grid[row][col]} ");

                    // Reset the background color after each cell
                    Console.ResetColor();
                }
                Console.WriteLine();  // Move to the next line after each row
            }
            Console.ResetColor();  // Ensure no lingering color changes
        }

    }
}
