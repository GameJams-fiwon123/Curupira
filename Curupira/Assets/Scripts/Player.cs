using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed = 100;

    private Rigidbody2D rb;
    private Vector2 motion = new Vector2();

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        motion.x = Input.GetAxisRaw("Horizontal");
        motion.y = Input.GetAxisRaw("Vertical");

        rb.velocity = motion.normalized * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.GameOver();
        print("oi");
    }
}
