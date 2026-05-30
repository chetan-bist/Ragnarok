using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float stoppingDistance = 1.5f; 

    [Header("Attack Settings")]
    public float swordDamage = 15f;
    public float attackRate = 1.5f; 
    private float nextAttackTime = 0f;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;

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
        if (player == null || isKnockedBack) return;

        LookAtPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Player range ma chha bhane ra Cooldown sakieko chha bhane Attack garne
        if (distanceToPlayer <= stoppingDistance && Time.time >= nextAttackTime)
        {
            SwordAttack();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void FixedUpdate()
    {
        if (player == null || isKnockedBack) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 currentVelocity = rb.linearVelocity;

        // Player range bhanda tada chha bhane hiddai jane
        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 targetPosition = new Vector2(player.position.x, currentVelocity.y);
            Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
            
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, currentVelocity.y);
            
            if(animator != null) animator.SetBool("isWalking", true);
        }
        else
        {
            // Range ma pugepachi rokkine
            rb.linearVelocity = new Vector2(0, currentVelocity.y);
            if(animator != null) animator.SetBool("isWalking", false);
        }
    }

    void SwordAttack()
    {
        if (animator != null) animator.SetTrigger("swordAttackTrigger");
    }

    // ANIMATION EVENT: Enemy ko sword attack frame ma yo call hunchha
    public void TriggerSwordDamage()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= stoppingDistance)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(swordDamage);
                Debug.Log("Enemy Sword-le Player-lai lagayo!");
            }
        }
    }

    // NEW: External function jahan bata Player-le hit garda yo run hunchha
    public void TakeKnockback()
    {
        if (player == null) return;

        // Player bhanda ulto side tira push garne direction nikalne
        float direction = transform.position.x > player.position.x ? 1f : -1f;
        
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(KnockbackRoutine(direction));
        }
    }

    private IEnumerator KnockbackRoutine(float direction)
    {
        isKnockedBack = true;
        
        // Ali pachhadi dhakalne force dine
        rb.linearVelocity = new Vector2(direction * knockbackForce, rb.linearVelocity.y);
        
        yield return new WaitForSeconds(knockbackDuration);
        
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
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