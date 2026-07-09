using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAbility : MonoBehaviour
{
    public BoxCollider2D bc;
    public Vector2 normalSize;
    public Vector2 normalOffset;
    public Vector2 crouchSize;
    public Vector2 crouchOffset;
    public PlayerController pc;
    public float normalSpeed;
    public float crouchSpeed;
    public bool isCrouched = false;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool isWallClimbing;
    public bool isTop;
    public float topRange = 0.5f;
    public bool isTopTwo;
    public float topRangeTwo = 0.5f;
    public float wallFallingSpeed = 0.3f;
    public float wallRange = 0.5f;
    public float normalGra;
    public float delay;
    public bool isWallJumping;
    public float wallJumpingDirection;
    public float wallJumpingTime = 0.6f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 0.6f;
    private Vector2 wallJumpingPower = new Vector2(14f, 16f);
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask notClimbLayer;
    [SerializeField] private Transform topCheck;
    [SerializeField] private Transform topCheckTwo;

    public SpriteRenderer sr;
    public Sprite normal;
    public Sprite crouch;

    // Start is called before the first frame update
    void Start()
    {
        normalSpeed = pc.speed;
        normalSize = bc.size;
        normalOffset = bc.offset;
        sr.sprite = normal;
        normalGra = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        isTop = Physics2D.OverlapCircle(topCheck.position, topRange, wallLayer);
        isTopTwo = Physics2D.OverlapCircle(topCheckTwo.position, topRangeTwo, wallLayer);
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouched = true;
            Crouch();
        }
        if (Input.GetButtonUp("Crouch") && !isTop && !isTopTwo)
        {
            isCrouched = false;
            Crouch();
        }
        if (isCrouched && !isTopTwo && !Input.GetButton("Crouch"))
        {
            isCrouched = false;
            Crouch();
        }
        onWall = Physics2D.OverlapCircle(wallCheck.position, wallRange, wallLayer) && !Physics2D.OverlapCircle(wallCheck.position, wallRange, notClimbLayer);
        onRightWall = Physics2D.OverlapCircle(wallCheck.position, wallRange, wallLayer) && pc.isFacingRight;
        onLeftWall = Physics2D.OverlapCircle(wallCheck.position, wallRange, wallLayer) && !pc.isFacingRight;
        WallClimb();
        WallJump();
        if (isWallClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallFallingSpeed);
            float speedModifier = pc.vertical > 0 ? 1f : 1;
            rb.velocity = new Vector2(rb.velocity.x, pc.vertical * (pc.speed * speedModifier));
        }
        if (pc.IsGrounded() && !isCrouched)
        {
            pc.speed = normalSpeed;
        }
    }

    public void Crouch()
    {
        if (isCrouched)
        {
            sr.sprite = crouch;
            bc.size = crouchSize;
            bc.offset = crouchOffset;
            pc.speed = crouchSpeed;
        }
        else
        {
            sr.sprite = normal;
            bc.size = normalSize;
            bc.offset = normalOffset;
            pc.speed = normalSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wallCheck.position, wallRange);
        Gizmos.DrawWireSphere(topCheck.position, topRange);
        Gizmos.DrawWireSphere(topCheckTwo.position, topRangeTwo);
    }
    
    private void WallClimb()
    {
        if (Input.GetButtonDown("Ability") && isWallClimbing)
        {
            if (pc.isFacingRight && onRightWall)
            {
                pc.isFacingRight = !pc.isFacingRight;
                transform.Rotate(0f, 180f, 0f);
            }
            else if (!pc.isFacingRight && onLeftWall)
            {
                pc.isFacingRight = !pc.isFacingRight;
                transform.Rotate(0f, 180f, 0f);
            }
            isWallClimbing = false;
            rb.gravityScale = normalGra;
            return;
        }
        if (onWall && !pc.IsGrounded() && pc.horizontal == 0f)
        {
            pc.speed = normalSpeed;
            isWallClimbing = true;
            pc.horizontal = 0;
            return;
        }
        else if (!onWall && isWallClimbing)
        {
            delay += Time.deltaTime;
            if (delay >= 0.2f)
            {
                rb.gravityScale = normalGra;
                delay = 0;
                isWallClimbing = false;
            }
            return;
        }
        
    }

    private void WallJump()
    {
        if (isWallClimbing)
        {
            isWallJumping = false;
            if (onRightWall)
            {
                wallJumpingDirection = -1f;
            }
            else
            {
                wallJumpingDirection = 1f;
            }
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallClimbing = false;
            pc.speed *= 1.5f;
            isWallJumping = true;
            Vector2 velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            rb.AddForce(velocity, ForceMode2D.Impulse);
            wallJumpingCounter = 0f;
            
            if (pc.isFacingRight && onRightWall)
            {
                pc.isFacingRight = !pc.isFacingRight;
                transform.Rotate(0f, 180f, 0f);
            }
            else if (!pc.isFacingRight && onLeftWall)
            {
                pc.isFacingRight = !pc.isFacingRight;
                transform.Rotate(0f, 180f, 0f);
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
            
            if (Input.GetAxisRaw("Horizontal") == -wallJumpingDirection || pc.IsGrounded())
            {
                isWallJumping = false;
            }
            
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    
}
