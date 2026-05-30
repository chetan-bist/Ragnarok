using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private float moveInput = 0f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    private float groundCheckRadius = 0.2f;

    [Header("Attack Range Settings (NEW)")]
    public Transform attackPoint;      // Sword ko location jahan bata enemy hit check hunchha
    public float attackRange = 1.5f;   // Kati tadako enemy-lai sword-le chhunchha
    public LayerMask enemyLayer;       // Inspector bata Enemy Layer select garna ko lagi
    public float playerDamage = 20f;   // Player ko sword damage

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;

    private bool facingRight = true;
    
    // 🔴 CHANGED: Private bata PUBLIC banaiyo, taaki Health script le direct block status check garna sakosh
    [HideInInspector] public bool isBlocking = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Ground Check
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // 2. Setting Animation Parameters
        if (animator != null)
        {
            animator.SetBool("isWalking", moveInput != 0 && !isBlocking);
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
            animator.SetBool("isBlocking", isBlocking);
        }
    }

    void FixedUpdate()
    {
        if (isBlocking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        // Movement and Flipping
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0 && !facingRight) Flip();
        else if (moveInput < 0 && facingRight) Flip();
    }

    // === UI BUTTON FUNCTIONS ===

    public void MoveLeft() { if(!isBlocking) moveInput = -1f; }
    public void MoveRight() { if(!isBlocking) moveInput = 1f; }
    public void StopMoving() { moveInput = 0f; }

    public void Jump()
    {
        if (isGrounded && !isBlocking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            if(animator != null) animator.SetTrigger("jumpTrigger");
        }
    }

    public void Attack()
    {
        if (isGrounded && !isBlocking && animator != null)
        {
            animator.SetTrigger("attackTrigger");
        }
    }

    public void AttackCombo()
    {
        if (isGrounded && !isBlocking && animator != null)
        {
            animator.SetTrigger("comboTrigger");
        }
    }

    // ========================================================
    // NEW: ANIMATION EVENT FUNCTION (Sword-le chhuda matra call hune)
    // ========================================================
    public void TriggerPlayerDamage()
    {
        if (attackPoint == null) return;

        // Player ko attackPoint ko range vitra vako Enemy range detect garne
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // 1. Enemy ko Health component khojerea damage dine
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(playerDamage);
            }

            // 🔴 FIXED SYNCHRONIZATION: Naya clean EnemyController code target logic set gariyo
            EnemyController enemyAI = enemy.GetComponent<EnemyController>();
            if (enemyAI != null)
            {
                enemyAI.TakeKnockback();
            }
        }
    }

    public void StartBlock()
    {
        if (isGrounded)
        {
            isBlocking = true;
            moveInput = 0f; 
        }
    }

    public void StopBlock()
    {
        isBlocking = false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Gizmos use garera Unity Editor ma Attack range check garne circle dekhauchha (Blue Color)
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}