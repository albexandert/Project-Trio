using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public float speed = 10f;
    public float baseAttackDmg = 1;
    public Rigidbody2D rb;

    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.7f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.GetComponent<EnemyBehavior>() != null)
        {
            hitInfo.GetComponent<EnemyBehavior>().EnemyHit(baseAttackDmg);
        }

        if (hitInfo.gameObject.layer != LayerMask.NameToLayer("Heroes") && hitInfo.gameObject.name != "Bound")
        {
            Destroy(gameObject);
        }
    }
}
