using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab; // Prefab for the box to spawn
    public int initialNumberOfBoxes = 25; // Initial number of boxes to spawn
    public PolygonCollider2D spawnArea; // Reference to the BoxCollider defining the spawn area

    private int currentBoxCount; // Current count of boxes in the scene

    void Start()
    {
        SpawnInitialBoxes();
    }

    void SpawnInitialBoxes()
    {
        currentBoxCount = 0; // Reset box count

        while (currentBoxCount < initialNumberOfBoxes)
        {
            SpawnBox();
        }
    }

    public void SpawnBox()
    {
        // Get the center and size of the BoxCollider
        Vector2 spawnAreaCenter = spawnArea.bounds.center;
        Vector2 spawnAreaSize = spawnArea.bounds.size;

        // Generate a random position within the bounds of the BoxCollider
        float randomX = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
        float randomY = Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // Instantiate the box at the random position
        Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
        currentBoxCount++; // Increment the box count
    }

    public void BoxDestroyed()
    {
        // Spawn a new box to maintain the minimum count
        SpawnBox();
    }

    // Optional: Call this method to check and maintain box count
    void Update()
    {
        if (currentBoxCount < initialNumberOfBoxes)
        {
            SpawnBox();
        }
    }
}
