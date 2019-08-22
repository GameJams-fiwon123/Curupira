using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    Sprite[] options;

    private Image img;
    private int index = 2;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            img.enabled = !img.enabled;
        }

        if (img.enabled)
        {

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                index--;
                if (index < 0)
                {
                    index = options.Length - 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                index++;

                if (index == options.Length)
                {
                    index = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (index)
                {
                    case 0:
                        FindObjectOfType<LevelManager>().LoadMainMenu();
                        break;
                    case 1:
                        FindObjectOfType<LevelManager>().LoadAgain();
                        break;
                    case 2:
                        img.enabled = false;
                        break;
                }
            }
        }

        img.sprite = options[index];
    }
}
