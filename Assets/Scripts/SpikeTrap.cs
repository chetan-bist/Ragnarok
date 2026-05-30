using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public float trapDamage = 20f; // Ek choti chhuda kati life ghaatne
    public float damageInterval = 1f; // Trap mathi adaptive basirahe kati sec ko gap ma damage dine
    
    private float nextDamageTime = 0f;

    // Yo function dynamic run hunchha jaba player trap vitra enter garchha
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check garne chhune object Player ho ki hoina
        if (other.CompareTag("Player"))
        {
            // Timer check garne (Harek 1 second ma matra damage hosh bhannala)
            if (Time.time >= nextDamageTime)
            {
                // Player ko Health script khojne
                Health playerHealth = other.GetComponent<Health>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(trapDamage);
                    Debug.Log("Trap-le Player-lai lagayo! Damage: " + trapDamage);
                }

                // Next damage hune time set garne
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}