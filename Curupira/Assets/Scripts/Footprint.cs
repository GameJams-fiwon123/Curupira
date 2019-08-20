using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprint : MonoBehaviour
{
    Vector3 nextPosition = Vector3.zero;

    public Vector3 NextPosition
    {
        get
        {
            return nextPosition;
        }

        set
        {
            nextPosition = value;
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
