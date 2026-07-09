using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBar hb;
    public GM gm;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        hb.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        hb.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            gm.isGameOver = true;
            gm.playerDied = true;
            SM.PlaySound(SoundType.DEATH);
            Destroy(gameObject);
        }
    }
}
