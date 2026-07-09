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
    public bool swordActive = true;
    public bool mageActive = false;
    public float delay;
    public bool delayActive = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!delayActive)
        {
            if (!sa.isWallClimbing && (!sa.isTop && !sa.isTopTwo) && !swa.isAttack && !ma.isMagic)
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
            swordHero.SetActive(false);
            mageHero.transform.position = swordHero.transform.position;
            if (swordHero.GetComponent<PlayerController>().isFacingRight != mageHero.GetComponent<PlayerController>().isFacingRight)
            {
                mageHero.transform.Rotate(0f, 180f, 0f);
            }
            mageHero.GetComponent<PlayerController>().isFacingRight = swordHero.GetComponent<PlayerController>().isFacingRight;
            mageHero.SetActive(true);
            swordActive = false;
            mageActive = true;
        }
        else if(mageActive)
        {
            mageHero.SetActive(false);
            slimeHero.transform.position = mageHero.transform.position;
            if (mageHero.GetComponent<PlayerController>().isFacingRight != slimeHero.GetComponent<PlayerController>().isFacingRight)
            {
                slimeHero.transform.Rotate(0f, 180f, 0f);
            }
            slimeHero.GetComponent<PlayerController>().isFacingRight = mageHero.GetComponent<PlayerController>().isFacingRight;
            slimeHero.SetActive(true);
            mageActive = false;
        }
        else
        {
            if (sa.isCrouched)
            {
                sa.sr.sprite = sa.normal;
                sa.bc.size = sa.normalSize;
                sa.bc.offset = sa.normalOffset;
                sa.pc.speed = sa.normalSpeed;
            }
            slimeHero.SetActive(false);
            swordHero.transform.position = slimeHero.transform.position;
            if (slimeHero.GetComponent<PlayerController>().isFacingRight != swordHero.GetComponent<PlayerController>().isFacingRight)
            {
                swordHero.transform.Rotate(0f, 180f, 0f);
            }
            swordHero.GetComponent<PlayerController>().isFacingRight = slimeHero.GetComponent<PlayerController>().isFacingRight;
            swordHero.SetActive(true);
            swordActive = true;
        }
        delayActive = true;
    }
}
