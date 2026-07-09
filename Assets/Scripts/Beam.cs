using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;

    public EnemyBehavior eb;
    [SerializeField] private float timer;
    // Start is called before the first frame update
    void Start()
    { 
        if (!eb.isFacingRight)
        {
            rb.velocity = transform.up * speed;
        }
        else
        {
            rb.velocity = transform.up * -speed;
        }
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
        if (hitInfo.gameObject.layer != LayerMask.NameToLayer("Harmful") && hitInfo.gameObject.name != "Bound")
        {
            Destroy(gameObject);
        }
    }
}
