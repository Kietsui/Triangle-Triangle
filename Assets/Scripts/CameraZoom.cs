using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    public float zoomSpeed = 1f; // Speed of zooming
    public float minZoom = 2f; // Minimum zoom value
    public float maxZoom = 10f; // Maximum zoom value

    private void Start()
    {
        // Set initial zoom based on current orthographic size
        if (virtualCamera != null)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize, minZoom, maxZoom);
        }
    }

    private void Update()
    {
        if (virtualCamera != null)
        {
            // Get the scroll wheel input
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                // Adjust the orthographic size or field of view based on zoom direction
                virtualCamera.m_Lens.OrthographicSize -= scrollInput * zoomSpeed;

                // Clamp the zoom value to stay within specified limits
                virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize, minZoom, maxZoom);
            }
        }
    }
}
