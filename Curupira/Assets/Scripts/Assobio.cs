using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assobio : MonoBehaviour
{
    private AudioSource aud;
    private Animator anim;
    private bool canUse = true;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canUse)
        {
            anim.SetBool("canUse", true);
            aud.Play();
            canUse = false;
        }

        if (!GameManager.instance.IsPaused())
        {
            anim.speed = 1;
        }
        else
        {
            anim.speed = 0;
        }
    } 

    public void EnableUse()
    {
        canUse = true;
    }

    public void DisableUse()
    {
        anim.SetBool("canUse", false);
    }
}
