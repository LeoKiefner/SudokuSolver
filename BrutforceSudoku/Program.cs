using System;
using System.Collections.Generic;
using System.IO;

class sudoku
{
    static void Main()
    {
        //Creation de la grille de Sudoku vide
        int[,] sudoku = new int[0, 0];

        //Appel a la fonction Menu
        Menu(sudoku);
    }

    // MenuPrincipal : Fonction qui affiche un menu a l'utilisateur lui permettant de resoudre ou quitter
    // Parametres :
    // sudoku : int[,] : La grille de Sudoku
    public static void Menu(int[,] sudoku)
    {
        //Affichage du Menu
        Console.Clear();
        Console.WriteLine("Comment voulez vous initialiser votre grille ?");
        Console.WriteLine("");
        Console.WriteLine("1. A partir d'un fichier");
        Console.WriteLine("");
        Console.WriteLine("2. Manuellement case par case");

        //Prise en compte du choix
        int choix1 = int.Parse(Console.ReadLine());

        if (choix1 == 1) { sudoku = GenererGrille(); }

        if (choix1 == 2)
        {
            //Appel a la fonction de la grille de Sudoku
            sudoku = DeclaGrille();

            //Appel a la fonction de modification/initialisation de la grille
            sudoku = InitGrille(sudoku);
        }



        //Appel a la fonction de verification de la grille
        VerifGrille1(sudoku);

        //Methode des Notes
        List<int>[,] Liste = TechniqueNotes(sudoku);

        //Appel a la fonction brutforce
        sudoku = Brutforce(sudoku, Liste);
    }


    // DeclaGrille : Fonction qui declare la grille de Sudoku par l'utilisateur (avcec ses propres dimensions)
    // Parametres :
    // Aucun
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

    // InitGrille : Fonction qui initialise la grille manuellement
    // Parametres :
    // sudoku : int[,] : La grille de Sudoku
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

                sudoku[lig, col] = int.Parse(Console.ReadLine());
            }
        }

        return sudoku;
    }
    // GenererGrille : Fonction qui declare et initialise la grille a partir d'un fichier par l'utilisateur
    // Parametres :
    // Aucun
    public static int[,] GenererGrille()
    {
        Console.WriteLine("Entrez le nom de votre fichier (ex : simple.txt) : ");
        string[] fichier_entier = File.ReadAllLines(Directory.GetCurrentDirectory() + "/" + Console.ReadLine());
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


    // VerifGrille : Fonction qui verifie la Grille (RESPONSIVE)
    // Parametres :
    // sudoku : int[,] : La grille de Sudoku
    public static bool VerifGrille1(int[,] sudoku)
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

    // BrutForce : Fonction qui resoud la grille de Sudoku en utilisant une methode "BrutForce" optimisee
    // Parametres :
    // sudoku : int[,] : La grille de Sudoku
    // Listee : List<int>[,] : La liste des retenues du tableau de sudoku
    public static int[,] Brutforce(int[,] sudoku, List<int>[,] Liste)
    {
        bool possible = true;

        int[,] grillefin = new int[sudoku.GetLength(0), sudoku.GetLength(1)];
        List<int>[,] ListeFin = new List<int>[sudoku.GetLength(0), sudoku.GetLength(1)];
        while (VerifGrille1(grillefin) == false)
        {
            possible = true;
            for (int lig = 0; lig < sudoku.GetLength(0); lig++)
            {
                for (int col = 0; col < sudoku.GetLength(1); col++)
                {
                    grillefin[lig, col] = sudoku[lig, col];

                    ListeFin[lig, col] = new List<int>();
                    foreach (int val in Liste[lig, col])
                    {
                        ListeFin[lig, col].Add(val);
                    }
                }
            }

            while (possible && VerifGrille1(grillefin) == false)
            {

                for (int lig = 0; lig < sudoku.GetLength(0) && possible; lig++)
                {
                    for (int col = 0; col < sudoku.GetLength(1) && possible; col++)
                    {
                        if (grillefin[lig, col] == 0 && ListeFin[lig, col].Count == 0)
                        {
                            possible = false;
                            //Console.WriteLine("permier");
                        }

                        else if (grillefin[lig, col] == 0)
                        {
                            //Console.WriteLine("deuxieme" + lig + "," + col);
                            Random Rnd = new Random();
                            int valeur = Rnd.Next(0, ListeFin[lig, col].Count);
                            grillefin[lig, col] = ListeFin[lig, col][valeur];
                            ListeFin = TechniqueNotes(grillefin);

                            /*Console.Clear();
                            for (int lig1 = 0; lig1 < sudoku.GetLength(0); lig1++)
                            {
                                for (int col1 = 0; col1 < sudoku.GetLength(1); col1++)
                                {
                                    Console.Write("     " + grillefin[lig1, col1]);
                                }

                                Console.WriteLine(" ");
                                Console.WriteLine(" ");

                            }*/

                        }
                    }
                }
            }

        }

        Console.WriteLine("La grille de Sudoku resolue avec le Brutforce :");
        Console.WriteLine(" ");

        for (int lig1 = 0; lig1 < sudoku.GetLength(0); lig1++)
        {
            for (int col1 = 0; col1 < sudoku.GetLength(1); col1++)
            {
                Console.Write("     " + grillefin[lig1, col1]);
            }
            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }


        return grillefin;
    }

    // TechniqueNotes : Fonction qui cree toutes les notes pour chaque case dans la grille de sudoku
    // Parametres :
    // sudoku : int[,] : La grille de Sudoku
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
}