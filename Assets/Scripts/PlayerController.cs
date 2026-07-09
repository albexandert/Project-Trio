using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float speed;
    public float jumpingPower;
    public bool isFacingRight = true;
    public HealthScript hp;
    public SlimeAbility sa;
    public SwordAbility swa;
    public MageAbility ma;
    private SpriteRenderer sr;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!sa.isWallClimbing && !swa.isAttack && !ma.isMagic)
        {
            horizontal = Input.GetAxisRaw("Horizontal"); 
        }
        else if (IsGrounded())
        {
            horizontal = 0;
        }
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && IsGrounded() && !sa.isWallClimbing)
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (horizontal != 0)
        {
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().Play("SwordWalk");
            }
        }
        else
        {
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().Play("SwordIdle");
            }
        }
        Flip();
    }

    private void FixedUpdate()
    {
        if (!sa.isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(sa.wallJumpingDirection * speed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        SM.PlaySound(SoundType.JUMP);
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);

        }
    }

    public IEnumerator FlashRed(SpriteRenderer sprite)
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        { 
            hp.TakeDamage(collision.gameObject.GetComponent<EnemyDamage>().damage);
            StartCoroutine(FlashRed(sr));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            hp.TakeDamage(collision.gameObject.GetComponent<EnemyDamage>().damage);
            StartCoroutine(FlashRed(sr));
        }
    }
}
