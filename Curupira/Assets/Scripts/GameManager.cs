using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool isPaused = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void GameOver()
    {
        FindObjectOfType<LevelManager>().LoadAgain();
    }

    public void Win()
    {
        FindObjectOfType<LevelManager>().LoadNextLevel();
    }
        
    public void SetPaused(bool flag)
    {
        isPaused = flag;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
