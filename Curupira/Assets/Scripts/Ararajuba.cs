using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ararajuba : MonoBehaviour
{
    private Animator anim = null;
    private Collider2D col2D = null;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            anim.SetBool("IsDie", true);
            col2D.enabled = false;
            Invoke("Die", 0.5f);
        }
    }

    private void Die()
    {
        GameManager.instance.GameOver();
    }
}
