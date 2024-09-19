using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            // Implement death logic here
            Destroy(collision.gameObject); // Destroy the player object
            // Optionally, you could restart the level or show a death screen
            Debug.Log("Player is out of bounds and has died.");
        }
    }
}
