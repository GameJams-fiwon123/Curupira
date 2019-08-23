using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [SerializeField] Transform follow = null;

    [SerializeField] GameObject prefabBullet = null;

    private Rigidbody2D rb2D;
    private Animator anim;
    private SpriteRenderer spr;
    private Vector3 motion = Vector3.zero;

    [Header("Settings Path")]
    [SerializeField] Transform paths = null;

    int index = -1;
    private Vector3 currentPathPosition;

    public float floatHeight;     // Desired floating height.
    public float liftForce;       // Force to apply when lifting the rigidbody.
    public float damping;         // Force reduction proportional to speed (reduces bouncing).

    Coroutine firing = null;


    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        SearchNextPath();
    }

    void SearchNextPath()
    {
        index++;
        if (index == paths.childCount)
        {
            index = 0;
        }

        currentPathPosition = paths.GetChild(index).position;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        See();
    }

    void Move()
    {
        if (!follow)
        {
            motion = currentPathPosition - transform.position;
            motion = motion.normalized;

            

            motion.x = motion.x / 50;
            motion.y = motion.y / 50;

            transform.position = transform.position + motion;
            float distance = Vector3.Distance(transform.position, currentPathPosition);

            if (distance < 0.1f)
            {
                SearchNextPath();
            }
        }
        else
        {
            motion = follow.position - transform.position;
            motion = motion.normalized;
            motion.x = motion.x / 35;
            motion.y = motion.y / 35;

            transform.position = transform.position + motion;
        }

        anim.SetInteger("Horizontal", 0);
        anim.SetInteger("Vertical", 0);

        if (Mathf.Abs(motion.x) > Mathf.Abs(motion.y))
        {
            if (motion.x > 0)
            {
                spr.flipX = false;
                anim.SetInteger("Horizontal", 1);
            }
            else if (motion.x < 0)
            {
                spr.flipX = true;
                anim.SetInteger("Horizontal", -1);
            }
        }
        else
        {
            if (motion.y > 0)
            {
                anim.SetInteger("Vertical", 1);
            }
            else if (motion.y < 0)
            {
                anim.SetInteger("Vertical", -1);
            }
        }


    }

    void See()
    {
        // Bit shift the index of the layer (10) and layer (11) to get a bit mask
        int layerMask = (1 << 10) | (1 << 11);

        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, motion, 1000, layerMask, 0, 0);

        // If it hits something...
        if (hit.collider != null)
        {
            // Calculate the distance from the surface and the "error" relative
            // to the floating height.
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            Debug.DrawRay(transform.position, motion * hit.distance, Color.yellow);

            if (hit.collider.name == "Player")
            {
                follow = hit.collider.transform;
                if (firing == null)
                {
                    firing = StartCoroutine(StartFire());
                }
                
            }
            else
            {
                if (firing != null)
                {
                    StopCoroutine(firing);
                    firing = null;
                }
                follow = null;
            }
        }
    }

    IEnumerator StartFire()
    {
        while (true)
        {
            GameObject instance = Instantiate(prefabBullet, transform.position, Quaternion.identity);
            if (Mathf.Abs(motion.x) > Mathf.Abs(motion.y))
            {
                if (motion.x > 0)
                {
                    instance.GetComponent<Bullet>().SetDirection(Vector3.right);
                }
                else
                {
                    instance.GetComponent<Bullet>().SetDirection(Vector3.left);
                }
                
            }
            else
            {
                if (motion.y > 0)
                {
                    instance.GetComponent<Bullet>().SetDirection(Vector3.up);
                }
                else
                {
                    instance.GetComponent<Bullet>().SetDirection(Vector3.down);
                }
            }

            

            yield return new WaitForSeconds(1f);
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Footprint")
        {
            currentPathPosition = collision.GetComponent<Footprint>().NextPosition;
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Pathing")
        {
            if (paths.gameObject != collision.gameObject)
            {
                paths = collision.gameObject.transform;
                index = -1;
                SearchNextPath();
            }
        }
    }
}
