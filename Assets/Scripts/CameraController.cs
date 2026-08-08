using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerSwitchingScript script;
    public GameObject swordHero;
    public GameObject mageHero;
    public GameObject slimeHero;
    public GM gm;

    void Awake()
    {
        gm = GameObject.FindWithTag("GM").GetComponent<GM>();
        script = GameObject.Find("Hero Switcher").GetComponent<PlayerSwitchingScript>();
        swordHero = GameObject.FindWithTag("SWH");
        mageHero = GameObject.FindWithTag("MH");
        slimeHero = GameObject.FindWithTag("SH");
    }
    // Update is called once per frame
    void Update()
    {
        if (!gm.isGameOver)
        {
            if (script.swordActive)
            {
                transform.position = swordHero.transform.position;
            }
            else if (script.mageActive)
            {
                transform.position = mageHero.transform.position;
            }
            else if (script.slimeActive)
            {
                transform.position = slimeHero.transform.position;
            }
        }
        
    }
}
