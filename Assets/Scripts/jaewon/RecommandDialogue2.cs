using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RecommandDialogue2 : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogues;
    [SerializeField] private List<string> text;
    public GameObject Panel;
    public ending end;
    private bool isDialogue = false;
    private bool isEscapeKeyActivated = false;
    private int currentCounter = 0; //현재 대사 인덱스
    private WaitForSeconds typingTime = new WaitForSeconds(0.05f); //한 글자씩 타이핑 되는 속도
    private void OnEnable()
    {
        dialoguePanel.SetActive(true);
        dialogues.gameObject.SetActive(true);
        dialogues.text = "";
        text.Add("가끔 휴식이 필요하면 언제든지 도란도를 찾아와줘");
        text.Add("우린 너를 기다릴게");
        text.Add("우리 이야기는 끝나지 않았어");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E클릭");
            if (!isDialogue)
            {
                ActiveDialogue();
            }
        }
    }
    public void ActiveDialogue()
    {
        if (text.Count > currentCounter)
        {
            StartCoroutine(ActiveDialogueCoroutine());
        }
        else
        {
            dialogues.gameObject.SetActive(false);
            dialoguePanel.SetActive(false);
            Panel.gameObject.SetActive(true);
            this.enabled = false;
            end.enabled = true;
        }
    }


    // 키보드로 메세지 보여주는 연출
    private IEnumerator ActiveDialogueCoroutine()
    {
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
