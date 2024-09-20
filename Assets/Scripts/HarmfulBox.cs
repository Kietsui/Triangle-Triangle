using UnityEngine;

public class HarmfulBox : MonoBehaviour
{
    private int boxDmg = 1; // Damage dealt to the player on collision
    public ParticleSystem breakEffect; // Particle system for the break effect
    private BoxSpawner boxSpawner; // Reference to the BoxSpawner
    public AudioClip explosionSound; // Sound for breaking the box
    private AudioSource audioSource; // Audio source to play sound
    private PlayerManager playerManager; // Reference to the player manager

    private void Start()
    {
        // Find the BoxSpawner and PlayerManager in the scene
        boxSpawner = FindObjectOfType<BoxSpawner>();
        playerManager = FindAnyObjectByType<PlayerManager>();
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from this GameObject.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage the player when colliding with the box
            playerManager.DamagePlayer(boxDmg);

            // Check if the player is charging (only destroy the box if they are)
            TriangleController triangleController = collision.gameObject.GetComponent<TriangleController>();

            if (triangleController != null && TriangleController.isCharging)
            {
                HandleChargeAttack(); // Destroy the box if the player is charging
                playerManager.IncrementScore(-1);
            }
        }
    }

    void PlayExplosionSound()
    {
        // Play the explosion sound if audioSource is valid
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or explosionSound is not assigned.");
        }
    }

    public void HandleChargeAttack()
    {
        // Handle charge attack which instantly breaks the object
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity).Play();
        }

        PlayExplosionSound(); // Play sound effect
        Destroy(gameObject); // Destroy the box
        boxSpawner.BoxDestroyed(); // Notify the spawner that a box has been destroyed
    }
}
