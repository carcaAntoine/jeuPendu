﻿using System;


namespace jeupendu
{
    class Program
    {
        //----------Variables Globales----------

        static string correctWord = ""; //mot qui doit être découvert
        static string myLetter = ""; // lettre donnée par l'utilisateur
        static int indexWord = 0;
        static string[] wordsList = new string[] { "abonne", "boueux", "faille", "loyale", "etoffe", "semoir", "noirci", "gateau", "anniversaire", "charcutier" };
        static List<string> correctWordLetters = new List<string>(); //contient les lettres du mot à trouver
        static List<string> myWordLetters = new List<string>(); //contient les lettres trouvées par le joueur
        static List<string> wrongLettersList = new List<string>(); //contient toutes les lettres entrées qui ne sont pas dans le mot
        static int PV = 8; //nombre de chances dont le joueur dispose (PV -= 1 uniquement s'il a faux)
        static int goodLettersCounter = 0; //compte le nombre de lettres trouvées
        static bool isApplication = true;
        static bool letterIsValid = true;

        //--------------------------------------

        static void Main(string[] args)
        {
            while (isApplication == true)
            {
                startGame();
                //gameInProgress();
                
            }
        }



        //----------------------------------------------------------------------------------------------------------------//
        //-------------------------------------------------- FONCTIONS ---------------------------------------------------//
        //----------------------------------------------------------------------------------------------------------------//

        static void startGame() //Lancement du jeu et préparation du mot à trouver
        {
            Console.WriteLine("######################");
            Console.WriteLine("#### Jeu du Pendu ####");
            Console.WriteLine("######################");

            //Création nombre aléatoire pour désigner le mot choisi 
            Random rdn = new Random();
            indexWord = rdn.Next(wordsList.Length - 1);
            correctWord = wordsList[indexWord];

            //Ajoute toutes les lettres du mot dans la liste
            for (int letterIndex = 0; letterIndex < correctWord.Length; letterIndex++)
            {
                correctWordLetters.Add(correctWord[letterIndex].ToString());
                myWordLetters.Add("_");
                //Console.WriteLine("_");
                Console.WriteLine(myWordLetters[letterIndex]);
            }
            
            Console.WriteLine("Vous avez " + PV + " tentatives.");

            // ----------- DEBUG ----------- //
            Console.WriteLine(correctWord);
            // ----------------------------- //
            gameInProgress();
        }

        static void gameInProgress()
        {
            while (PV > 0 && goodLettersCounter != correctWordLetters.Count)
            {
                //Demande la lettre de l'utilisateur
                Console.WriteLine("\nÉcrivez une lettre : ");
                showWrongLetters();
                myLetter = Console.ReadLine();
                if (myLetter.Length != 1)
                {
                    Console.WriteLine("Erreur ! Veuillez écrire une seule lettre.");
                }
                else
                {
                    Console.WriteLine("-----------------------");
                    checkIfLetterIsValid();

                    if (letterIsValid) //Ne vérifie si la lettre est bonne que si elle n'a pas déjà été tentée
                    {
                        checkIfLetterIsGood();
                    }
                    else
                    {
                        letterIsValid = true; //remise valeur par défaut
                    }

                    showMyWord();
                }
            }

            GameOver();
        }

        static void showWrongLetters() //Affiche les lettres déjà tentées
        {
            Console.WriteLine("Lettres incorrectes déjà tentées : ");
            for (int wrongLetter = 0; wrongLetter < wrongLettersList.Count; wrongLetter++)
            {
                Console.WriteLine(wrongLettersList[wrongLetter]);
            }
            Console.WriteLine("------------");

        }

        static void checkIfLetterIsValid() // Vérifie que la lettre n'a pas déjà été proposée
        {
            // Vérifie pour les lettres correctes
            for (int correctLetter = 0; correctLetter < myWordLetters.Count; correctLetter++)
            {
                if (myLetter == myWordLetters[correctLetter])
                {
                    Console.WriteLine("Vous avez déjà joué cette lettre ! Veuillez retenter :");
                    letterIsValid = false;
                }
            }

            // Vérifie pour les lettres incorrectes
            for (int incorrectLetter = 0; incorrectLetter < wrongLettersList.Count; incorrectLetter++)
            {
                if (myLetter == wrongLettersList[incorrectLetter])
                {
                    Console.WriteLine("Vous avez déjà joué cette lettre ! Veuillez retenter :");
                    letterIsValid = false;
                }
            }
        }



        static void checkIfLetterIsGood() //compare la lettre de l'utilisateur avec toutes les lettres du mot à trouver
        {
            int compteurWrongLetter = 0;

            for (int index = 0; index < correctWordLetters.Count; index++) // r = index de la lettre à comparer (dans le mot à trouver)
            {
                if (myLetter == correctWordLetters[index])
                {
                    myWordLetters.RemoveAt(index);
                    myWordLetters.Insert(index, myLetter);
                    goodLettersCounter += 1;
                }
                else
                {
                    compteurWrongLetter += 1;
                }

            }
            
            if (compteurWrongLetter == correctWordLetters.Count)
            {
                PV -= 1;
                wrongLettersList.Add(myLetter);
                Console.WriteLine("Raté ! Il vous reste " + PV + " tentative(s).");
            }

            compteurWrongLetter = 0;
        }

        static void showMyWord() // Montre le mot de l'utilisateur (affiche les lettres trouvées et "_" pour les lettres pas encore trouvées)
        {
            foreach (string lettreToShow in myWordLetters)
            {
                Console.WriteLine(lettreToShow);
            }
        }

        static void GameOver() //Finit la partie (gagné ou perdu)
        {
            if (goodLettersCounter == correctWordLetters.Count)
            {
                Console.WriteLine("GAGNÉ !");
            }
            else
            {
                Console.WriteLine("PERDU !");
                Console.WriteLine("Le mot à trouver était " + correctWord);
            }
            retryOption();
        }

        static void retryOption()      
        {
            Console.WriteLine("Voulez-vous Retenter ou Quitter ? (1 ou 0)");
            string retry = Console.ReadLine();
            if (retry == "1")
            {
                PV = 8;
                goodLettersCounter = 0;

                correctWordLetters.Clear();
                myWordLetters.Clear();
                wrongLettersList.Clear();
                startGame();
            }
            else if (retry == "0")
            {
                isApplication = false;
            }
            else
            {
                Console.WriteLine("Entrez 0 ou 1 et rien d'autre");
                retryOption();
            }
        }
    }
}




