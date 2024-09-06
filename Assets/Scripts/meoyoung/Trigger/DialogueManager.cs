using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogues;
    [SerializeField] private List<string> text;
    [SerializeField] private GameObject image;

    [HideInInspector] public bool isActivated = false;
    private bool isDialogue = false;
    private bool isEscapeKeyActivated = false;
    private int currentCounter = 0; //현재 대사 인덱스
    private WaitForSeconds typingTime = new WaitForSeconds(0.05f); //한 글자씩 타이핑 되는 속도

    // 호출하면 대화 UI에 정해진 대사 출력

    private void Update()
    {
        if (isActivated)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isDialogue)
                {
                    ActiveDialogue();
                }
            }
        }

        if(isEscapeKeyActivated)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                image.SetActive(false);
                isEscapeKeyActivated = false;
                isActivated = true;
                ActiveDialogue();
            }
        }
    }
    public void ActiveDialogue()
    {
        if(currentCounter == 7)
        {
            dialoguePanel.SetActive(false);
            isActivated = false;
            isEscapeKeyActivated = true;
            image.SetActive(true);
            currentCounter++;
            return;
        }

        if (text.Count > currentCounter)
        {
            StartCoroutine(ActiveDialogueCoroutine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            this.enabled = false;
            FadeController.Instance.StartFadeIn();
            isActivated = false;
            StartCoroutine(StartChangeScene());
        }
    }

    IEnumerator StartChangeScene()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Main");
    }

    // 키보드로 메세지 보여주는 연출
    private IEnumerator ActiveDialogueCoroutine()
    {
        if(currentCounter == 4)
        {
            isActivated = false;
            FadeController.Instance.StartFadeOut();
            yield return new WaitForSeconds(2.0f);
            isActivated= true;
        }
        isDialogue = true;
        dialoguePanel.SetActive(true);
        dialogues.text = ""; // 텍스트 초기화

        foreach (char letter in text[currentCounter])
        {
            if (Input.GetKey(KeyCode.Space))
            {
                dialogues.text = text[currentCounter];
                break;
            }
            dialogues.text += letter;
            yield return typingTime;
        }
        currentCounter++;
        isDialogue = false;
    }
}
