using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTutorial : MonoBehaviour
{
    [Tooltip("튜토리얼을 위해 지정할 초기 스폰 장소")]
    [SerializeField] Transform startPoint;

    [Tooltip("스폰 시킬 대상(플레이어)")]
    [SerializeField] Transform player;

    [Tooltip("튜토리얼 대사")]
    [SerializeField] List<string> text;

    [Tooltip("대사를 출력할 창")]
    [SerializeField] private GameObject dialoguePanel;

    [Tooltip("대사를 출력할 Text 오브젝트")]
    [SerializeField] private Text dialogues;

    [Tooltip("잠시 중지할 NPC 탐지 기능")]
    [SerializeField] PlayerCanSee pcs;

    [Tooltip("잠시 중지할 플레이어 움직임 기능")]
    [SerializeField] PlayerMove pm;

    private int currentCounter = 0; //현재 대사 인덱스
    private bool isActivated = false;
    private bool isDialogue = false;
    private WaitForSeconds typingTime = new WaitForSeconds(0.05f);
    public PlayerCamera playercamera;
    public GameObject Kim;


    private void Start()
    {
        pcs.enabled = false;
        pm.enabled = false;
        FadeController.Instance.StartFadeOut();
        player.position = startPoint.position;
        StartCoroutine(ActiveDialogueCoroutine());
    }

    private void Update()
    {
        if (isActivated)
        {
            PlayerCamera.state = 3;

            StartCoroutine(playercamera.ViewTalk(Kim, 5.0f));
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isDialogue)
                {
                    ActiveDialogue();
                }
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
            PlayerCamera.state = 2;
            dialoguePanel.SetActive(false);
            this.enabled = false;
            isActivated = false;
            pcs.enabled = true;
            pm.enabled = true;
        }
    }


    private IEnumerator ActiveDialogueCoroutine()
    {
        if(currentCounter == 0)
        {
            yield return new WaitForSeconds(2.0f);
            isActivated = true;
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
