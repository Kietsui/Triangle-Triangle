using UnityEngine;

public class BoxDamage : MonoBehaviour
{
    public ParticleSystem breakEffect; // Particle system for the break effect
    private BoxSpawner boxSpawner; // Reference to the BoxSpawner
    public AudioClip explosionSound;
    private AudioSource audioSource;
    private PlayerManager playerManager;

    private void Start()
    {
        // Find the BoxSpawner in the scene
        boxSpawner = FindObjectOfType<BoxSpawner>();
        audioSource = GetComponent<AudioSource>();
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the collider is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            TriangleController triangleController = collision.gameObject.GetComponent<TriangleController>();
            
            // Check if the player is currently charging
            if (triangleController != null && TriangleController.isCharging)
            {
                // Instantiate the break effect
                if (breakEffect != null)
                {
                    Instantiate(breakEffect, transform.position, Quaternion.identity).Play();
                    PlayExplosionSound();
                    playerManager.IncrementScore(1);
                }
                Destroy(gameObject); // Destroy the game object after playing the effect
                boxSpawner.BoxDestroyed(); // Notify the spawner that a box has been destroyed
                Debug.Log("Box destroyed by charge attack.");
            }
        }
    }

    void PlayExplosionSound()
    {
        // Play the explosion sound
        audioSource.PlayOneShot(explosionSound);
    }
    
    public void HandleChargeAttack()
    {
        // Handle charge attack which instantly breaks the object
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity).Play();
            PlayExplosionSound();
        }
        Destroy(gameObject); // Destroy the game object immediately
        boxSpawner.BoxDestroyed(); // Notify the spawner that a box has been destroyed
    }
}
