using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //private AudioSource aud;

    //private void Awake()
    //{
    //    aud = GetComponent<AudioSource>();
    //}

    public void StartGame()
    {
        GameManager.instance.SetStarted(true);
    }
}
