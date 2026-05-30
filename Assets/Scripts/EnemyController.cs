using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float stoppingDistance = 1.8f; 

    [Header("Attack Settings")]
    public float swordDamage = 15f; 
    public float attackRate = 1.5f;
    private float nextAttackTime = 0f;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;       // Kati jorle pachhadi dhakalne
    public float knockbackDuration = 0.2f;   // Kati ber samma pachhadi dhakalne
    private bool isKnockedBack = false;      // Knockback lagirako chha ki nai check garne

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        PlayerController p = FindFirstObjectByType<PlayerController>();
        if (p != null) player = p.transform;
    }

    void Update()
    {
        // Knockback vako belai movement ra attack sabai freeze garne
        if (player == null || isKnockedBack) return;

        LookAtPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Core Attack State: Stopping distance vitra pugepachi aafai Attack trigger thichne
        if (distanceToPlayer <= stoppingDistance && Time.time >= nextAttackTime)
        {
            if (animator != null)
            {
                animator.SetTrigger("swordAttackTrigger");
            }
            nextAttackTime = Time.time + attackRate;
        }
    }

    void FixedUpdate()
    {
        // Knockback vako belai physics velocity freeze garne
        if (player == null || isKnockedBack) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 currentVelocity = rb.linearVelocity;

        // Core Movement State: Player tada vaye Walk garne, najik vaye Idle (Speed 0) basne
        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 targetPosition = new Vector2(player.position.x, currentVelocity.y);
            Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
            
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, currentVelocity.y);
            
            if (animator != null) animator.SetBool("isWalking", true);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, currentVelocity.y);
            if (animator != null) animator.SetBool("isWalking", false);
        }
    }

    // Animation Event Pin le call garne function (Sword-le chhuda matra hit lagne)
    public void TriggerSwordDamage()
    {
        if (player == null || isKnockedBack) return; 

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= stoppingDistance + 0.5f) 
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(swordDamage);
                Debug.Log("🎯 SUCCESS: Enemy Sword-le Player-lai hit garyo!");
            }
        }
    }

    // Player-le hit garda Enemy-lai knockback line trigger
    public void TakeKnockback()
    {
        if (player == null) return;

        float direction = transform.position.x > player.position.x ? 1f : -1f;
        
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(KnockbackRoutine(direction));
        }
    }

    private IEnumerator KnockbackRoutine(float direction)
    {
        isKnockedBack = true;
        
        // 🔴 CLEAN EMBED: Knockback lagda walk animation lai instant False (Idle state) banako
        if (animator != null) animator.SetBool("isWalking", false);
        
        rb.linearVelocity = new Vector2(direction * knockbackForce, rb.linearVelocity.y);
        
        yield return new WaitForSeconds(knockbackDuration);
        
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        isKnockedBack = false;
    }

    void LookAtPlayer()
    {
        if (player.position.x > transform.position.x && !facingRight) Flip();
        else if (player.position.x < transform.position.x && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}