using System;
using System.Linq;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            // On déclare une grille de Sudoku incomplète
            int[,] grid =
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            if (SolveSudoku(grid))
            {
                // On affiche la grille résolue
                PrintGrid(grid);
            }
            else
            {
                Console.WriteLine("Impossible de résoudre la grille.");
            }
        }

        static bool SolveSudoku(int[,] grid)
        {
            int row = 0, col = 0;
            bool isEmpty = true;

            // On vérifie s'il reste des cellules vides dans la grille
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        row = i;
                        col = j;

                        isEmpty = false;
                        break;
                    }
                }
                if (!isEmpty)
                {
                    break;
                }
            }

            // Si toutes les cellules sont remplies, on a fini de résoudre la grille
            if (isEmpty)
            {
                return true;
            }

            // On essaie toutes les valeurs possibles pour la cellule vide
            for (int num = 1; num <= 9; num++)
            {
                if (IsSafe(grid, row, col, num))
                {
                    // On assigne la valeur à la cellule vide
                    grid[row, col] = num;

                    // On réessaye de résoudre la grille avec cette valeur
                    if (SolveSudoku(grid))
                    {
                        return true;
                    }
                    else
                    {
                        // Si la valeur ne permet pas de résoudre la grille, on la "dé-assigne"
                        grid[row, col] = 0;
                    }
                }
            }

            // On retourne false si aucune valeur n'a permis de résoudre la grille
            return false;
        }

        static bool IsSafe(int[,] grid, int row, int col, int num)
        {
            // On vérifie que la valeur n'est pas déjà présente sur la ligne
            for (int c = 0; c < 9; c++)
            {
                if (grid[row, c] == num)
                {
                    return false;
                }
            }

            // On vérifie que la valeur n'est pas déjà présente sur la colonne
            for (int r = 0; r < 9; r++)
            {
                if (grid[r, col] == num)
                {
                    return false;
                }
            }

            // On vérifie que la valeur n'est pas déjà présente dans le bloc 3x3 contenant la cellule
            int startRow = row - row % 3;
            int startCol = col - col % 3;

            for (int r = startRow; r < startRow + 3; r++)
            {
                for (int c = startCol; c < startCol + 3; c++)
                {
                    if (grid[r, c] == num)
                    {
                        return false;
                    }
                }
            }

            // Si la valeur n'est présente ni sur la ligne, ni sur la colonne, ni dans le bloc 3x3, on peut l'utiliser
            return true;
        }

        static void PrintGrid(int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}