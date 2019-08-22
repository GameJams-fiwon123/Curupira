using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button[] options;

    private int index = 3;

    // Update is called once per frame
    void Update()
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
                    FindObjectOfType<LevelManager>().LoadNextLevel();
                    break;
                case 1:
                    FindObjectOfType<LevelManager>().LoadCredits();
                    break;
                case 2:
                    FindObjectOfType<LevelManager>().LoadInstruction();
                    break;
                case 3:
                    FindObjectOfType<LevelManager>().ExitGame();
                    break;
            }
        }

        options[index].Select();
    }
}
