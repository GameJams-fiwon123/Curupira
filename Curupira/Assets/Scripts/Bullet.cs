using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 100f;

    Rigidbody2D rb2D = null;

    Vector3 direction = Vector3.zero;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb2D.velocity = direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                transform.eulerAngles = Vector3.forward;
            }
            else
            {
                transform.eulerAngles = Vector3.forward * 180;
            }

        }
        else
        {
            if (direction.y > 0)
            {
                transform.eulerAngles = Vector3.forward * -90;
            }
            else
            {
                transform.eulerAngles = Vector3.forward * 90;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
