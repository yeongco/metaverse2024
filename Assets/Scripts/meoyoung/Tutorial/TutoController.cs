using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoController : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    void Start()
    {
        FadeController.Instance.JustFade();
        StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(1.0f);
        dialogueManager.isActivated = true;
        dialogueManager.ActiveDialogue();
    }

}
