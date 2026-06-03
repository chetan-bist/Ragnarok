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

         // --- नयाँ लेभल अनलक गर्ने कोड ---
        string sceneName = SceneManager.GetActiveScene().name;
        string levelNumberString = sceneName.Replace("level", ""); 
        
        int currentLevel;
        if (int.TryParse(levelNumberString, out currentLevel))
        {
            int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

            if (currentLevel == reachedLevel)
            {
                PlayerPrefs.SetInt("ReachedLevel", currentLevel + 1);
                PlayerPrefs.Save(); 
                Debug.Log("Next Level Unlocked: Level " + (currentLevel + 1));
            }
        }
    }

    // 🔴 NEW: Next Level UI Button le click garda yo function run garcha
    public void LoadNextLevel()
    {
        // int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        // {
        //     SceneManager.LoadScene(nextSceneIndex);
        // }
        // else
        // {
        //     Debug.Log("Sabai level sakiyo! Going back to Main Menu (Scene 0)");
        //     SceneManager.LoadScene(0); // Pura game sakye pachi Main Menu scene layout grid load handine
        // }

        Time.timeScale = 1f; // गेम सुरुमा अनपज गर्ने (नत्र अर्को लेभल पनि रोकिएकै हुन्छ)
        // अर्को लेभल लोड गर्ने (Build Settings को आधारमा)
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
         // --- तपाईंको पुरानो First-Time Player Logic यहाँ छ ---
        if (!PlayerPrefs.HasKey("FirstTimePlayed"))
        {
            // गेम जीवनमा पहिलो पटक खोल्दै छ भने:
            PlayerPrefs.SetInt("FirstTimePlayed", 1);
            PlayerPrefs.Save();

            // सिधै Level1 लोड गर्ने (तपाईंको पुरानो कोडमा Scene Index २ थियो)
            // नोट: यदि Build Settings मा level1 को index 2 हो भने ठीक छ, नत्र "level1" नाम लेख्दा झन् सुरक्षित हुन्छ।
            SceneManager.LoadScene(2); 
        }
        else
        {
            // पहिले नै खेलिसकेको प्लेयर हो भने Map (Level Selection) मा पठाउने
            SceneManager.LoadScene("LevelSelection"); 
        }
    }

    // 🔴 NEW: Quit UI Button le click garda yo function run garcha
    public void QuitGame()
    {
        Debug.Log("Game Quit vayo!");
        Application.Quit(); // Mobile ya PC ma application exit code pathako
    }
}