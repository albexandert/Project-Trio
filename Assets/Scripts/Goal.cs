using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] private GM gm;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LevelEnd()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Heroes"))
        {
            gm.playerCleared = true;
            while(true)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1f * collision.gameObject.GetComponent<PlayerController>().speed, collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
        }
        StartCoroutine(LevelEnd());
    }
}
