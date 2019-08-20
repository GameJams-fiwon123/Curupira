using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [SerializeField] Transform follow = null;

    [Header("Settings Hunter")]
    [SerializeField] float speed = 100;

    private Rigidbody2D rb2D;
    private Vector3 motion = Vector3.zero;

    [Header("Settings Path")]
    [SerializeField] Transform paths = null;

    int index = -1;
    private Vector3 currentPathPosition;

    public float floatHeight;     // Desired floating height.
    public float liftForce;       // Force to apply when lifting the rigidbody.
    public float damping;         // Force reduction proportional to speed (reduces bouncing).


    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
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

            if (transform.position == currentPathPosition)
            {
                SearchNextPath();
            }
        }
        else
        {
            motion = follow.position - transform.position;
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

            if(hit.collider.name == "Player")
                follow = hit.collider.transform;
            else
            {
                follow = null;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Footprint")
        {
            currentPathPosition = collision.GetComponent<Footprint>().NextPosition;
        }
    }
}
