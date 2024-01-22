using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Button menuButton;

    private void Start()
    {
        // Attach an event listener to the Menu button
        menuButton.onClick.AddListener(MainMenu);
    }

    private void MainMenu()
    {
        // Load the game scene (replace "GameScene" with your actual game scene name)
        SceneManager.LoadScene("Menu");
    }
}
