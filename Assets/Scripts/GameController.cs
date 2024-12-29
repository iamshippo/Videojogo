using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int whoTurn; // 0 = X and 1 = O
    public int turnCount; // Counts the number of turns played
    public GameObject[] turnIcons; // Displays whose turn it is
    public Sprite[] playIcons; // 0 = X icon and 1 = O icon
    public Button[] tictactoeSpaces; // Playable spaces in our game
    public int[] markedSpaces; // IDs which space was marked by which player
    public Text WinnerText; // Holds the text component of the winner text
    public GameObject[] winningLine; // Holds all the different lines to show a winner
    public GameObject winnerPanel;
    public int xPlayerScore;
    public int oPlayerScore;
    public Text xPlayerScoreText;
    public Text oPlayerScoreText;
    public AudioSource buttonClickAudio;
    public AudioSource victoryAudio; // Added variable for victory sound
    public AudioSource rematchAudio; // Added variable for rematch sound
    public AudioSource restartAudio; // Added variable for restart sound
    private bool isRestarting = false; // Flag to differentiate restart from rematch

    // Variable pública para controlar el tamaño de la imagen
    public float iconScaleFactor = 0.5f; // Factor de escala para hacer la imagen más pequeña

    void Start()
    {
        GameSetup();
    }

    void GameSetup()
    {
        whoTurn = 0;
        turnCount = 0;
        winnerPanel.SetActive(false);
        WinnerText.gameObject.SetActive(false);

        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);

        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            tictactoeSpaces[i].interactable = true;
            tictactoeSpaces[i].GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < markedSpaces.Length; i++)
        {
            markedSpaces[i] = -100; // Initialize as unmarked
        }
    }

    public void TicTacToeButton(int WhichNumber)
    {
        tictactoeSpaces[WhichNumber].GetComponent<Image>().sprite = playIcons[whoTurn];
        tictactoeSpaces[WhichNumber].interactable = false;

        markedSpaces[WhichNumber] = whoTurn + 1;
        turnCount++;

        // Escalar el icono para hacerlo más pequeño
        ScaleIcon(tictactoeSpaces[WhichNumber].transform);

        if (turnCount > 4) // Only start checking for a winner after 5 moves
        {
            WinnerCheck();
        }

        // Switch turn
        if (whoTurn == 0)
        {
            whoTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
        }
        else
        {
            whoTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
        }
    }

    // Función para ajustar la escala del icono
    void ScaleIcon(Transform iconTransform)
    {
        iconTransform.localScale = new Vector3(iconScaleFactor, iconScaleFactor, iconScaleFactor);
    }

    void WinnerCheck()
    {
        int s1 = markedSpaces[0] + markedSpaces[1] + markedSpaces[2];
        int s2 = markedSpaces[3] + markedSpaces[4] + markedSpaces[5];
        int s3 = markedSpaces[6] + markedSpaces[7] + markedSpaces[8];
        int s4 = markedSpaces[0] + markedSpaces[3] + markedSpaces[6];
        int s5 = markedSpaces[1] + markedSpaces[4] + markedSpaces[7];
        int s6 = markedSpaces[2] + markedSpaces[5] + markedSpaces[8];
        int s7 = markedSpaces[0] + markedSpaces[4] + markedSpaces[8];
        int s8 = markedSpaces[2] + markedSpaces[4] + markedSpaces[6];
        var solutions = new int[] { s1, s2, s3, s4, s5, s6, s7, s8 };

        for (int i = 0; i < solutions.Length; i++)
        {
            if (solutions[i] == 3 * (whoTurn + 1))
            {
                WinnerDisplay(i);
                return;
            }
        }
    }

    void WinnerDisplay(int indexIn)
    {
        winnerPanel.SetActive(true); // Show winner panel
        WinnerText.gameObject.SetActive(true);

        // Display the correct winner
        if (whoTurn == 0)
        {
            xPlayerScore++;
            xPlayerScoreText.text = xPlayerScore.ToString();
            WinnerText.text = "Player X Wins!";
        }
        else if (whoTurn == 1)
        {
            oPlayerScore++;
            oPlayerScoreText.text = oPlayerScore.ToString();
            WinnerText.text = "Player O Wins!";
        }

        // Play victory sound
        if (victoryAudio != null)
        {
            victoryAudio.Play();
        }

        // Show the winning line
        winningLine[indexIn].SetActive(true);

        // Disable further interaction with the game board
        for (int i = 0; i < tictactoeSpaces.Length; i++)
        {
            tictactoeSpaces[i].interactable = false;
        }
    }

    public void Rematch()
    {
        GameSetup();
        for (int i = 0; i < winningLine.Length; i++)
        {
            winningLine[i].SetActive(false);
        }
        winnerPanel.SetActive(false); // Corrected typo

        // Play rematch sound only if not restarting
        if (rematchAudio != null && !isRestarting)
        {
            rematchAudio.Play();
        }
    }

    public void Restart()
    {
        isRestarting = true; // Set flag to true for restart
        Rematch(); // Calls rematch logic, excluding rematch sound
        xPlayerScore = 0;
        oPlayerScore = 0;
        xPlayerScoreText.text = "0";
        oPlayerScoreText.text = "0";

        // Play restart sound
        if (restartAudio != null)
        {
            restartAudio.Play();
        }

        isRestarting = false; // Reset flag
    }

    public void PlayButtonClick()
    {
        buttonClickAudio.Play();
    }
}
