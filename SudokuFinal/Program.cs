using System;
using System.Collections.Generic;
using System.IO;

class sudoku
{
    static void Main()
    {

        //Creation de la grille de Sudoku vide
        int[,] sudoku = new int[0, 0];

        //Appel a la fonction du Menu Principal
        MenuPrincipal(sudoku);
    }

    // MenuPrincipal : Fonction qui affiche un menu a l'utilisateur lui permettant de resoudre ou quitter
    // Parametres :
    // Aucun
    public static void MenuPrincipal(int[,] sudoku)
    {
        //Clear pour rendre la console plus belle
        Console.Clear();

        //Proposition des choix
        Console.WriteLine("Que voulez vous faire ?");
        Console.WriteLine(" ");
        Console.WriteLine("1. Resoudre ma grille ");
        Console.WriteLine(" ");
        Console.WriteLine("2. Quitter");

        //Logo
        Console.WriteLine(" ");
        Console.WriteLine("");
        Console.WriteLine("   ___    _   _    ___     ___     ___     ___     ___     ___   ");
        Console.WriteLine("  / __|  | | | |  |   \\   / _ \\   / __|   / _ \\   |   \\   | __|  ");
        Console.WriteLine("  \\__ \\  | |_| |  | |) | | (_) | | (__   | (_) |  | |) |  | _|   ");
        Console.WriteLine("  |___/   \\___/   |___/   \\___/   \\___|   \\___/   |___/   |___|  ");
        Console.WriteLine("");
        Console.WriteLine("");

        //Prise en compte des choix
        int choix = int.Parse(Console.ReadLine());

        if (choix == 1)
        {
            //Choix entre initialisation par fichier ou manuelle
            Console.Clear();
            Console.WriteLine("Comment voulez vous initialiser votre grille ?");
            Console.WriteLine("");
            Console.WriteLine("1. A partir d'un fichier");
            Console.WriteLine("");
            Console.WriteLine("2. Manuellement case par case");

            //Logo
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("   ___    _   _    ___     ___     ___     ___     ___     ___   ");
            Console.WriteLine("  / __|  | | | |  |   \\   / _ \\   / __|   / _ \\   |   \\   | __|  ");
            Console.WriteLine("  \\__ \\  | |_| |  | |) | | (_) | | (__   | (_) |  | |) |  | _|   ");
            Console.WriteLine("  |___/   \\___/   |___/   \\___/   \\___|   \\___/   |___/   |___|  ");
            Console.WriteLine("");
            Console.WriteLine("");


            int choix1 = int.Parse(Console.ReadLine());

            if (choix1 == 1) { sudoku = GenererGrille(); }

            if (choix1 == 2)
            {
                //Appel a la fonction de la grille de Sudoku
                sudoku = DeclaGrille();

                //Appel a la fonction de modification/initialisation de la grille
                sudoku = InitGrille(sudoku);
            }

            //On verifie que le dernier Sudoku ne soit pas le meme que celui actuellement (que la resolution ai evoluee)
            int[,] derniersudoku = new int[sudoku.GetLength(0), sudoku.GetLength(1)];

            for (int lig = 0; lig < sudoku.GetLength(0); lig++)
            {
                for (int col = 0; col < sudoku.GetLength(1); col++)
                {
                    derniersudoku[lig, col] = sudoku[lig, col];
                }
            }
            List<int>[,] Liste = TechniqueNotes(sudoku);
            bool changement = true;
            int[,] temp_sudo = new int[sudoku.GetLength(0), sudoku.GetLength(1)];
            List<int>[,] temp_ret = new List<int>[sudoku.GetLength(0), sudoku.GetLength(1)];

            //Tant qu'il y'a un changement, le programme continue d'essayer de resoudre la grille
            while (changement)
            {
                for (int i = 0; i < sudoku.GetLength(0); i++)
                {
                    for (int j = 0; j < sudoku.GetLength(1); j++)
                    {
                        temp_sudo[i, j] = sudoku[i, j];
                        temp_ret[i, j] = Liste[i, j];
                    }
                }
                //Appel a la fonction d'actualisation
                Actualisation(sudoku, Liste);


                //Appel aux fonctions de resolution intelligentes avec une actualisation entre chaque etape
                VisibleSeul(sudoku, Liste);
                Actualisation(sudoku, Liste);
                VisiblePairs(sudoku, Liste);
                Actualisation(sudoku, Liste);
                VisibleTriples(sudoku, Liste);
                Actualisation(sudoku, Liste);
                CacheeSeul(sudoku, Liste);
                Actualisation(sudoku, Liste);

                //Par defaut on mets qu'il n'y a plus de changement
                changement = false;
                for (int i = 0; i < sudoku.GetLength(0); i++)
                {
                    for (int j = 0; j < sudoku.GetLength(1); j++)
                    {
                        if (sudoku[i, j] != temp_sudo[i, j] || Liste[i, j] != temp_ret[i, j])
                        {
                            changement = true;
                        }
                    }
                }
            }

            //Appel a la fonction d'affichage
            AffichageSudoku(sudoku);

        }

        else
        {
        }
    }

