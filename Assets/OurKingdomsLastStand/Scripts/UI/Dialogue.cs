using UnityEngine;
using TMPro;
using System.Collections;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    public float textSpeed;

    private int index;

    void Start()
    {
        dialogueText.text = string.Empty;
        StartDialogue();
    }
    public void NextDialogue()
    {
         if(dialogueText.text == dialogueLines[index])
         {
             NextLine();
         }
         else
         {
             StopAllCoroutines();
             dialogueText.text = dialogueLines[index];
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            GameManager.instance.TutorialComplete();
        }
    }
}
