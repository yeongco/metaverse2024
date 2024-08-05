using UnityEngine;
using System.Collections;

public class AudioRecorder : MonoBehaviour
{
    private AudioClip audioClip;
    private string microphone;
    public ClovaSpeechRecognizer clovaSpeechRecognizer;
    public STTS_save save;
    
    void Start()
    {
        microphone = Microphone.devices[0];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !TalkingUICon.instance.isRecording)
        {
            TalkingUICon.instance.istalking = true;
            TalkingUICon.instance.isRecording = true;
            StartRecording();
            Debug.Log("≥Ï»≠ Ω√¿€");
        }
        if (Input.GetKeyUp(KeyCode.Space) && TalkingUICon.instance.isRecording)
        {
            StopRecording();
            Debug.Log("≥Ï»≠ ¡æ∑·");
            clovaSpeechRecognizer.SendAudioClip(Application.persistentDataPath + "/audio.wav");
            TalkingUICon.instance.isRecording = false;
            TalkingUICon.instance.isGenerating = true;
        }
    }
    public void StartRecording()
    {
        audioClip = Microphone.Start(microphone, false, 10, 44100);
    }

    public void StopRecording()
    {
        Microphone.End(microphone);
        SaveClip(audioClip);
    }

    private void SaveClip(AudioClip clip)
    {
        string filePath = Application.persistentDataPath + "/audio.wav";
        SavWav.Save(filePath, clip);
        Debug.Log("≥Ï»≠ ºº¿Ã∫Í øœ∑·");
    }
}
