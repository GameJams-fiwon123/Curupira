using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assobio : MonoBehaviour
{

    private Animator anim;
    private bool canUse = true;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canUse)
        {
            anim.SetBool("canUse", true);
            canUse = false;
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
