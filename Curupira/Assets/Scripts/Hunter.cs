using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [SerializeField] Transform target = null;

    [SerializeField] GameObject prefabBullet = null;

    private Rigidbody2D rb2D;
    private Animator anim;
    private SpriteRenderer spr;
    private Vector3 motion = Vector3.zero;

    [Header("Settings Path")]
    [SerializeField] Transform paths = null;

    int index = -1;
    private Vector3 currentPathPosition;

    private List<Vector3> footprintPositions = new List<Vector3>();

    Coroutine firing = null;
    private bool isCatch = false;


    private float rateTimeCollison = 0.5f;
    private float waitTimeCollision = 0f;

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
        if (!GameManager.instance.IsPaused())
        {
            if (waitTimeCollision < rateTimeCollison)
                waitTimeCollision += Time.deltaTime;

            if (firing == null)
            {
                Move();
                ProcessAnimation();
            }

            See();
            Fire();
        }
    }

    void Move()
    {
        Vector3 currentPosition = Vector3.zero;

        if (footprintPositions.Count > 0)
        {
            currentPosition = footprintPositions[0];

        }
        else
        {
            currentPosition = currentPathPosition;
        }

        motion = currentPosition - transform.position;
        motion = motion.normalized;

        motion.x = motion.x / 50;
        motion.y = motion.y / 50;

        transform.position = transform.position + motion;
        float distance = Vector3.Distance(transform.position, currentPosition);

        if (distance < 0.1f)
        {
            if (footprintPositions.Count > 0)
            {
                footprintPositions.RemoveAt(0);
            }

            transform.position = currentPosition;
            SearchNextPath();
        }
    }

    private void ProcessAnimation()
    {
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
        int layerMask = (1 << 10) | (1 << 11) | (1 << 17) | (1 << 18);

        // Cast a ray straight down.
        RaycastHit2D[] hits = new RaycastHit2D[4];
        Vector2 dir = Vector2.zero;

        hits[0] = Physics2D.Raycast(transform.position, Vector2.up, 2, layerMask, 0, 0);
        hits[1] = Physics2D.Raycast(transform.position, Vector2.right, 2, layerMask, 0, 0);
        hits[2] = Physics2D.Raycast(transform.position, Vector2.down, 2,layerMask, 0, 0);
        hits[3] = Physics2D.Raycast(transform.position, Vector2.left, 2,layerMask, 0, 0);

        isCatch = false;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null)
            {
                // Calculate the distance from the surface and the "error" relative
                // to the floating height.
                float distance = Vector2.Distance(hits[i].point, transform.position);
                Debug.DrawRay(transform.position, motion * 100, Color.yellow);

                if (hits[i].collider.tag == "Player" || hits[i].collider.tag == "Jaguar" || hits[i].collider.tag == "Ararajuba")
                {
                    isCatch = true;
                    target = hits[i].collider.transform;
                }
            } 
        }
    }

    void Fire()
    {
        if (isCatch)
        {
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
            target = null;
        }
    }

    IEnumerator StartFire()
    {
        while (true)
        {
            GameObject instance = Instantiate(prefabBullet, transform.position, Quaternion.identity);

            Vector2 dir = target.position - transform.position;
            instance.GetComponent<Bullet>().SetDirection(dir);

            yield return new WaitForSeconds(1f);
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Footprint")
        {
            footprintPositions.Add(collision.transform.position);
            footprintPositions.Add(collision.GetComponent<Footprint>().NextPosition);
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Pathing")
        {
            if (paths.gameObject != collision.gameObject)
            {
                paths = collision.gameObject.transform;
                index = -1;
            }
        }
        
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (waitTimeCollision > rateTimeCollison)
        {
            SearchNextPath();
            waitTimeCollision = 0f;
        }
    }
}
