using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField]
    int countEnemies = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            countEnemies--;

            if (countEnemies <= 0)
            {
                print("Voce Venceu");
            }
        }
    }
}
