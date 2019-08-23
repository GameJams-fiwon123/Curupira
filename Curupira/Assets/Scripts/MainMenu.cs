using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button[] options = null;

    
    private AudioSource aud;

    [SerializeField]
    private AudioClip[] audioClips = null;
    private int index = 3;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!aud.isPlaying)
        {
            FindObjectOfType<MusicManager>().EnableChannel();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index--;
            if (index < 0)
            {
                index = options.Length - 1;
            }

            FindObjectOfType<MusicManager>().DisableChannel();
            aud.clip = audioClips[1];
            aud.Play();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index++;

            if (index == options.Length)
            {
                index = 0;
            }

            FindObjectOfType<MusicManager>().DisableChannel();
            aud.clip = audioClips[1];
            aud.Play();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
            switch (index)
            {
                case 0:
                    FindObjectOfType<LevelManager>().ExitGame();
                    break;
                case 1:
                    FindObjectOfType<LevelManager>().LoadInstruction();
                    break;
                case 2:
                    FindObjectOfType<LevelManager>().LoadCredits();
                    break;
                case 3:
                    FindObjectOfType<MusicManager>().DisableChannel();
                    aud.clip = audioClips[0];
                    aud.Play();
                    StartCoroutine(StartGame());
                    break;
            }
        }

        options[index].Select();
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<MusicManager>().EnableChannel();
        FindObjectOfType<LevelManager>().LoadNextLevel();
    }
}
