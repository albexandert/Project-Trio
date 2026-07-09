using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAbility : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float baseAttackDmg = 3;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public LayerMask breakablesLayer;
    public LayerMask harmfulLayer;
    public bool isAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            isAttack = false;
            if (Input.GetButtonDown("Ability"))
            {
                isAttack = true;
                Debug.Log("Slash!");
                Slash();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Slash()
    {
        SM.PlaySound(SoundType.SLASH);
        Collider2D[] slashedBreak = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, breakablesLayer);
        Collider2D[] slashedEnemy = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, harmfulLayer);
        foreach (Collider2D thing in slashedBreak)
        {
            if (thing.GetComponent<Rock>() == null)
            {
                Destroy(thing.gameObject);
            }
            else
            {
                thing.GetComponent<Rock>().damage(baseAttackDmg);
            }
            
        }
        foreach (Collider2D thing in slashedEnemy)
        {
            if (thing.GetComponent<EnemyBehavior>() != null)
            {
                thing.GetComponent<EnemyBehavior>().EnemyHit(baseAttackDmg);
            }
            else
            {
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
