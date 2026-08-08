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

    public PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
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

        if (pc.isFacingRight && !pc.isSliding || pc.isSliding && pc.lastOnWallLeftTime > 0)
        {
            firePoint.rotation = Quaternion.Euler(transform.localRotation.x, 0, transform.localRotation.z);
        }
        else if (!pc.isFacingRight && !pc.isSliding || pc.isSliding && pc.lastOnWallRightTime > 0)
        {
            firePoint.rotation = Quaternion.Euler(transform.localRotation.x, 180, transform.localRotation.z);
        }

        if (pc.isSliding && pc.lastOnWallTime > 0)
        {
            firePoint.localPosition = new Vector3(-0.25f, -0.14f, 0);
        }
        else 
        {
            firePoint.localPosition = new Vector3(0.25f, -0.14f, 0);
        }
    }

    void Shoot()
    {
        Instantiate(magicPrefab, firePoint.position, firePoint.rotation);
    }
}
