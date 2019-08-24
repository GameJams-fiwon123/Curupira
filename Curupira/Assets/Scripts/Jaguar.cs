using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jaguar : MonoBehaviour
{
    private Animator anim;
    private Collider2D col2D;
    private SpriteRenderer spr;

    [Header("Settings Path")]
    [SerializeField] Transform paths = null;
    int index = -1;
    private Vector3 currentPathPosition;
    private Vector3 motion = Vector3.zero;
    [SerializeField] private bool IsMove = true;
    [SerializeField] private bool IsLoop = true;

    private bool isDie = false;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();
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
            if (IsLoop)
            {
                index = 0;
            }
            else
            {
                IsMove = false;
                motion = Vector3.zero;
            }
        }
        else
        {
            currentPathPosition = paths.GetChild(index).position;
        }

    }

    private void Update()
    {
        if (!GameManager.instance.IsPaused() && IsMove && !isDie)
        {
            Move();
        }

        ProcessAnimation();
    }

    void Move()
    {
        Vector3 currentPosition = Vector3.zero;

        currentPosition = currentPathPosition;

        motion = currentPosition - transform.position;
        motion = motion.normalized;

        motion.x = motion.x / 50;
        motion.y = motion.y / 50;

        transform.position = transform.position + motion;
        float distance = Vector3.Distance(transform.position, currentPosition);

        if (distance < 0.1f)
        {
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
                spr.flipX = true;
                anim.SetInteger("Horizontal", 1);
            }
            else if (motion.x < 0)
            {
                spr.flipX = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            anim.SetBool("IsDie", true);
            col2D.enabled = false;
            Invoke("Die", 0.5f);
            isDie = true;
        }
    }

    private void Die()
    {
        GameManager.instance.GameOver();
    }

    public void EnableMove()
    {
        IsMove = true;
        index = -1;
        SearchNextPath();
    }
}
