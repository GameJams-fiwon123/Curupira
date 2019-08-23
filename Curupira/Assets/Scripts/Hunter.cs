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
        if (firing == null)
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
        int layerMask = (1 << 10) | (1 << 11) | (1 << 17) | (1 << 18);

        // Cast a ray straight down.
        RaycastHit2D[] hits = new RaycastHit2D[4];
        Vector2 dir = Vector2.zero;

        hits[0] = Physics2D.Raycast(transform.position, Vector2.up, 2, layerMask, 0, 0);
        hits[1] = Physics2D.Raycast(transform.position, Vector2.right, 2, layerMask, 0, 0);
        hits[2] = Physics2D.Raycast(transform.position, Vector2.down, 2,layerMask, 0, 0);
        hits[3] = Physics2D.Raycast(transform.position, Vector2.left, 2,layerMask, 0, 0);

        bool isCatch = false;

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
                    follow = hits[i].collider.transform;
                }
            } 
        }

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
            follow = null;
        }
    }

    IEnumerator StartFire()
    {
        while (true)
        {
            GameObject instance = Instantiate(prefabBullet, transform.position, Quaternion.identity);

            Vector2 dir = follow.position - transform.position;
            instance.GetComponent<Bullet>().SetDirection(dir);

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
