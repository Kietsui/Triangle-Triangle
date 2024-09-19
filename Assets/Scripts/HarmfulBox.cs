using Unity.VisualScripting;
using UnityEngine;

public class HarmfulBox : MonoBehaviour
{
    public ParticleSystem breakEffect; // Particle system for the break effect
    private BoxSpawner boxSpawner; // Reference to the BoxSpawner
    public AudioClip explosionSound;
    private AudioSource audioSource;

    private void Start()
    {
        // Find the BoxSpawner in the scene
        boxSpawner = FindObjectOfType<BoxSpawner>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
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