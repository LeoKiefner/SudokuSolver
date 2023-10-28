using System;
using System.Reflection.Metadata.Ecma335;

class sudoku
{
    static void Main()
    {
        //Appel a la fonction de la grille de Sudoku
        int[,] grille = DeclaGrille();

        //Appel a la fonction de modification/initialisation de la grille
        grille = InitGrille(grille);

        //Affiche la grille
        for (int lig1 = 0; lig1 < grille.GetLength(0); lig1++)
        {
            for (int col1 = 0; col1 < grille.GetLength(1); col1++)
            {
                Console.Write("     " + grille[lig1, col1]);
            }
            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }

        //Appel a la fonction de verification de la grille
        VerifGrille(grille);

        //Appel a la fonction brutforce
        grille = Brutforce(grille);

        //Appel a la fonction OptimiseBrutforce
        grille = OptimiseBrutforce(grille);
    }

    // Fonction qui declare la grille de Sudoku par l'utilisateur (avcec ses propres dimensions)
    public static int[,] DeclaGrille()
    {
        int[,] erreur = new int[0, 0];

        //Initialisation et declaration des dimensiosn de la grille
        Console.WriteLine("Quelles sont les dimensions de votre Sudoku (4, 9, 16) ?");
        int dim = int.Parse(Console.ReadLine());

        //Initialisation de la grille
        if (dim == 4)
        {
            int[,] sudo = new int[4, 4];
            Console.WriteLine("Votre grille de Sudoku a bien ete creee avec une dimension de " + dim + "x" + dim + " !");
            return sudo;
        }

        if (dim == 9)
        {
            int[,] sudo = new int[9, 9];
            Console.WriteLine("Votre grille de Sudoku a bien ete creee avec une dimension de " + dim + "x" + dim + " !");
            return sudo;
        }

        if (dim == 16)
        {
            int[,] sudo = new int[16, 16];
            Console.WriteLine("Votre grille de Sudoku a bien ete creee avec une dimension de " + dim + "x" + dim + " !");
            return sudo;
        }
        else
        {
            Console.WriteLine("ERREUR : Dimensions non valide");
            return erreur;
        }
    }

    //Fonction qui initialise la grille de Sudoku par l'utilisateur
    public static int[,] InitGrille(int[,] sudoku)
    {
        Console.WriteLine("Entrez les valeurs de votre grille");

        for (int lig = 0; lig < sudoku.GetLength(0); lig++)
        {
            for (int col = 0; col < sudoku.GetLength(1); col++)
            {
                Console.Clear();
                Console.WriteLine("Remplacez la valeur x par votre chiffre de votre grille");
                Console.WriteLine("");
                for (int lig1 = 0; lig1 < sudoku.GetLength(0); lig1++)
                {
                    for (int col1 = 0; col1 < sudoku.GetLength(1); col1++)
                    {
                        if (lig1 == lig && col1 == col)
                        {
                            Console.Write("     " + "x");
                        }
                        else
                        {
                            Console.Write("     " + sudoku[lig1, col1]);
                        }
                    }
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                }

                int n = int.Parse(Console.ReadLine());
                if (n > 0 && n <= sudoku.GetLength(0))
                {
                    sudoku[lig, col] = n;
                }
                else
                {
                    Console.WriteLine("ERREUR : Valeur non valide");
                    lig--;
                }
            }
        }

        return sudoku;
    }

