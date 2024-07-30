using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TalkingUICon : MonoBehaviour
{
    public AudioRecorder audioRecorder;
    public TTS tts;
    public GPT gpt;
    public Text instruction;
    public Image Recording;
    public void Update()
    {
        if (!audioRecorder.istalking)
        {
            instruction.text = "Space를 눌러 말을 걸어보세요!";
        }
        if(audioRecorder.istalking && audioRecorder.isRecording)
        {
            Recording.gameObject.SetActive(true);
        }else if(audioRecorder.istalking && !audioRecorder.isRecording)
        {
            instruction.text = "녹음 중...";
            Recording.gameObject.SetActive(false);
        }
    }
}