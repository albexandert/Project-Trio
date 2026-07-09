using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlatformScript : MonoBehaviour
{
    public BoxCollider2D bc;
    public SpriteRenderer sr;
    public Color color;
    public Color nColor;
    public bool isSolid = false;
    // Start is called before the first frame update
    void Start()
    {
        color = sr.color;
        nColor = new Color(sr.color.r, sr.color.g, sr.color.b, 255f);
        if (isSolid)
        {
            gameObject.layer = 6;
            sr.color = nColor;
            bc.isTrigger = false;
        }
        else
        {
            gameObject.layer = 0;
            sr.color = color;
            bc.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Magic" && bc.isTrigger)
        {
            gameObject.layer = 6;
            sr.color = nColor;
            bc.isTrigger = false;
            return;
        }
        if (collision.gameObject.tag == "Magic" && !bc.isTrigger)
        {
            gameObject.layer = 0;
            sr.color = color;
            bc.isTrigger = true;
            return;
        }
    }
}