    //Fonction de verification de la grille
    public static void VerifGrille(int[,] sudoku)
    {
        //Verification des lignes
        for (int lig = 0; lig < sudoku.GetLength(0); lig++)
        {
            for (int col = 0; col < sudoku.GetLength(1) - 1; col++)
            {
                for (int col2 = col + 1; col2 < sudoku.GetLength(1); col2++)
                {
                    if (sudoku[lig, col] == sudoku[lig, col2] && sudoku[lig, col] != 0)
                    {
                        Console.WriteLine("ERREUR : Ligne " + (lig + 1) + " : Valeurs identiques");
                    }
                }
            }
        }

        //Verification des colonnes
        for (int col = 0; col < sudoku.GetLength(1); col++)
        {
            for (int lig = 0; lig < sudoku.GetLength(0) - 1; lig++)
            {
                for (int lig2 = lig + 1; lig2 < sudoku.GetLength(0); lig2++)
                {
                    if (sudoku[lig, col] == sudoku[lig2, col] && sudoku[lig, col] != 0)
                    {
                        Console.WriteLine("ERREUR : Colonne " + (col + 1) + " : Valeurs identiques");
                    }
                }
            }
        }

        //Verification des carres
        int sqrt = (int)Math.Sqrt(sudoku.GetLength(0));
        for (int lig = 0; lig < sqrt; lig++)
        {
            for (int col = 0; col < sqrt; col++)
            {
                for (int lig2 = lig * sqrt; lig2 < (lig + 1) * sqrt; lig2++)
                {
                    for (int col2 = col * sqrt; col2 < (col + 1) * sqrt; col2++)
                    {
                        for (int lig3 = lig * sqrt; lig3 < (lig + 1) * sqrt; lig3++)
                        {
                            for (int col3 = col * sqrt; col3 < (col + 1) * sqrt; col3++)
                            {
                                if (sudoku[lig2, col2] == sudoku[lig3, col3] && sudoku[lig2, col2] != 0 && !(lig2 == lig3 && col2 == col3))
                                {
                                    Console.WriteLine("ERREUR : Carre [" + (lig + 1) + "," + (col + 1) + "] : Valeurs identiques");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //Fonction de resolution par bruteforce
    public static int[,] Brutforce(int[,] sudoku)
    {
        bool complet = false;
        int[,] sudoku_tmp = sudoku;
        int lig = 0;
        int col = 0;

        while (!complet)
        {
            if (sudoku_tmp[lig, col] == sudoku.GetLength(0))
            {
                sudoku_tmp[lig, col] = 0;

                if (col > 0)
                {
                    col--;
                }
                else if (lig > 0)
                {
                    lig--;
                    col = sudoku.GetLength(1) - 1;
                }
                else
                {
                    complet = true;
                }
            }
            else
            {
                sudoku_tmp[lig, col]++;

                if (VerifieCase(sudoku_tmp, lig, col))
                {
                    if (col < sudoku.GetLength(1) - 1)
                    {
                        col++;
                    }
                    else if (lig < sudoku.GetLength(0) - 1)
                    {
                        lig++;
                        col = 0;
                    }
                    else
                    {
                        complet = true;
                    }
                }
            }
        }

        return sudoku_tmp;
    }

    //Fonction pour verifier si un chiffre peut etre place a un endroit donne
    public static bool VerifieCase(int[,] sudoku, int lig, int col)
    {
        bool verifie = true;
        int[] verifieLig = new int[sudoku.GetLength(0)];
        int[] verifieCol = new int[sudoku.GetLength(1)];
        int[] verifieCarre = new int[sudoku.GetLength(0)];

        //Compte le nombre de chiffres de chaque type pour la ligne, colonne et carre
        for (int i = 0; i < sudoku.GetLength(0) && verifie == true; i++)
        {
            //Colonne
            if (!(sudoku[i, col] == 0))
            {
                verifieCol[sudoku[i, col] - 1]++;
                if (verifieCol[sudoku[i, col] - 1] > 1)
                {
                    verifie = false;
                }
            }
            //Ligne
            if (!(sudoku[lig, i] == 0))
            {
                verifieLig[sudoku[lig, i] - 1]++;
                if (verifieLig[sudoku[lig, i] - 1] > 1)
                {
                    verifie = false;
                }
            }

            //Carre
            int sqrt = (int)Math.Sqrt(sudoku.GetLength(0));
            int ligCarre = lig / sqrt;
            int colCarre = col / sqrt;
            if (!(sudoku[(ligCarre * sqrt) + (i / sqrt), (colCarre * sqrt) + (i % sqrt)] == 0))
            {
                verifieCarre[sudoku[(ligCarre * sqrt) + (i / sqrt), (colCarre * sqrt) + (i % sqrt)] - 1]++;
                if (verifieCarre[sudoku[(ligCarre * sqrt) + (i / sqrt), (colCarre * sqrt) + (i % sqrt)] - 1] > 1)
                {
                    verifie = false;
                }
            }
        }

        return verifie;
    }

    //Fonction pour optimiser la resolution par bruteforce en utilisant la technique des notes
    public static int[,] OptimiseBrutforce(int[,] sudoku)
    {
        bool complet = false;
        int[,] sudoku_tmp = sudoku;
        int lig = 0;
        int col = 0;
        int[,][] notes = new int[sudoku.GetLength(0), sudoku.GetLength(1)][];

        while (!complet)
        {
            if (sudoku_tmp[lig, col] == sudoku.GetLength(0))
            {
                sudoku_tmp[lig, col] = 0;
                notes[lig, col] = new int[0];

                if (col > 0)
                {
                    col--;
                }
                else if (lig > 0)
                {
                    lig--;
                    col = sudoku.GetLength(1) - 1;
                }
                else
                {
                    complet = true;
                }
            }
            else
            {
                if (notes[lig, col] == null || notes[lig, col].Length == 0)
                {
                    notes[lig, col] = new int[sudoku.GetLength(0)];

                    for (int i = 1; i <= sudoku.GetLength(0); i++)
                    {
                        sudoku_tmp[lig, col] = i;

                        if (VerifieCase(sudoku_tmp, lig, col))
                        {
                            notes[lig, col][i - 1] = i;
                        }
                    }

                    sudoku_tmp[lig, col] = 0;
                }

                sudoku_tmp[lig, col] = notes[lig, col][0];
                notes[lig, col] = new int[notes[lig, col].Length - 1];

                if (VerifieCase(sudoku_tmp, lig, col))
                {
                    if (col < sudoku.GetLength(1) - 1)
                    {
                        col++;
                    }
                    else if (lig < sudoku.GetLength(0) - 1)
                    {
                        lig++;
                        col = 0;
                    }
                    else
                    {
                        complet = true;
                    }
                }
            }
        }

        return sudoku_tmp;
    }
}