    // DeclaGrille : Fonction qui declare et initialise une grille de Sudoku a partir d'un fichier (RESPONSIVE)
    // Aucun
    public static int[,] GenererGrille()
    {
        //Affichage a l'utilisateur
        Console.WriteLine("Entrez le nom de votre fichier (ex : simple.txt) : ");
        //Chemin et nom du Fichier
        string[] fichier_entier = File.ReadAllLines(Directory.GetCurrentDirectory() + "/" + Console.ReadLine());
        //Taille du Sudoku
        int taille_sudo = (int)Math.Sqrt(fichier_entier[0].Split(' ').Length);
        int[,] tab = new int[taille_sudo, taille_sudo];
        int position = 0;
        for (int i = 0; i < fichier_entier.Length; i++)
        {
            string[] ligne = fichier_entier[i].Split(' ');
            for (int j = 0; j < taille_sudo; j++)
            {
                for (int k = 0; k < taille_sudo; k++)
                {
                    tab[j, k] = int.Parse(ligne[position]);
                    position++;
                }
            }
        }
        return tab;
    }

    // DeclaGrille : Fonction qui declare le tableau 2D de Sudoku avec les valeurs de l'utilisateur
    // Parametres :
    // Aucun
    public static int[,] DeclaGrille()
    {
        Console.Clear();
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

    // InitGrille : Fonction qui initialise les valeurs dans la grille de Sudoku
    // Parametres :
    // sudoku : int[,] : La Grille De Sudoku
    public static int[,] InitGrille(int[,] sudoku)
    {
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
                        if (col1 == 0)
                        {
                            Console.Write("|");
                        }


                        if (lig1 == lig && col1 == col)
                        {
                            Console.Write("     " + "x");
                        }
                        else
                        {
                            Console.Write("     " + sudoku[lig1, col1]);
                        }
                        if (col1 == sudoku.GetLength(1) - 1)
                        {
                            Console.Write("     " + "|");
                        }
                    }

                    Console.WriteLine(" ");

                    Console.WriteLine(" ");

                }


