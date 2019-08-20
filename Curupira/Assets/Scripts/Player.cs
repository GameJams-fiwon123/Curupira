using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject footprintPrefab = null;
    private bool canPutFootprint = true; 

    private Rigidbody2D rb = null;
    private List<Vector2> motions = new List<Vector2>();

    private Vector3 savePosition;
    private Vector3 newPosition = Vector3.zero;

    private Vector2 lastMotion = Vector3.zero;

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
        PutFootprint();
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

            if (transform.position == newPosition)
            {
                motions.RemoveAt(0);
                newPosition = Vector3.zero;
                canPutFootprint = true;
            }
        }
    }

    private void PutFootprint()
    {
        if (canPutFootprint)
        {
            if (newPosition != Vector3.zero)
            {
                Vector3 direction = newPosition - transform.position;
                direction = direction.normalized;

                GameObject instanceFootprint = Instantiate(footprintPrefab, transform.position, Quaternion.identity);
                instanceFootprint.GetComponent<Footprint>().NextPosition = transform.position - direction;

                if (direction.x != lastMotion.x || direction.y != lastMotion.y)
                {
                    Vector3 aux = direction;
                    direction = lastMotion;
                    lastMotion = aux;
                } else
                {
                    lastMotion = direction;
                }

                if (direction.x > 0)
                {
                    instanceFootprint.transform.eulerAngles = Vector3.forward * -90;
                }
                else if (direction.x < 0)
                {
                    instanceFootprint.transform.eulerAngles = Vector3.forward * 90;
                }
                else if (direction.y > 0)
                {
                    instanceFootprint.transform.rotation.SetLookRotation(Vector3.up);
                }
                else if (direction.y < 0)
                {
                    instanceFootprint.transform.eulerAngles = Vector3.forward * 180;
                }

                canPutFootprint = false;
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
}
