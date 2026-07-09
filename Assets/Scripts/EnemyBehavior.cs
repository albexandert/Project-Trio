using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    public Transform currentpoint;
    [SerializeField] private float attackRange = 2f;
    public Transform attackPoint;
    [SerializeField] private float speed;
    [SerializeField] private float enemyHP;
    public Transform firePoint;
    public GameObject beamPrefab;
    private float delay;
    private bool delayActive = false;
    public bool isFacingRight = false;
    [SerializeField] private LayerMask HeroLayer;
    private GameObject beam;
    private SpriteRenderer sr;
    private Color norColor;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        norColor = sr.color;
        if (currentpoint == pointB.transform)
        {
            transform.Rotate(0f, 180f, 0f);
        }
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 point = currentpoint.position - transform.position;
        if (currentpoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if(Vector2.Distance(transform.position, currentpoint.position) < 0.5f && currentpoint == pointB.transform)
        {
            currentpoint = pointA.transform;
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = false;
        }
        if (Vector2.Distance(transform.position, currentpoint.position) < 0.5f && currentpoint == pointA.transform)
        {
            currentpoint = pointB.transform;
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = true;
        }
        Collider2D[] Heroes = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, HeroLayer);
        foreach (Collider2D thing in Heroes)
        {
            if (!delayActive)
            {
                enemyShoot();
                delayActive = true;
            }
            else
            {
                delay += Time.deltaTime;
                if (delay >= 3f)
                {
                    delay = 0;
                    delayActive = false;
                }
            }
        }
    }

    public IEnumerator FlashRed(SpriteRenderer sprite)
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = norColor;
    }
    public void EnemyHit(float dmg)
    {
        enemyHP -= dmg;
        StartCoroutine(FlashRed(sr));
        if (enemyHP <= 0)
        {
            Debug.Log("Enemy is dead!");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Enemy took " + dmg + " damage / Enemy has " + enemyHP + " HP remaining.");
        }
    }

    public void enemyShoot()
    {
        beam = Instantiate(beamPrefab, firePoint.position, beamPrefab.transform.rotation);
        beam.GetComponent<Beam>().eb = GetComponent<EnemyBehavior>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
