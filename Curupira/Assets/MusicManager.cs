using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource canal3;

    public MusicManager instance;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableChannel()
    {
        canal3.volume = 1;
    }

    public void DisableChannel()
    {
        canal3.volume = 0;
    }
}
