using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 60f; // Total game duration in seconds
    private float timeRemaining; // Time remaining in the game
    private bool isGameActive = false; // Track if the game is active or over

    public TMP_Text timerText; // UI text for the timer
    public GameObject gameOverUI; // UI element to display on game over
    public TMP_Text gameOverMessage; // Message to display on game over
    public TMP_Text restartPromptText; // Text to prompt the user to restart
    public GameObject playerPrefab; // Prefab for the player
    private GameObject currentPlayer; // Current player instance

    private void Start()
    {
        gameOverUI.SetActive(false); // Hide game-over UI at the start
        restartPromptText.gameObject.SetActive(false); // Hide the restart prompt initially
        StartCoroutine(StartGameWithCountdown()); // Start the game with a countdown
    }

    private void Update()
    {
        if (isGameActive)
        {
            UpdateTimer(); // Update the timer only if the game is active
        }
        // No need to handle game over input anymore
    }

    private IEnumerator StartGameWithCountdown()
    {
        FreezeGame(true);

        // Display countdown
        for (int i = 3; i > 0; i--)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
        }

        FreezeGame(false);
        StartGame();
    }

    private void FreezeGame(bool freeze)
    {
        if (currentPlayer != null)
        {
            currentPlayer.GetComponent<PlayerManager>().SetPlayerActive(!freeze);
        }
    }

    private void StartGame()
    {
        timeRemaining = gameDuration;
        isGameActive = true;

        // Instantiate a new player object
        if (currentPlayer != null)
        {
            Destroy(currentPlayer); // Destroy existing player if it exists
        }

        currentPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); // Use desired starting position
        PlayerManager player = currentPlayer.GetComponent<PlayerManager>(); // Get the PlayerManager component
        player.ResetPlayer(); // Reset player stats

        restartPromptText.gameObject.SetActive(false); // Hide restart prompt when the game starts
    }

    public void EndGame(string message)
    {
        isGameActive = false;
        gameOverMessage.text = message; // Set the game-over message
        gameOverUI.SetActive(true); // Show the game-over UI
        restartPromptText.gameObject.SetActive(true); // Show restart prompt
    }

    private void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            EndGame("Time's up! Game Over.");
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay = Mathf.Clamp(timeToDisplay, 0, Mathf.Infinity);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
        Debug.Log("Game is quitting..."); // For testing purposes, to show in the console
    }
}
