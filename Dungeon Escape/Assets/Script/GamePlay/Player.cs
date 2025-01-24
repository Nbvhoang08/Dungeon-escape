using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;
    public bool HasKey;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 12f);
    public bool IsDead;
    public bool IsDone;
    public int StarScore;
    public Animator  anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float offset;
    private bool isTouchingScreen;
    void Start()
    {
        IsDead = false;
        IsDone = false;
    }
    private void Update()
    {
        if(IsDead) return;
        if(IsDone) return;

        anim.SetBool("OnGround",IsGrounded());
        anim.SetBool("OnWall",isWallSliding);
        anim.SetBool("IsFalling", rb.velocity.y <= 0);


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isTouchingScreen = true;

                if (IsGrounded())
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                }
                else if (isWallSliding)
                {
                    WallJump();
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isTouchingScreen = false;

                if (rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                }
            }
        }

        WallSlide();
        WallJump();
    }

    private void FixedUpdate()
    {
        if(IsDead) return;
        if (!isWallJumping)
        {
            // Di chuyển tự động dựa trên hướng của Player
            float direction = isFacingRight ? 1f : -1f;
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra va chạm với object có tag "Spike"
        if (collision.CompareTag("Spike") && !IsDead)
        {
            IsDead = true; // Đảm bảo chỉ chết một lần
            Die();
        }
        else if(collision.CompareTag("Spike") && !IsDone)
        {
            IsDone = true;
        }
    }

    IEnumerator DoneGame()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale =0;
    }


    [Header("Death Settings")]
    [SerializeField] private GameObject deathEffectPrefab; // Prefab hiệu ứng chết
    [SerializeField] private GameObject spriteRenderer; // Sprite của Player
    private void Die()
    {
        // 1. Ngừng hiển thị sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.SetActive(false);
        }

        // 2. Spawn hiệu ứng chết tại vị trí của Player
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // 3. Thu nhỏ scale sau 0.5 giây
        StartCoroutine(ShrinkAndDestroy());
    }

    private IEnumerator ShrinkAndDestroy()
    {
        yield return new WaitForSeconds(1.6f);
        Time.timeScale = 0;
        
        gameObject.SetActive(false); // Tắt object
        
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        // Kiểm tra va chạm tại vị trí chính, trên và dưới
        bool isWalledCenter = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        bool isWalledTop = Physics2D.OverlapCircle(wallCheck.position + Vector3.up * offset, 0.2f, wallLayer);
        bool isWalledBottom = Physics2D.OverlapCircle(wallCheck.position + Vector3.down * offset, 0.2f, wallLayer);

        // Trả về true nếu bất kỳ vị trí nào chạm tường
        return isWalledCenter || isWalledTop || isWalledBottom;
    }
    private void OnDrawGizmos()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;

            // Vẽ vòng tròn ở vị trí chính
            Gizmos.DrawWireSphere(wallCheck.position, 0.2f);

            // Vẽ vòng tròn lệch lên
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position + Vector3.up * offset, 0.2f);

            // Vẽ vòng tròn lệch xuống
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(wallCheck.position + Vector3.down * offset, 0.2f);
        }
         // Vẽ GroundCheck
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded())
        {
            isWallSliding = true;
            rb.velocity = new Vector2(0, -wallSlidingSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = isFacingRight ? -1f : 1f;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (isTouchingScreen && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            // Đảo hướng Player nếu cần
            isFacingRight = !isFacingRight;
            Flip();

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        // Đảo hướng Player
        Vector3 localScale = transform.localScale;
        localScale.x = isFacingRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }
}
