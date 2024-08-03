using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TalkingUICon : MonoBehaviour
{
    public static TalkingUICon instance;
    public AudioRecorder audioRecorder;
    GameObject talkingNPC;
    NPCController npcController;
    public TTS tts;
    public GPT gpt;
    public Text instruction;
    public Image Recording;
    public bool isWaiting = true;
    public bool istalking = false;
    public bool isRecording = false;
    public bool isGenerating = false;
    public bool isTTS = false;
    //istalking = 사용자가 space를 누르고 AI가 답변하기까지의 상태, 한 사이클 전체를 말함
    //isWaiting = 사이클이 끝났으며, 사용자가 대화를 시작하길 기다리는 단계
    //isRecording = 녹화중인 단계
    //isGenerating = 답변을 생성하는 단계
    //isTTS = 음성을 내뱉는 단계, 3초후면 자동으로 꺼진다.
    private void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(this);
        }
        else
        {
        instance = this;
        }
    }
    private void OnEnable()
    {
        talkingNPC = PlayerCanSee.instance.closestObject;
        npcController = talkingNPC.GetComponent<NPCController>();
    }
    public void Update()
    {
        if (!istalking && isWaiting)
        {
            isWaiting = false;
            npcController.ChangeState(npcController._lootatState);
        }

        if (istalking && isRecording)
        {
            instruction.text = "녹음 중...";
            Debug.Log("녹음 중...");
            npcController.ChangeState(npcController._nodState);
            Recording.gameObject.SetActive(true);
        }

        if(istalking && isGenerating)
        {
            Debug.Log("답변 생성 중...");
            instruction.text = "답변 생성 중...";
            npcController.ChangeState(npcController._thinkState);
            Recording.gameObject.SetActive(false);
        }
        if (istalking && isTTS)
        {
            Recording.gameObject.SetActive(false);
            npcController.ChangeState(npcController._goodState);
        }
    }
}