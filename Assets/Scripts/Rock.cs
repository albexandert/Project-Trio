using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private float rockHP = 6;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite crackedSprite;
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void damage(float dmg)
    {
        rockHP -= dmg;
        sr.sprite = crackedSprite;
        if (rockHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
