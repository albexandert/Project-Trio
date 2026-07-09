using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAbility : MonoBehaviour
{
    public Transform firePoint;
    public GameObject magicPrefab;
    public float magicRate = 2f;
    float nextMagicTime = 0f;
    public bool isMagic = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextMagicTime)
        {
            isMagic = false;
            if (Input.GetButtonDown("Ability"))
            {
                isMagic = true;
                Shoot();
                nextMagicTime = Time.time + 1f / magicRate;
            }
        }
    }

    void Shoot()
    {
        Instantiate(magicPrefab, firePoint.position, firePoint.rotation);
    }
}
