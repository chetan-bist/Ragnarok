using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("UI Panel Settings")]
    public GameObject winPanel; // Unity bata setup garne Congratulations Panel reference

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Game suru huda visual win panel screen bata complete ON vaye pani hide (OFF) rakhne
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    // 🔴 CHANGED: Enemy complete dead huda yo call hunchha
    public void EnemyDefeated()
    {
        Debug.Log("Enemy haryo! Showing Win Panel...");
        
        // 1. Ekdum paila screen ma Congratulations UI Panel show (ON) garne
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    // 🔴 NEW: Next Level UI Button le click garda yo function run garcha
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Sabai level sakiyo! Going back to Main Menu (Scene 0)");
            SceneManager.LoadScene(0); // Pura game sakye pachi Main Menu scene layout grid load handine
        }
    }

    // 🔴 NEW: Quit UI Button le click garda yo function run garcha
    public void QuitGame()
    {
        Debug.Log("Game Quit vayo!");
        Application.Quit(); // Mobile ya PC ma application exit code pathako
    }
}