using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprint : MonoBehaviour
{
    Vector2 direction = Vector2.zero;

    public Vector2 Direction
    {
        get
        {
            return direction;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Footprint")
        {
            Destroy(collision.gameObject);
        }
    }
}
