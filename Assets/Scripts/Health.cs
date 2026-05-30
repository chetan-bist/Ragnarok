using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI References")]
    public Slider healthSlider; 

    [Header("Game Over Settings")]
    public GameObject gameOverPanel; 

    [Header("References")]
    private Animator animator;
    private bool isDead = false;
    public bool isPlayer = false;

    void Start()
    {
        // 🔴 SAFETY EXTRA FIX: Naya level load huda game time state standard 1f (Normal) huna parchha
        Time.timeScale = 1f;

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        if (isPlayer)
        {
            PlayerController player = GetComponent<PlayerController>();
            if (player != null && player.isBlocking)
            {
                Debug.Log("🛡️ BLOCK SUCCESS: Player-le damage block garyo!");
                return; 
            }
        }

        currentHealth -= damageAmount;
        
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; 
            healthSlider.value = currentHealth;
        }

        Debug.Log(gameObject.name + " took damage! Current HP: " + currentHealth);

        if (animator != null)
        {
            animator.SetTrigger("hurtTrigger");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        if (healthSlider != null) healthSlider.value = 0;

        Debug.Log(gameObject.name + " DIED!");

        if (animator != null)
        {
            animator.SetTrigger("dieTrigger");
        }

        if (!isPlayer)
        {
            Invoke("HandleEnemyDeath", 2f);
        }
        else
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if(rb != null) rb.linearVelocity = Vector2.zero;

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true); 
            }

            // 🔴 CHANGED: Player marera panel aaune bittikai background game dynamic physics absolute freeze (Pause) handiyeko
            Time.timeScale = 0f; 
            Debug.Log("⏸️ GAME SCREEN PAUSED: Player died.");
        }
    }

    void HandleEnemyDeath()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.EnemyDefeated();
        }
        Destroy(gameObject);
    }

    public void RestartLevel()
    {
        // 🔴 CHANGED: Level restart huna vanda thik agadi time physics engine state normal (1f) ma pathayeko
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}