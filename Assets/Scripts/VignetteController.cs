using UnityEngine;
using UnityEngine.UI;

public class VignetteController : MonoBehaviour
{
    public Transform player;  // Reference to the player object
    public Image vignetteImage;  // Reference to the Image component (vignette)

    public float fadeSpeed = 2f;  // Speed at which the vignette fades
    public float fadeDistance = 5f;  // Distance from the border where the vignette starts to fade in

    [SerializeField] private Collider2D[] mapBorderColliders;  // Array to store map border colliders

    void Update()
    {
        // Calculate the distance to the nearest map border
        float distanceToBorder = GetDistanceToNearestBorder();

        // Calculate alpha based on how close the player is to the border
        float targetAlpha = Mathf.Clamp01((fadeDistance - distanceToBorder) / fadeDistance);

        // Fade the vignette
        Color vignetteColor = vignetteImage.color;
        vignetteColor.a = Mathf.Lerp(vignetteColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
        vignetteImage.color = vignetteColor;
    }

    float GetDistanceToNearestBorder()
    {
        float minDistance = float.MaxValue;

        // Check distance from player to each map border collider
        foreach (var collider in mapBorderColliders)
        {
            // Calculate the distance from the player to each edge of the collider's bounds
            Bounds bounds = collider.bounds;

            float distanceToLeft = Mathf.Abs(player.position.x - bounds.min.x);
            float distanceToRight = Mathf.Abs(player.position.x - bounds.max.x);
            float distanceToBottom = Mathf.Abs(player.position.y - bounds.min.y);
            float distanceToTop = Mathf.Abs(player.position.y - bounds.max.y);

            // Find the shortest distance to any edge
            float distanceToClosestEdge = Mathf.Min(distanceToLeft, distanceToRight, distanceToBottom, distanceToTop);

            if (distanceToClosestEdge < minDistance)
            {
                minDistance = distanceToClosestEdge;
            }
        }

        return minDistance;
    }
}
