﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    Color32 colorSelect = new Color32(226, 243, 228, 255);
    Color32 colorUnselect = new Color32(70, 135, 143, 255);

    [SerializeField]
    TextMeshProUGUI txtButtonPrevious = null;

    [SerializeField]
    TextMeshProUGUI txtButtonNext = null;

    [SerializeField]
    Image img = null;

    private AudioSource aud;

    [SerializeField]
    Sprite[] rules = null;
    int index = 0;

    bool isNext = true;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    private void Start()
    {
        MusicManager.instance.PlayMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<LevelManager>().LoadMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isNext)
            {
                index++;

                if (index == rules.Length - 1)
                {
                    txtButtonNext.text = "Finish";
                }
                else if (index >= rules.Length)
                {
                    index = rules.Length - 1;
                    FindObjectOfType<LevelManager>().LoadMainMenu();
                }

                txtButtonPrevious.enabled = true;
            }
            else
            {
                index--;

                if (index <= 0)
                {
                    index = 0;
                    txtButtonPrevious.enabled = false;
                    isNext = true;
                    txtButtonNext.color = colorSelect;
                    txtButtonPrevious.color = colorUnselect;
                }

                txtButtonNext.text = "Next";
            }

            img.sprite = rules[index];
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!isNext)
            {
                isNext = true;
                txtButtonNext.color = colorSelect;
                txtButtonPrevious.color = colorUnselect;
                aud.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (index > 0)
            {
                if (isNext)
                {
                    isNext = false;
                    txtButtonPrevious.color = colorSelect;
                    txtButtonNext.color = colorUnselect;
                    aud.Play();
                }
            }
        }
    }
}
