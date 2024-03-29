﻿using System.Collections;
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
        

        if (GameManager.instance.IsStarted()  && !GameManager.instance.IsPaused())
        {
            anim.speed = 1;

            if (Input.GetKeyDown(KeyCode.Z) && canUse)
            {
                canUse = false;
                FindObjectOfType<Hunter>().TakeAssobio();

                anim.SetBool("canUse", true);
                aud.Play();
            }
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
