using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Game : MonoBehaviour
{
    // ---------- UNITY ---------- //
    public GameObject[] potenceImage; //image dans laquelle on va afficher les sprites du pendu
    public Sprite[] sprites; //sprites du pendu
    public Sprite[] notes;
    public Text WordToFindText;
    public Text triedLettersText; //Lettres déjà tentées
    public GameObject GameOverCanvas;
    public Text ResponsePendu;
    public GameObject alreadyTriedMessage;

    // ---------- VARIABLES ---------- //

    static string correctWord = ""; //mot qui doit être découvert
    static string myLetter = ""; // lettre donnée par l'utilisateur
    static int indexWord = 0;
    static string[] wordsList = new string[] { "abonne", "boueux", "faille", "loyale", "etoffe", "semoir", "noirci", "gateau", "anniversaire", "charcutier" };
    static List<string> correctWordLetters = new List<string>(); //contient les lettres du mot à trouver
    static List<string> myWordLetters = new List<string>(); //contient les lettres trouvées par le joueur
    static List<string> wrongLettersList = new List<string>(); //contient toutes les lettres entrées qui ne sont pas dans le mot
    static int PV = 7; //nombre de chances dont le joueur dispose (PV -= 1 uniquement s'il a faux)
    static int spriteIndex = 0; //Détermine quel sprite du pendu afficher
    static int goodLettersCounter = 0; //compte le nombre de lettres trouvées
    static bool letterIsValid = true;
    static bool gameIsOver = false; //indique si la partie est finie ou non
    public string resultat; //contient le message de fin à afficher
    public int indexNote; //index de l'image Note (0/20 ou 20/20)

    void Start()
    {
        alreadyTriedMessage = GameObject.Find("LettreTenteeMessage");
        InitGame();
    }

    void InitGame()
    {
        WordToFindText = GameObject.Find("MotADeviner").GetComponent<Text>();
        WordToFindText.text = "";
        triedLettersText = GameObject.Find("LettresTenteesTexte").GetComponent<Text>();
        triedLettersText.text = "";

        GameOverCanvas = GameObject.Find("EndGameCanvas");
        ResponsePendu = GameOverCanvas.transform.GetChild(2).GetComponent<Text>();
        
        GameOverCanvas.SetActive(false);
        alreadyTriedMessage.SetActive(false);
        
        
        DisplayPendu(spriteIndex);
        StartGame();
    }

    void DisplayPendu(int i)
    {
        potenceImage[0].transform.GetComponent<Image>().sprite = sprites[i];
    }

    void StartGame() //Lancement du jeu et préparation du mot à trouver
    {
        //Création nombre aléatoire pour désigner le mot choisi 
        System.Random rdn = new System.Random();
        indexWord = rdn.Next(wordsList.Length - 1);
        correctWord = wordsList[indexWord];

        //Ajoute toutes les lettres du mot dans la liste
        for (int letterIndex = 0; letterIndex < correctWord.Length; letterIndex++)
        {
            correctWordLetters.Add(correctWord[letterIndex].ToString());
            myWordLetters.Add("_ ");

        }

        // ----------- DEBUG ----------- //
        Debug.Log(correctWord);
        // ----------------------------- //
        ShowMyWord();
    }

    void ShowMyWord() // Montre le mot de l'utilisateur (affiche les lettres trouvées et "_" pour les lettres pas encore trouvées)
    {
        WordToFindText.text = ""; // Empêche le mot de se démultiplier à l'affichage
        foreach (string lettreToShow in myWordLetters)
        {
            WordToFindText.text += lettreToShow;
        }
    }

    void ReadStringInput(string s) //Récupère la valeur de l'Input Field
    {

        myLetter = s.ToLower();
        alreadyTriedMessage.SetActive(false);
        if (myLetter.Length == 1 && !gameIsOver)
        {
            GameInProgress();
        }

    }

    void GameInProgress()
    {
        CheckIfLetterIsValid();

        if (letterIsValid) //Ne vérifie si la lettre est bonne que si elle n'a pas déjà été tentée
        {
            CheckIfLetterIsGood();
        }
        else
        {
            letterIsValid = true; //remise valeur par défaut
        }

        ShowMyWord();
        Debug.Log("nombre de bonnes lettres : " + goodLettersCounter);
        Debug.Log("nombre de lettres à trouver : " + correctWordLetters.Count);

        if (PV == 0 || goodLettersCounter == correctWordLetters.Count)
        {

            EndGame();
        }

    }

    void CheckIfLetterIsValid() // Vérifie que la lettre n'a pas déjà été proposée
    {
        // Vérifie pour les lettres correctes
        for (int correctLetter = 0; correctLetter < myWordLetters.Count; correctLetter++)
        {
            if (myLetter == myWordLetters[correctLetter])
            {
                Debug.Log("Vous avez déjà joué cette lettre ! Veuillez retenter :");
                alreadyTriedMessage.SetActive(true);
                letterIsValid = false;
            }
        }

        // Vérifie pour les lettres incorrectes
        for (int incorrectLetter = 0; incorrectLetter < wrongLettersList.Count; incorrectLetter++)
        {
            if (myLetter == wrongLettersList[incorrectLetter])
            {
                Debug.Log("Vous avez déjà joué cette lettre ! Veuillez retenter :");
                alreadyTriedMessage.SetActive(true);
                letterIsValid = false;
            }
        }
    }

    void CheckIfLetterIsGood() //compare la lettre de l'utilisateur avec toutes les lettres du mot à trouver
    {
        int compteurWrongLetter = 0;

        for (int index = 0; index < correctWordLetters.Count; index++) // index de la lettre à comparer (dans le mot à trouver)
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
            triedLettersText.text = triedLettersText.text + myLetter + " ";
            wrongLettersList.Add(myLetter);
            Debug.Log("Raté ! Il vous reste " + PV + " tentative(s).");
            spriteIndex += 1;
            DisplayPendu(spriteIndex);

        }

        compteurWrongLetter = 0;
    }

    void EndGame() //Finit la partie (gagné ou perdu)
    {
        gameIsOver = true;

        if (goodLettersCounter == correctWordLetters.Count)
        {
            resultat = "GAGNÉ !";
            indexNote = 0;
            ResponsePendu.text = "";

        }
        else
        {
            resultat = "PERDU !";
            indexNote = 1;
            ResponsePendu.text = "Le mot à trouver était " + correctWord;

        }
        DisplayEndGameCanvas(indexNote, resultat);

    }

    void DisplayEndGameCanvas(int noteIndex, string message)
    {
        GameOverCanvas.transform.GetChild(0).GetComponent<Image>().sprite = notes[noteIndex];
        GameOverCanvas.transform.GetChild(1).GetComponent<Text>().text = message;
        GameOverCanvas.SetActive(true);
    }

    public void NewGame()
    {

        PV = 7;
        goodLettersCounter = 0;
        spriteIndex = 0;

        gameIsOver = false;

        correctWordLetters.Clear();
        myWordLetters.Clear();
        wrongLettersList.Clear();
        InitGame();

        GameOverCanvas.SetActive(false);

    }

    public void DoExitGame()
    {
        Application.Quit();
    }

}
