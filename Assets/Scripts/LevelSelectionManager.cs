using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Level Buttons Setup")]
    // Unity Inspector बाट ५ वटै Button लाई क्रमैसँग यहाँ Drag and Drop गर्ने
    public Button[] levelButtons; 

    void Start()
    {
        // Player कति लेभल सम्म पुग्यो memory बाट लिने (सुरुमा १ हुन्छ)
        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = i + 1; 

            if (levelNumber > reachedLevel)
            {
                // अनलक नभएको लेभल Click गर्न नमिल्ने बनाउने
                levelButtons[i].interactable = false;

                // Locked बटनलाई अलि धमिलो (Grayish) बनाउने
                Color tempColor = levelButtons[i].image.color;
                tempColor.a = 0.4f; 
                levelButtons[i].image.color = tempColor;
            }
            else
            {
                // अनलक भएको लेभलको Button Click गर्दा exact 'level1', 'level2' सीन लोड हुने
                levelButtons[i].interactable = true;
                string targetSceneName = "level" + levelNumber; 
                levelButtons[i].onClick.AddListener(() => LoadTargetLevel(targetSceneName));
            }
        }
    }

    void LoadTargetLevel(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}