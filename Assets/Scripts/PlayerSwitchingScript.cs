using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchingScript : MonoBehaviour
{
    public GameObject swordHero;
    public GameObject mageHero;
    public GameObject slimeHero;
    public SlimeAbility sa;
    public SwordAbility swa;
    public MageAbility ma;
    public bool swordActive;
    public bool mageActive;
    public bool slimeActive;
    public float delay;
    public bool delayActive = false;
    // Start is called before the first frame update
    void Start()
    {
        swordHero = GameObject.FindGameObjectWithTag("SWH");
        mageHero = GameObject.FindGameObjectWithTag("MH");
        slimeHero = GameObject.FindGameObjectWithTag("SH");
        swa = swordHero.GetComponent<SwordAbility>();
        ma = mageHero.GetComponent<MageAbility>();
        sa = slimeHero.GetComponent<SlimeAbility>();
        mageHero.SetActive(false);
        slimeHero.SetActive(false);
        swordActive = true;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (!delayActive)
        {
            if ((!sa.isTop && !sa.isTopTwo) && !swa.isAttack && !ma.isMagic)
            {
                if (Input.GetButtonDown("Switch"))
                {
                    SwitchHero();
                }
            }
        }
        else
        {
            delay += Time.deltaTime;
            if (delay >= 1)
            {
                delay = 0;
                delayActive = false;
            }
        }
    }

    public void SwitchHero()
    {
        if(swordActive)
        {
            swordHero.GetComponent<PlayerController>().isJumping = false;
            swordHero.GetComponent<PlayerController>().isWallJumping = false;
            swordHero.GetComponent<PlayerController>().isJumpCut = false;
            swordHero.GetComponent<PlayerController>().isJumpFalling = false;
            swordHero.GetComponent<PlayerController>().lastPressedJumpTime = 0;
            swordHero.GetComponent<PlayerController>().lastOnGroundTime = 0;
            swordHero.GetComponent<PlayerController>().lastOnWallRightTime = 0;
            swordHero.GetComponent<PlayerController>().lastOnWallLeftTime = 0;
            swordHero.GetComponent<PlayerController>().isIF = false;
            swordHero.GetComponent<PlayerController>().sr.color = new Color(Color.white.r, Color.white.g, Color.white.g, 1);
            swordHero.SetActive(false);
            mageHero.SetActive(true);
            mageHero.transform.position = swordHero.transform.position;
            if (swordHero.GetComponent<PlayerController>().isFacingRight != mageHero.GetComponent<PlayerController>().isFacingRight)
            {
                mageHero.GetComponent<PlayerController>().Turn();
            }
            if (swordHero.GetComponent<PlayerController>().isSliding)
            {
                mageHero.GetComponent<PlayerController>().isSliding = true;
            }
            swordHero.GetComponent<PlayerController>().isSliding = false;
            swordActive = false;
            mageActive = true;
        }
        else if(mageActive)
        {
            mageHero.GetComponent<PlayerController>().isJumping = false;
            mageHero.GetComponent<PlayerController>().isWallJumping = false;
            mageHero.GetComponent<PlayerController>().isJumpCut = false;
            mageHero.GetComponent<PlayerController>().isJumpFalling = false;
            mageHero.GetComponent<PlayerController>().lastPressedJumpTime = 0;
            mageHero.GetComponent<PlayerController>().lastOnGroundTime = 0;
            mageHero.GetComponent<PlayerController>().lastOnWallRightTime = 0;
            mageHero.GetComponent<PlayerController>().lastOnWallLeftTime = 0;
            mageHero.GetComponent<PlayerController>().isIF = false;
            mageHero.GetComponent<PlayerController>().sr.color = new Color(Color.white.r, Color.white.g, Color.white.g, 1);
            mageHero.SetActive(false);
            slimeHero.SetActive(true);
            slimeHero.transform.position = mageHero.transform.position;
            if (slimeHero.GetComponent<PlayerController>().isFacingRight != mageHero.GetComponent<PlayerController>().isFacingRight)
            {
                slimeHero.GetComponent<PlayerController>().Turn();
            }
            if (mageHero.GetComponent<PlayerController>().isSliding)
            {
                slimeHero.GetComponent<PlayerController>().isSliding = true;
            }
            mageHero.GetComponent<PlayerController>().isSliding = false;
            mageActive = false;
            slimeActive = true;
        }
        else
        {
            slimeHero.GetComponent<PlayerController>().isJumping = false;
            slimeHero.GetComponent<PlayerController>().isWallJumping = false;
            slimeHero.GetComponent<PlayerController>().isJumpCut = false;
            slimeHero.GetComponent<PlayerController>().isJumpFalling = false;
            slimeHero.GetComponent<PlayerController>().lastPressedJumpTime = 0;
            slimeHero.GetComponent<PlayerController>().lastOnGroundTime = 0;
            slimeHero.GetComponent<PlayerController>().lastOnWallRightTime = 0;
            slimeHero.GetComponent<PlayerController>().lastOnWallLeftTime = 0;
            slimeHero.GetComponent<PlayerController>().isIF = false;
            slimeHero.GetComponent<PlayerController>().sr.color = new Color(Color.white.r, Color.white.g, Color.white.g, 1);
            if (sa.isCrouched)
            {
                sa.isCrouched = false;
                sa.Crouch();
            }
            slimeHero.SetActive(false);
            swordHero.SetActive(true);
            swordHero.transform.position = slimeHero.transform.position;
            if (slimeHero.GetComponent<PlayerController>().isFacingRight != swordHero.GetComponent<PlayerController>().isFacingRight)
            {
                swordHero.GetComponent<PlayerController>().Turn();
            }
            if (slimeHero.GetComponent<PlayerController>().isSliding)
            {
                swordHero.GetComponent<PlayerController>().isSliding = true;
            }
            slimeHero.GetComponent<PlayerController>().isSliding = false;
            slimeActive = false;
            swordActive = true;
        }
        delayActive = true;
    }

}


