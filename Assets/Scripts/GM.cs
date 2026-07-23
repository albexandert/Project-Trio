using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public bool isGameOver = false;
    public bool playerDied = false;
    public bool playerCleared = false;
    [SerializeField] private PlayerController pcSW;
    [SerializeField] private PlayerController pcM;
    [SerializeField] private PlayerController pcS;
    [SerializeField] private SwordAbility swA;
    [SerializeField] private MageAbility mA;
    //[SerializeField] private SlimeAbility sA;
    //[SerializeField] private PlayerSwitchingScript pss;
    [SerializeField] private CameraController cc;
    [SerializeField] private AudioSource music;
    [SerializeField] private MeshRenderer gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        gameOverText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCleared && !playerDied)
        {
            isGameOver = true;
            pcSW.enabled = false;
            pcM.enabled = false;
            pcS.enabled = false;
            swA.enabled = false;
            mA.enabled = false;
            //sA.enabled = false;
            //pss.enabled = false;
            cc.enabled = false;
        }
        if (isGameOver == true)
        {
            music.Stop();
            if (playerDied)
            {
                gameOverText.enabled = true;
                if (Input.GetButtonDown("Jump"))
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}
