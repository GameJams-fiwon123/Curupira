using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject footprintPrefab = null;
    private bool canPutFootprint = true;
    private bool canDestroyFootprint = false;
    private GameObject lastFootprint;

    private Rigidbody2D rb = null;
    private Animator anim = null;
    private SpriteRenderer spr = null;
    private List<Vector2> motions = new List<Vector2>();

    private Vector3 savePosition;
    private Vector3 newPosition = Vector3.zero;

    private Vector3 lastMotion = Vector3.down;
    private Vector3 aux;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
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
        anim.SetInteger("Horizontal", 0);
        anim.SetInteger("Vertical", 0);

        if (Input.GetButtonDown("left"))
        {
            spr.flipX = true;
            anim.SetInteger("Horizontal", -1);
            motion.x = -1;
        }
        else if (Input.GetButtonDown("right"))
        {
            spr.flipX = false;
            anim.SetInteger("Horizontal", 1);
            motion.x = 1;
        }
        else if (Input.GetButtonDown("up"))
        {
            anim.SetInteger("Vertical", 1);
            motion.y = 1;
        }
        else if (Input.GetButtonDown("down"))
        {
            anim.SetInteger("Vertical", -1);
            motion.y = -1;
        }

        if (motion.x != 0 || motion.y != 0) 
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

            if (lastMotion.x == motions[0].x && lastMotion.y == motions[0].y)
            {
                savePosition = transform.position;

                newPosition = transform.position;
                newPosition.x += motions[0].x;
                newPosition.y += motions[0].y;
                lastMotion = motions[0];
            }
            else
            {
                print(motions.Count );
                lastMotion = motions[0];
                motions.RemoveAt(0);
            }

            
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
        if(Input.GetKeyDown(KeyCode.X))
       // if (canPutFootprint)
        {
            //if (newPosition != Vector3.zero)
           // {
                Vector3 direction = lastMotion+transform.position - transform.position;
                direction = direction.normalized;

                GameObject instanceFootprint = Instantiate(footprintPrefab, transform.position, Quaternion.identity);

                //if ((direction.x != lastMotion.x || direction.y != lastMotion.y) && lastMotion != Vector2.zero)
                //{
                //    aux = direction;
                //    direction = lastMotion;
                //    lastMotion = aux;
                //}
                //else
                //{
                //    //aux = lastMotion;
                //    lastMotion = direction;
                //}

                instanceFootprint.GetComponent<Footprint>().NextPosition = transform.position - direction;

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
          //  }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            transform.position = savePosition;
            motions.RemoveAt(0);
            newPosition = Vector3.zero;
            canPutFootprint = true;
           // lastMotion = aux;
        }
        else
        {
            GameManager.instance.GameOver();
        }
    }
}
