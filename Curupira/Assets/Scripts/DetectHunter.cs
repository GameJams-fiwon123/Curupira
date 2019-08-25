using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHunter : MonoBehaviour
{
    [SerializeField]
    private Jaguar refJaguar = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            refJaguar.EnableMove();
        }
    }
}