                sudoku[lig, col] = int.Parse(Console.ReadLine());
            }
        }

        return sudoku;
    }

    // VerifGrille : Fonction qui verifie la grille (RESPONSIVE)
    // Parametres :
    // sudoku : int[,] : La Grille De Sudoku
    public static bool VerifGrille(int[,] sudoku)
    {
        bool Verif = true;
        int i = 0;

        //Creation d'une liste pour verificaiton
        List<int> Nombres = new List<int>();

        while (Verif == true && i < sudoku.GetLength(0))
        {
            //Colonnes

            for (int j = 1; j < sudoku.GetLength(0) + 1; j++)
            {
                Nombres.Add(j);
            }


            for (int col = 0; col < sudoku.GetLength(0); col++)
            {
                if (sudoku[col, i] > sudoku.GetLength(0))
                { Verif = false; }

                else
                {
                    Nombres.Remove(sudoku[col, i]);
                }

            }

            if (Nombres.Count > 0)
            {
                Verif = false;
            }
            Nombres.Clear();

            //Lignes

            for (int j = 1; j < sudoku.GetLength(0) + 1; j++)
            {
                Nombres.Add(j);
            }


            for (int lig = 0; lig < sudoku.GetLength(1); lig++)
            {
                if (sudoku[i, lig] > sudoku.GetLength(0))
                { Verif = false; }

                else
                {
                    Nombres.Remove(sudoku[i, lig]);
                }

            }
            if (Nombres.Count > 0)
            {
                Verif = false;
            }
            Nombres.Clear();

            i++;
        }
        return Verif;
    }

    // VisibleSeul : Fonction qui trouve les nombres individuels visibles dans la grille
    // Parametres :
    // sudoku : int[,] : La Grille De Sudoku
    // Liste : List<int>[,] : Liste des Retenues de la grille de Sudoku
    public static void VisibleSeul(int[,] sudoku, List<int>[,] Liste)
    {

        for (int lig = 0; lig < sudoku.GetLength(0); lig++)
        {
            for (int col = 0; col < sudoku.GetLength(1); col++)
            {
                if (sudoku[lig, col] == 0 && Liste[lig, col].Count == 1)
                {
                    sudoku[lig, col] = Liste[lig, col][0];
                    Liste[lig, col].Clear();
                }
            }
        }
    }

    // VisiblePairs : Fonction qui trouve les paires visibles dans la grille
    // Parametres :
    // sudoku : int[,] : La Grille De Sudoku
    // Liste : List<int>[,] : Liste des Retenues de la grille de Sudoku
    public static void VisiblePairs(int[,] sudoku, List<int>[,] Liste)
    {
        for (int lig = 0; lig < sudoku.GetLength(0); lig++)
        {
            for (int col = 0; col < sudoku.GetLength(1); col++)
            {
                if (sudoku[lig, col] == 0 && Liste[lig, col].Count == 2)
                {
                    //Creation de sauvegardes des listes pour comparer
                    int sauvegarde = Liste[lig, col][0];
                    int sauvegarde1 = Liste[lig, col][1];

                    //Verifications Lignes pour comparer
                    for (int lig1 = 0; lig1 < sudoku.GetLength(0); lig1++)
                    {
                        if (Liste[lig1, col].Count == 2 && Liste[lig1, col][0] == sauvegarde && Liste[lig1, col][1] == sauvegarde1)
                        {
                            for (int lig2 = 0; lig2 < sudoku.GetLength(0); lig2++)
                            {
                                if (lig2 == lig1 || lig2 == lig) { }
                                else
                                {
                                    for (int i = 0; i < Liste[lig2, col].Count; i++)
                                    {
                                        if (Liste[lig2, col][i] == sauvegarde || Liste[lig2, col][i] == sauvegarde1)
                                        {
                                            Liste[lig2, col].Remove(Liste[lig2, col][i]);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Verifications Colonnes pour comparer
                    for (int col1 = 0; col1 < sudoku.GetLength(1); col1++)
                    {
                        if (Liste[lig, col1].Count == 2 && Liste[lig, col1][0] == sauvegarde && Liste[lig, col1][1] == sauvegarde1)
                        {
                            for (int col2 = 0; col2 < sudoku.GetLength(0); col2++)
                            {
                                if (col2 == col1 || col2 == col) { }
                                else
                                {
                                    for (int i = 0; i < Liste[lig, col2].Count; i++)
                                    {
                                        if (Liste[lig, col2][i] == sauvegarde || Liste[lig, col2][i] == sauvegarde1)
                                        {
                                            Liste[lig, col2].Remove(Liste[lig, col2][i]);

                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Verification des Carres pour comparer
                    int carre = (int)Math.Sqrt(sudoku.GetLength(0)) * (lig / (int)Math.Sqrt(sudoku.GetLength(0)));
                    int carrecol = (int)Math.Sqrt(sudoku.GetLength(1)) * (col / (int)Math.Sqrt(sudoku.GetLength(1)));
                    //Verification Carre (RESPONSIVE)
                    for (int veriflig = carre; veriflig < Math.Sqrt(sudoku.GetLength(0)) + carre; veriflig++)
                    {
                        for (int verifcol = carrecol; verifcol < Math.Sqrt(sudoku.GetLength(1)) + carrecol; verifcol++)
                        {
                            if (Liste[veriflig, verifcol].Count == 2 && Liste[veriflig, verifcol][0] == sauvegarde && Liste[veriflig, verifcol][1] == sauvegarde1)
                            {
                                for (int lig2 = carre; lig2 < Math.Sqrt(sudoku.GetLength(0)) + carre; lig2++)
                                {
                                    for (int col2 = carrecol; col2 < Math.Sqrt(sudoku.GetLength(1)) + carrecol; col2++)
                                    {
                                        if (col2 == verifcol && lig2 == veriflig || col2 == col && lig2 == lig) { }
                                        else
                                        {
                                            for (int i = 0; i < Liste[lig2, col2].Count; i++)
                                            {
                                                if (Liste[lig2, col2][i] == sauvegarde || Liste[lig2, col2][i] == sauvegarde1)
                                                {
                                                    Liste[lig2, col2].Remove(Liste[lig2, col2][i]);

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
    }

    // VisibleTriples : Fonction qui trouve les triplets visibles dans la grille
    // Parametres :
    // sudoku : int[,] : La Grille De Sudoku
    // Liste : List<int>[,] : Liste des Retenues de la grille de Sudoku
    public static void VisibleTriples(int[,] sudoku, List<int>[,] Liste)
    {
        for (int i = 0; i < sudoku.GetLength(0); i++)
        {
            for (int j = 0; j < sudoku.GetLength(1); j++)
            {
                if (Liste[i, j].Count == 2)
                {
                    //Verification des lignes
                    for (int k = 0; k < sudoku.GetLength(0); k++)
                    {
                        if (Liste[i, k].Count == 2 && k != j)
                        {
                            for (int u = 0; u < sudoku.GetLength(0); u++)
                            {
                                if (Liste[i, u].Count == 2 && u != j && u != k)
                                {
                                    if ((Liste[i, u][0] == Liste[i, k][0] || Liste[i, u][0] == Liste[i, k][1] || Liste[i, u][0] == Liste[i, j][0] || Liste[i, u][0] == Liste[i, j][1])
                                    && (Liste[i, u][1] == Liste[i, k][0] || Liste[i, u][1] == Liste[i, k][1] || Liste[i, u][1] == Liste[i, j][0] || Liste[i, u][1] == Liste[i, j][1])
                                    && (Liste[i, k][0] == Liste[i, u][0] || Liste[i, k][0] == Liste[i, u][1] || Liste[i, k][0] == Liste[i, j][0] || Liste[i, k][0] == Liste[i, j][1])
                                    && (Liste[i, k][1] == Liste[i, u][0] || Liste[i, k][1] == Liste[i, u][1] || Liste[i, k][1] == Liste[i, j][0] || Liste[i, k][1] == Liste[i, j][1])
                                    && (Liste[i, j][0] == Liste[i, u][0] || Liste[i, j][0] == Liste[i, u][1] || Liste[i, j][0] == Liste[i, k][0] || Liste[i, j][0] == Liste[i, k][1])
                                    && (Liste[i, j][1] == Liste[i, u][0] || Liste[i, j][1] == Liste[i, u][1] || Liste[i, j][1] == Liste[i, k][0] || Liste[i, j][1] == Liste[i, k][1]))
                                    {
                                        for (int b = 0; b < sudoku.GetLength(0); b++)
                                        {
                                            for (int s = 0; s < Liste[i, b].Count; s++)
                                            {
                                                int val = Liste[i, b][s];

                                                if ((val == Liste[i, k][0] || val == Liste[i, k][1] || val == Liste[i, j][0] || val == Liste[i, j][1] || val == Liste[i, u][0] || val == Liste[i, u][1]) && b != k && b != j && b != u)
                                                {
                                                    Liste[i, b].Remove(val);
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                    //Verification des colonnes
                    for (int l = 0; l < sudoku.GetLength(1); l++)
                    {
                        if (Liste[l, j].Count == 2 && l != i)
                        {
                            for (int d = 0; d < sudoku.GetLength(1); d++)
                            {
                                if (Liste[d, j].Count == 2 && d != i && d != l)
                                {
                                    if ((Liste[d, j][0] == Liste[l, j][0] || Liste[d, j][0] == Liste[l, j][1] || Liste[d, j][0] == Liste[i, j][0] || Liste[d, j][0] == Liste[i, j][1])
                                    && (Liste[d, j][1] == Liste[l, j][0] || Liste[d, j][1] == Liste[l, j][1] || Liste[d, j][1] == Liste[i, j][0] || Liste[d, j][1] == Liste[i, j][1])
                                    && (Liste[l, j][0] == Liste[d, j][0] || Liste[l, j][0] == Liste[d, j][1] || Liste[l, j][0] == Liste[i, j][0] || Liste[l, j][0] == Liste[i, j][1])
                                    && (Liste[l, j][1] == Liste[d, j][0] || Liste[l, j][1] == Liste[d, j][1] || Liste[l, j][1] == Liste[i, j][0] || Liste[l, j][1] == Liste[i, j][1])
                                    && (Liste[i, j][0] == Liste[d, j][0] || Liste[i, j][0] == Liste[d, j][1] || Liste[i, j][0] == Liste[l, j][0] || Liste[i, j][0] == Liste[l, j][1])
                                    && (Liste[i, j][1] == Liste[d, j][0] || Liste[i, j][1] == Liste[d, j][1] || Liste[i, j][1] == Liste[l, j][0] || Liste[i, j][1] == Liste[l, j][1]))
                                    {

                                        for (int q = 0; q < sudoku.GetLength(1); q++)
                                        {
                                            for (int s1 = 0; s1 < Liste[q, j].Count; s1++)
                                            {
                                                int val2 = Liste[q, j][s1];

                                                if ((val2 == Liste[l, j][0] || val2 == Liste[l, j][1] || val2 == Liste[i, j][0] || val2 == Liste[i, j][1] || val2 == Liste[d, j][0] || val2 == Liste[d, j][1]) && q != l && q != i && q != d)
                                                {
                                                    Liste[q, j].Remove(val2);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                        }
                    }

                    //Verification des carres
                    int Carrelig = (int)Math.Sqrt(sudoku.GetLength(0)) * (i / (int)Math.Sqrt(sudoku.GetLength(0)));
                    int Carrelcol = (int)Math.Sqrt(sudoku.GetLength(1)) * (j / (int)Math.Sqrt(sudoku.GetLength(1)));
                    for (int m = Carrelig; m < Carrelig + (int)Math.Sqrt(sudoku.GetLength(0)); m++)
                    {
                        for (int n = Carrelcol; n < Carrelcol + (int)Math.Sqrt(sudoku.GetLength(1)); n++)
                        {
                            if (Liste[m, n].Count == 2 && (m != i || n != j))
                            {
                                for (int m2 = Carrelig; m2 < Carrelig + (int)Math.Sqrt(sudoku.GetLength(0)); m2++)
                                {
                                    for (int n2 = Carrelcol; n2 < Carrelcol + (int)Math.Sqrt(sudoku.GetLength(1)); n2++)
                                    {
                                        if (Liste[m2, n2].Count == 2 && (m2 != m || n2 != n) && (m2 != i || n2 != j))
                                        {
                                            if ((Liste[m, n][0] == Liste[i, j][0] || Liste[m, n][0] == Liste[i, j][1] || Liste[m, n][0] == Liste[m2, n2][0] || Liste[m, n][0] == Liste[m2, n2][1])
                                            && (Liste[m, n][1] == Liste[i, j][0] || Liste[m, n][1] == Liste[i, j][1] || Liste[m, n][1] == Liste[m2, n2][0] || Liste[m, n][1] == Liste[m2, n2][1])
                                            && (Liste[m2, n2][0] == Liste[i, j][0] || Liste[m2, n2][0] == Liste[i, j][1] || Liste[m2, n2][0] == Liste[m, n][0] || Liste[m2, n2][0] == Liste[m, n][1])
                                            && (Liste[m2, n2][1] == Liste[i, j][0] || Liste[m2, n2][1] == Liste[i, j][1] || Liste[m2, n2][1] == Liste[m, n][0] || Liste[m2, n2][1] == Liste[m, n][1])
                                            && (Liste[i, j][0] == Liste[m, n][0] || Liste[i, j][0] == Liste[m, n][1] || Liste[i, j][0] == Liste[m2, n2][0] || Liste[i, j][0] == Liste[m2, n2][1])
                                            && (Liste[i, j][1] == Liste[m, n][0] || Liste[i, j][1] == Liste[m, n][1] || Liste[i, j][1] == Liste[m2, n2][0] || Liste[i, j][1] == Liste[m2, n2][1]))
                                            {
                                                for (int o = Carrelig; o < Carrelig + (int)Math.Sqrt(sudoku.GetLength(0)); o++)
                                                {
                                                    for (int p = Carrelcol; p < Carrelcol + (int)Math.Sqrt(sudoku.GetLength(1)); p++)
                                                    {
                                                        for (int f = 0; f < Liste[o, p].Count; f++)
                                                        {
                                                            int val3 = Liste[o, p][f];

                                                            if ((val3 == Liste[m, n][0] || val3 == Liste[m, n][1] || val3 == Liste[m2, n2][0] || val3 == Liste[m2, n2][1] || val3 == Liste[i, j][0] || val3 == Liste[i, j][1]) && (o != i || p != j) && (o != m || p != n) && (o != m2 || p != n2))
                                                            {
                                                                Liste[o, p].Remove(val3);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // TechniqueNotes : Fonction qui creer la liste de Retenues
    // Parametres :
    // sudoku : int[,] : La Grille De Sudoku
    public static List<int>[,] TechniqueNotes(int[,] sudoku)
    {
        //Initialisation de la liste a partir des autres listes
        List<int>[,] Liste = new List<int>[sudoku.GetLength(0), sudoku.GetLength(1)];

        //Pour chaque case
        for (int lig = 0; lig < sudoku.GetLength(0); lig++)
        {
            for (int col = 0; col < sudoku.GetLength(1); col++)
            {
                //Si la case actuelle est vide
                if (sudoku[lig, col] == 0)
                {
                    //Initialisation de la liste dans UNE case
                    List<int> Nombres = new List<int>();

                    //Ajout des nombres dans la liste pour chaque case en fonction des dimensions du sudoku
                    for (int i = 1; i < sudoku.GetLength(0) + 1; i++)
                    {
                        Nombres.Add(i);
                    }


                    //Si un nombre est deja present dans la meme ligne alors il est supprime de la liste
                    for (int veriflig = 0; veriflig < sudoku.GetLength(0); veriflig++)
                    {
                        if (sudoku[veriflig, col] != 0)
                        {
                            Nombres.Remove(sudoku[veriflig, col]);
                        }
                    }

                    //Si un nombre est deja present dans la meme colonne alors il est supprime de la liste
                    for (int verifcol = 0; verifcol < sudoku.GetLength(1); verifcol++)
                    {
                        if (sudoku[lig, verifcol] != 0)
                        {
                            Nombres.Remove(sudoku[lig, verifcol]);
                        }
                    }

                    //Initialisation et declaration des dimensions des carres (RESPONSIVE)
                    int carre = (int)Math.Sqrt(sudoku.GetLength(0)) * (lig / (int)Math.Sqrt(sudoku.GetLength(0)));
                    int carrecol = (int)Math.Sqrt(sudoku.GetLength(1)) * (col / (int)Math.Sqrt(sudoku.GetLength(1)));
                    //Verification Carre (RESPONSIVE)
                    for (int veriflig = carre; veriflig < Math.Sqrt(sudoku.GetLength(0)) + carre; veriflig++)
                    {
                        for (int verifcol = carrecol; verifcol < Math.Sqrt(sudoku.GetLength(1)) + carrecol; verifcol++)
                        {
                            if (sudoku[veriflig, verifcol] != 0)
                            {
                                Nombres.Remove(sudoku[veriflig, verifcol]);
                            }
                        }
                    }

                    //Stockage de la liste dans le tableau   
                    Liste[lig, col] = Nombres;
                }

                else
                {
                    Liste[lig, col] = new List<int>();
                }
            }
        }
        return Liste;
    }


    // CacheeSeul : Fonction qui trouve les nombres cachees individuels d'une grille 
    // Parametres :
    // sudoku : int[,] : La Grille De Sudoku
    // Liste : List<int>[,] : Liste des Retenues de la grille de Sudoku
    public static void CacheeSeul(int[,] sudoku, List<int>[,] Liste)
    {
        for (int lig = 0; lig < sudoku.GetLength(0); lig++)
        {
            for (int col = 0; col < sudoku.GetLength(1); col++)
            {
                if (sudoku[lig, col] == 0)
                {
                    List<int> valeurpossible = Liste[lig, col];
                    if (valeurpossible.Count == 1)
                    {
                        int CacheeSeul = valeurpossible[0];
                        sudoku[lig, col] = CacheeSeul;
                    }
                }
            }
        }
    }


    // Actualisation: Fonction qui actualise la Liste des Retenues de la Grille
    // Parametres :
    // sudoku : int[,] : La Grille De Sudoku
    // Liste : List<int>[,] : Liste des Retenues de la grille de Sudoku
    public static void Actualisation(int[,] sudoku, List<int>[,] Liste)
    {
        //Pour chaque case
        for (int lig = 0; lig < sudoku.GetLength(0); lig++)
        {
            for (int col = 0; col < sudoku.GetLength(1); col++)
            {
                //Si la case actuelle est vide
                if (sudoku[lig, col] == 0)
                {
                    //Si un nombre est deja present dans la meme ligne alors il est supprime de la liste
                    for (int veriflig = 0; veriflig < sudoku.GetLength(0); veriflig++)
                    {
                        if (sudoku[veriflig, col] != 0)
                        {
                            Liste[lig, col].Remove(sudoku[veriflig, col]);
                        }
                    }

                    //Si un nombre est deja present dans la meme colonne alors il est supprime de la liste
                    for (int verifcol = 0; verifcol < sudoku.GetLength(1); verifcol++)
                    {
                        if (sudoku[lig, verifcol] != 0)
                        {
                            Liste[lig, col].Remove(sudoku[lig, verifcol]);
                        }
                    }

                    //Initialisation et declaration des dimensions des carres (RESPONSIVE)
                    int carre = (int)Math.Sqrt(sudoku.GetLength(0)) * (lig / (int)Math.Sqrt(sudoku.GetLength(0)));
                    int carrecol = (int)Math.Sqrt(sudoku.GetLength(1)) * (col / (int)Math.Sqrt(sudoku.GetLength(1)));
                    //Verification Carre (RESPONSIVE)
                    for (int veriflig = carre; veriflig < Math.Sqrt(sudoku.GetLength(0)) + carre; veriflig++)
                    {
                        for (int verifcol = carrecol; verifcol < Math.Sqrt(sudoku.GetLength(1)) + carrecol; verifcol++)
                        {
                            if (sudoku[veriflig, verifcol] != 0)
                            {
                                Liste[lig, col].Remove(sudoku[veriflig, verifcol]);
                            }
                        }
                    }
                }
            }
        }
    }

    // Affichage Sudoku : Fonction qui affiche le Sudoku
    // Parametres :
    // sudoku : int[,] : La Grille De Sudoku
    public static void AffichageSudoku(int[,] sudoku)
    {
        Console.WriteLine("");
        Console.WriteLine("Voici la grille de Sudoku traitee par Sudocode :");
        Console.WriteLine("");


        for (int lig1 = 0; lig1 < sudoku.GetLength(0); lig1++)
        {
            for (int col1 = 0; col1 < sudoku.GetLength(1); col1++)
            {
                Console.Write("     " + sudoku[lig1, col1]);

            }
            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }

        //Le Logo
        Console.WriteLine("");
        Console.WriteLine("   ___    _   _    ___     ___     ___     ___     ___     ___   ");
        Console.WriteLine("  / __|  | | | |  |   \\   / _ \\   / __|   / _ \\   |   \\   | __|  ");
        Console.WriteLine("  \\__ \\  | |_| |  | |) | | (_) | | (__   | (_) |  | |) |  | _|   ");
        Console.WriteLine("  |___/   \\___/   |___/   \\___/   \\___|   \\___/   |___/   |___|  ");
        Console.WriteLine("");
        Console.WriteLine("");
    }
}
