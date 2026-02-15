using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHUD : MonoBehaviour
{
    public PlayerStats stats;

    [Header("UI Refs (Fill Images)")]
    public Image healthFill;
    public Image forceFill;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    bool gameOverShown = false;

    void Start()
    {
        if (gameOverPanel)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!stats) return;

        if (healthFill) healthFill.fillAmount = stats.Health01();
        if (forceFill) forceFill.fillAmount = stats.Force01();

        // Check for death
        if (!gameOverShown && stats.health <= 0f)
        {
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        gameOverShown = true;

        if (gameOverPanel)
            gameOverPanel.SetActive(true);

        // Optional: freeze the game
        Time.timeScale = 0f;
    }

    // Called by Restart button
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
