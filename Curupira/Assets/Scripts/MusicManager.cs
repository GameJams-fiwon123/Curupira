using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource canal3 = null;

    [SerializeField]
    private AudioSource canal1 = null;

    public static MusicManager instance = null;

    [SerializeField] private AudioClip[] musics = null;

    [SerializeField] private AudioClip historia = null;
    [SerializeField] private AudioClip final = null;


    [SerializeField] private AudioClip mainMenu = null;

    // Start is called before the first frame update
    private void Awake()
    {
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public void EnableChannel()
    {
        canal1.volume = 1;
    }

    public void DisableChannel()
    {
        canal1.volume = 0;
    }

    public void EnableChanne3()
    {
        canal3.volume = 1;
    }

    public void DisableChannel3()
    {
        canal3.volume = 0;
    }

    public void PlayRandomMusic()
    {
        int index = Random.Range(0, 3);

        switch (index)
        {
            case 0:
                PlayMusic1();
                break;
            case 1:
                PlayMusic2();
                break;
            case 2:
                PlayMusic3();
                break;
        }
    }

    public void PlayMusic1()
    {
        DisableChannel();
        canal3.clip = musics[0];
        canal3.Play();
    }

    public void PlayMusic2()
    {
        DisableChannel();
        canal3.clip = musics[1];
        canal3.Play();
    }

    public void PlayMusic3()
    {
        DisableChannel();
        canal3.clip = musics[2];
        canal3.Play();
    }

    public void PlayStory()
    {
        DisableChannel();
        if (canal3.clip != historia)
        {
            canal3.clip = historia;
            canal3.Play();
        }
    }

    public void PlayFinal()
    {
        DisableChannel();
        if (canal3.clip != final)
        {
            canal3.clip = final;
            canal3.Play();
        }
    }

    public void PlayMainMenu()
    {
        if (!canal1.isPlaying)
            canal1.Play();

        EnableChannel();

        if (canal3.clip != mainMenu)
        {
            canal3.clip = mainMenu;
            canal3.Play();
        }
    }
}
