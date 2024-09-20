using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject goodBoxPrefab; // Prefab for the good box
    public GameObject badBoxPrefab; // Prefab for the bad box
    public int totalBoxes = 200; // Total number of boxes to spawn
    public int goodBoxCount = 75; // Number of good boxes
    public int badBoxCount = 125; // Number of bad boxes
    public PolygonCollider2D spawnArea; // Reference to the BoxCollider defining the spawn area

    private int currentBoxCount; // Current count of boxes in the scene

    void Start()
    {
        SpawnInitialBoxes();
    }

    void SpawnInitialBoxes()
    {
        currentBoxCount = 0; // Reset box count

        // Spawn the required number of good boxes
        for (int i = 0; i < goodBoxCount; i++)
        {
            SpawnBox(goodBoxPrefab);
        }

        // Spawn the required number of bad boxes
        for (int i = 0; i < badBoxCount; i++)
        {
            SpawnBox(badBoxPrefab);
        }

        currentBoxCount = totalBoxes; // Set the current box count to total boxes
    }

    public void SpawnBox(GameObject boxPrefab)
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
    }

    public void BoxDestroyed()
    {
        currentBoxCount--; // Decrement the current box count when a box is destroyed

        // Spawn a new box to maintain the total count
        if (currentBoxCount < totalBoxes)
        {
            // Determine which type of box to spawn based on the desired counts
            if (goodBoxCount > 0)
            {
                SpawnBox(goodBoxPrefab);
                goodBoxCount--;
            }
            else if (badBoxCount > 0)
            {
                SpawnBox(badBoxPrefab);
                badBoxCount--;
            }
        }
    }

    // Optional: Call this method to check and maintain box count
    void Update()
    {
        // Ensure the current box count does not exceed the total count
        if (currentBoxCount < totalBoxes)
        {
            // Logic to maintain balance can be placed here if necessary
        }
    }
}
