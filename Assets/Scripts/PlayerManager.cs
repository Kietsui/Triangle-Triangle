using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Reference to the player's sprite renderer
    private Color originalColor; // Store the original color of the player
    public float flashDuration = 0.352f; // Duration to flash red
    public int playerHp;
    public int playerMaxHp;
    public TMP_Text playerHpText;
    public TMP_Text playerScoreText;
    public int playerScore;
    private GameManager gameManager;

    public delegate void PlayerDeathHandler();
    public event PlayerDeathHandler OnPlayerDeath; // Event for player death

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        originalColor = spriteRenderer.color; // Store the original color
        gameManager = FindObjectOfType<GameManager>(); // Ensure you get the GameManager

        ResetPlayer(); // Initialize player HP and other values
    }

    private void UpdateTexts()
    {
        // Update the UI text fields
        playerHpText.text = $"HP: {playerHp}";  // Display health
        playerScoreText.text = $"Score: {playerScore}";  // Display score
    }

    public void IncrementScore(int amount)
    {
        // Increase the player's score and update the UI
        playerScore += amount;
        UpdateTexts();  // Refresh score display
    }

    public void DamagePlayer(int damage)
    {
        // Apply damage to the player
        TakeDamage(damage);

        // If the player is alive, start the flashing effect
        if (playerHp > 0)
        {
            StartCoroutine(FlashRed());
        }
    }

    private void TakeDamage(int damage)
    {
        // Reduce player health and update the UI
        playerHp -= damage;
        Debug.Log("Player took damage: " + damage);

        UpdateTexts(); // Refresh health display

        // Check if the player is dead
        if (playerHp <= 0)
        {
            HandlePlayerDeath();
        }
    }

    public void SetPlayerActive(bool isActive)
    {
        // Enable or disable player controls
        GetComponent<TriangleController>().enabled = isActive; // Replace with your actual movement script
    }

    private void HandlePlayerDeath()
    {
        // Trigger player death event and handle death logic
        Destroy(gameObject);
        Debug.Log("Player has died!");
        OnPlayerDeath?.Invoke();
        gameManager.EndGame("You Died! Game Over.");
    }

    private System.Collections.IEnumerator FlashRed()
    {
        // Temporarily change the player's sprite color to red to indicate damage
        spriteRenderer.color = Color.red;

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        // Restore the original player color
        spriteRenderer.color = originalColor;
    }

    public void ResetPlayer()
    {
        // Reset player attributes
        playerHp = playerMaxHp;
        playerScore = 0;
        UpdateTexts();  // Refresh the UI after resetting
    }
}
