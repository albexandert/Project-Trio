using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAbility : MonoBehaviour
{
    public GameObject normalSize;
    public GameObject crouchSize;
    public PlayerController pc;
    public float normalSpeed;
    public float crouchSpeed;
    public float slideSpeedUp;
    public float slideSpeedDown;
    public bool isCrouched = false;
    public bool isTop;
    public float topRange = 0.5f;
    public bool isTopTwo;
    public float topRangeTwo = 0.5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform topCheck;
    [SerializeField] private Transform topCheckTwo;

    [SerializeField] private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        normalSpeed = 4;
        normalSize = transform.Find("Ncolliders").gameObject;
        crouchSize = transform.Find("Scolliders").gameObject;
        crouchSize.SetActive(false);

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pc.isSliding && pc.lastOnGroundTime > 0)
        {
            isTop = Physics2D.OverlapCircle(topCheck.position, topRange, wallLayer);
            isTopTwo = Physics2D.OverlapCircle(topCheckTwo.position, topRangeTwo, wallLayer);
            if (Input.GetButtonDown("Crouch"))
            {
                isCrouched = true;
                Crouch();
            }
            if (Input.GetButton("Crouch") && pc.lastOnGroundTime == pc.data.coyoteTime)
            {
                isCrouched = true;
                Crouch();
            }
            else if(Input.GetButton("Crouch") && pc.lastOnGroundTime > 0)
            {
                isCrouched = false;
                Crouch();
            }
            if (Input.GetButtonUp("Crouch") && !isTop && !isTopTwo)
            {
                isCrouched = false;
                Crouch();
            }
            if (isCrouched && !isTopTwo && !isTop && !Input.GetButton("Crouch"))
            {
                isCrouched = false;
                Crouch();
            }
            if (pc.lastOnGroundTime <= 0 && !isCrouched)
            {
                pc.data.runMaxSpeed = normalSpeed;
            }
        }
        if (pc.isSliding)
        {
            WallClimb();
        }

        
        if (pc.lastOnGroundTime >= 0)
        {
            pc.groundCheckSize.x = 1f;
        }
        else
        {
            pc.groundCheckSize.x = 0.4f;
        }
    }

    public void Crouch()
    {
        if (isCrouched && pc.lastOnGroundTime > 0)
        {
            animator.SetBool("isCrouched", true);
            crouchSize.SetActive(true);
            normalSize.SetActive(false);
            pc.data.runMaxSpeed = crouchSpeed;
        }
        else
        {
            animator.SetBool("isCrouched", false);
            crouchSize.SetActive(false);
            normalSize.SetActive(true);
            pc.data.runMaxSpeed = normalSpeed;
        }
    }
    private void WallClimb()
    {
        if (pc.moveInput.y == 1)
        {
            pc.data.slideSpeed = slideSpeedUp;
        }
        else if (pc.moveInput.y == -1)
        {
            pc.data.slideSpeed = slideSpeedDown;
        }
        else
        {
            pc.data.slideSpeed = 0;
        }
    }  

    private void OnDrawGizmos()
    { 
        Gizmos.DrawWireSphere(topCheck.position, topRange);
        Gizmos.DrawWireSphere(topCheckTwo.position, topRangeTwo);
    }

    
}
