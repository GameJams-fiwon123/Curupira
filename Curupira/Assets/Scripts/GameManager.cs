using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        FindObjectOfType<LevelManager>().LoadGameOver();
    }

    public void Win()
    {
        FindObjectOfType<LevelManager>().LoadNextLevel();
    }
        
}
