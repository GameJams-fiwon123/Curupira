using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private float waitTime = 0.1f;

    private TextMeshProUGUI txt;
    private Animator anim;

    [SerializeField]
    private string[] dialogues = null;
    int index = -1;

    // Start is called before the first frame update
    void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
    }

    IEnumerator StartDialogue()
    {
        char[] charDialogue = dialogues[index].ToCharArray();

        for (int i = 0; i < charDialogue.Length; i++)
        {
            txt.text += charDialogue[i];
            yield return new WaitForSeconds(waitTime);
        }

        anim.speed = 1;
    }

    public void NextDialogue()
    {
        index++;
        if (index < dialogues.Length)
        {
            anim.speed = 0;
            txt.text = "";
            StartCoroutine(StartDialogue());
        }
        else
        {
            FindObjectOfType<LevelManager>().LoadNextLevel();
        }
    }
}
