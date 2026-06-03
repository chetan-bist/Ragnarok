using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    
    public GameObject tutorialPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
         if (!PlayerPrefs.HasKey("FirstTimeShowTutorial"))
        {
            // गेम जीवनमा पहिलो पटक खोल्दै छ भने:
            PlayerPrefs.SetInt("FirstTimeShowTutorial", 1);
            PlayerPrefs.Save();

           if (other.CompareTag("Player"))
        {
            tutorialPanel.SetActive(true);
            Time.timeScale = 0f; // pause game
        }
        }
        
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f; // game resume
    }
}
