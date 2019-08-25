using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{

    private void Start()
    {
        MusicManager.instance.PlayStory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            FindObjectOfType<LevelManager>().LoadNextLevel();
        }
    }
}
