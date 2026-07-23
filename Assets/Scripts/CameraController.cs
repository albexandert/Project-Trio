using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject script;
    public GameObject swordHero;
    public GameObject mageHero;
    public GameObject slimeHero;
    public GM gm;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if (!gm.isGameOver)
        {
            if (script.GetComponent<PlayerSwitchingScript>().swordActive)
            {
                transform.position = swordHero.transform.position;
            }
            else if (script.GetComponent<PlayerSwitchingScript>().mageActive)
            {
                transform.position = mageHero.transform.position;
            }
            /*
            else
            {
                transform.position = slimeHero.transform.position;
            }
            */
        }
        
    }
}
