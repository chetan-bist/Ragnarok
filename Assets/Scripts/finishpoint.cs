using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    
    public GameObject finishPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            finishPanel.SetActive(true);
            Time.timeScale = 0f; // pause game
        }
    }

    // public void CloseTutorial()
    // {
    //     finishPanel.SetActive(false);
    //     Time.timeScale = 1f; // game resume
    // }
}