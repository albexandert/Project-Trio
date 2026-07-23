using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData data;
    public HealthScript hp;
    public Vector2 moveInput;
    public bool isFacingRight;
    public bool isJumping;
    public bool isWallJumping;
    public bool isSliding;

    public float lastOnGroundTime;
    public float lastOnWallTime;
    public float lastOnWallRightTime { get; private set; }
    public float lastOnWallLeftTime { get; private set; }
    private bool isJumpCut;
    private bool isJumpFalling;
    private float wallJumpStartTime;
    private int lastWallJumpDir;
    public float lastPressedJumpTime { get; private set; }
    //public SlimeAbility sa;
    public SwordAbility swa;
    public MageAbility ma;
    public  SpriteRenderer sr;

    public Rigidbody2D rb { get; private set; }
    //public PlayerAnimator AnimHandler { get; private set; }
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private Transform frontWallCheckPoint;
    [SerializeField] private Transform backWallCheckPoint;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private LayerMask groundLayer;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        rb.gravityScale = data.gravityScale;
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        lastOnGroundTime -= Time.deltaTime;
        lastOnWallTime -= Time.deltaTime;
        lastOnWallRightTime -= Time.deltaTime;
        lastOnWallLeftTime -= Time.deltaTime;
        lastPressedJumpTime -= Time.deltaTime;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.x != 0)
            CheckDirectionToFace(moveInput.x > 0);

        if (Input.GetButtonDown("Jump"))
        {
            OnJumpInput();
        }

        if (Input.GetButtonUp("Jump"))
        {
            OnJumpUpInput();
        }

        if (!isJumping)
        {
            //Ground Check
            if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer)) //checks if set box overlaps with ground
            {
                lastOnGroundTime = data.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }

            //Right Wall Check
            if (((Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && isFacingRight)
                        || (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !isFacingRight)) && !isWallJumping)
                lastOnWallRightTime = data.coyoteTime;

            //Left Wall Check
            if (((Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !isFacingRight)
                        || (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer) && isFacingRight)) && !isWallJumping)
                lastOnWallLeftTime = data.coyoteTime;

            //Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
            lastOnWallTime = Mathf.Max(lastOnWallLeftTime, lastOnWallRightTime);
        }

        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;

            if (!isWallJumping)
                isJumpFalling = true;
        }

        if (isWallJumping && Time.time - wallJumpStartTime > data.wallJumpTime)
        {
            isWallJumping = false;
        }

        if (lastOnGroundTime > 0 && !isJumping && !isWallJumping)
        {
            isJumpCut = false;

            if (!isJumping)
                isJumpFalling = false;
        }

        //Jump
        if (CanJump() && lastPressedJumpTime > 0)
        {
            isJumping = true;
            isWallJumping = false;
            isJumpCut = false;
            isJumpFalling = false;
            Jump();
        }
        //WALL JUMP
        else if (CanWallJump() && lastPressedJumpTime > 0)
        {
            isWallJumping = true;
            isJumping = false;
            isJumpCut = false;
            isJumpFalling = false;
            wallJumpStartTime = Time.time;
            lastWallJumpDir = (lastOnWallRightTime > 0) ? -1 : 1;

            WallJump(lastWallJumpDir);
        }

        if (CanSlide() && ((lastOnWallLeftTime > 0 && moveInput.x < 0) || (lastOnWallRightTime > 0 && moveInput.x > 0)))
            isSliding = true;
        else
            isSliding = false;

        //Higher gravity if we've released the jump input or are falling
        if (isSliding)
        {
            SetGravityScale(0);
        }
        else if (rb.velocity.y < 0 && moveInput.y < 0)
        {
            //Much higher gravity if holding down
            SetGravityScale(data.gravityScale * data.fastFallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFastFallSpeed));
        }
        else if (isJumpCut)
        {
            //Higher gravity if jump button released
            SetGravityScale(data.gravityScale * data.jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
        }
        else if ((isJumping || isWallJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < data.jumpHangTimeThreshold)
        {
            SetGravityScale(data.gravityScale * data.jumpHangGravityMult);
        }
        else if (rb.velocity.y < 0)
        {
            //Higher gravity if falling
            SetGravityScale(data.gravityScale * data.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
        }
        else
        {
            //Default gravity if standing on a platform or moving upwards
            SetGravityScale(data.gravityScale);
        }

    }

    private void FixedUpdate()
    {
        //Handle Run
        if (isWallJumping)
            Run(data.wallJumpRunLerp);
        else
            Run(1);

        //Handle Slide
        if (isSliding)
            Slide();
    }

    private void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
            Turn();
    }

    public void OnJumpInput()
    {
        lastPressedJumpTime = data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            isJumpCut = true;
    }

    public void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    private void Run(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = moveInput.x * data.runMaxSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (lastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;

        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((isJumping || isWallJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < data.jumpHangTimeThreshold)
        {
            accelRate *= data.jumpHangAccelerationMult;
            targetSpeed *= data.jumpHangMaxSpeedMult;
        }

        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (data.doConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && lastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rb.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        lastPressedJumpTime = 0;
        lastOnGroundTime = 0;
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        float force = data.jumpForce;
        if (rb.velocity.y < 0)
            force -= rb.velocity.y;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void WallJump(int dir)
    {
        //Ensures we can't call Wall Jump multiple times from one press
        lastPressedJumpTime = 0;
        lastOnGroundTime = 0;
        lastOnWallRightTime = 0;
        lastOnWallLeftTime = 0;

        Vector2 force = new Vector2(data.wallJumpForce.x, data.wallJumpForce.y);
        force.x *= dir; //apply force in opposite direction of wall

        if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
            force.x -= rb.velocity.x;

        if (rb.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= rb.velocity.y;

        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Slide()
    {
        //We remove the remaining upwards Impulse to prevent upwards sliding
        if (rb.velocity.y > 0)
        {
            rb.AddForce(-rb.velocity.y * Vector2.up, ForceMode2D.Impulse);
        }
        //Works the same as the Run but only in the y-axis
        float speedDif = data.slideSpeed - rb.velocity.y;
        float movement = speedDif * data.slideAccel;
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called.
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        rb.AddForce(movement * Vector2.up, ForceMode2D.Force);
    }
    private bool CanJump()
    {
        return lastOnGroundTime > 0 && !isJumping;
    }

    private bool CanWallJump()
    {
        return lastPressedJumpTime > 0 && lastOnWallTime > 0 && lastOnGroundTime <= 0 && (!isWallJumping ||
            (lastOnWallRightTime > 0 && lastWallJumpDir == 1) || (lastOnWallLeftTime > 0 && lastWallJumpDir == -1));
    }

    private bool CanJumpCut()
    {
        return isJumping && rb.velocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return isWallJumping && rb.velocity.y > 0;
    }

    public bool CanSlide()
    {
        if (lastOnWallTime > 0 && !isJumping && !isWallJumping && lastOnGroundTime <= 0)
            return true;
        else
            return false;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(frontWallCheckPoint.position, wallCheckSize);
        Gizmos.DrawWireCube(backWallCheckPoint.position, wallCheckSize);
    }
}