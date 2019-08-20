using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed = 100;

    private Rigidbody2D rb;
    private List<Vector2> motions = new List<Vector2>();

    private Vector3 savePosition;
    private Vector3 newPosition = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        savePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        Move();
    }

    private void Inputs()
    {
        Vector2 motion = Vector2.zero;

        if (Input.GetButtonDown("left"))
        {
            motion.x = -1;
        }
        else if (Input.GetButtonDown("right"))
        {
            motion.x = 1;
        }
        else if (Input.GetButtonDown("up"))
        {
            motion.y = 1;
        }
        else if (Input.GetButtonDown("down"))
        {
            motion.y = -1;
        }

        if (motion.x != 0)
        {
            motions.Add(motion);
        }
        else if (motion.y != 0)
        {
            motions.Add(motion);
        }


    }

    void Move()
    {
        GetNextPosition();
        ProcessMove();
    }

    void GetNextPosition()
    {
        if (motions.Count > 0 && newPosition == Vector3.zero)
        {
            savePosition = transform.position;

            newPosition = transform.position;
            newPosition.x += motions[0].x;
            newPosition.y += motions[0].y;
        }
    }

    void ProcessMove()
    {
        if (newPosition != Vector3.zero)
        {
            Vector3 motion = Vector3.zero;
            motion.x = motions[0].x / 10;
            motion.y = motions[0].y / 10;

            transform.position = transform.position + motion;
            //rb.velocity = motions[0].normalized * Time.deltaTime * speed;
            //float distance = Vector2.Distance(transform.position, newPosition);

            //print(transform.position + "  " + newPosition);
            if (transform.position == newPosition)
            {
                motions.RemoveAt(0);
                newPosition = Vector3.zero;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            transform.position = savePosition;
            motions.RemoveAt(0);
            newPosition = Vector3.zero;
        }
        else
        {
            GameManager.instance.GameOver();
        }
    }

    IEnumerator Walk(Vector3 newPosition)
    {
        

        while (transform.position != newPosition)
            yield return new WaitForSeconds(0);

        rb.velocity = Vector2.zero;
    }
}
