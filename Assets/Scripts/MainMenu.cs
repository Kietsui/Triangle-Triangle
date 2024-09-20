using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton; // Reference to the UI Button

    private void Start()
    {
        // Add a listener to the button to call LoadGameScene when clicked
        if (startButton != null)
        {
            startButton.onClick.AddListener(LoadGameScene);
        }
    }

    // Method to load the game scene
    public void LoadGameScene()
    {
        // Replace "GameScene" with the actual name of your game scene
        SceneManager.LoadScene("SampleScene");
    }
}
