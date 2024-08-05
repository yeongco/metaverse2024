using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Net;
using System;
using System.Text;
public class TTS : MonoBehaviour
{
    public bool isReadyToTTS = false;
    public GPT gpt;
    public AudioRecorder audioRecorder;
    Coroutine a;
    public void Start()
    {
        TextToSpeech("안녕하세요");
    }
    public void Update()
    {
        if (isReadyToTTS)
        {
            TextToSpeech(gpt.result);
        }
    }
    public void TextToSpeech(string sentence)
    {
        Debug.Log("대입 문장 : " + sentence);
        AudioSource audio = GetComponent<AudioSource>();

        // API 발급하고 받은 client_id, client_secret 작성
        string client_id = "jgw5cfcbb4";
        string client_secret = "m3XpLu0sLXzlO2DA1IcCevQQtbefRqITJVukm2kd";

        string url = "https://naveropenapi.apigw.ntruss.com/tts-premium/v1/tts";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Headers.Add("X-NCP-APIGW-API-KEY-ID", client_id);
        request.Headers.Add("X-NCP-APIGW-API-KEY", client_secret);
        request.Method = "POST";

        // 목소리 이름 지정 
        string avatar_name = PlayerCanSee.instance.closestObject.gameObject.name;
        byte[] byteDataParams = Encoding.UTF8.GetBytes($"speaker={avatar_name}&volume=0&speed=0&pitch=0&format=wav&text={sentence}");

        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = byteDataParams.Length;

        // TTS 요청
        Stream st = request.GetRequestStream();
        st.Write(byteDataParams, 0, byteDataParams.Length);
        st.Close();
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        // stream 으로 요청 값 받음 
        Stream input = response.GetResponseStream();

        // stream to byte
        var memoryStream = new MemoryStream();
        input.CopyTo(memoryStream);
        byte[] byteArray = memoryStream.ToArray();
        float[] f = ConvertByteToFloat(byteArray);

        // byte to audioclip 
        using (Stream s = new MemoryStream(byteArray))
        {
            AudioClip audioClip = AudioClip.Create("tts123", f.Length, 1, 24000, false);
            audioClip.SetData(f, 0);
            audio.clip = audioClip;
            audio.Play();
            TalkingUICon.instance.isTTS = false;
            TalkingUICon.instance.istalking = false;
            TalkingUICon.instance.isWaiting = true;
            isReadyToTTS = false;
            a = null;
            //if (a == null) StartCoroutine(SetisTTS());
        }
    }
    public IEnumerator SetisTTS()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        TalkingUICon.instance.isTTS = false;
        TalkingUICon.instance.istalking = false;
        TalkingUICon.instance.isWaiting = true;
        isReadyToTTS = false;
        a = null;
    }
    private float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 2];
        for (int i = 0; i < floatArr.Length; i++)
        {
            floatArr[i] = BitConverter.ToInt16(array, i * 2) / 32768.0f;
        }
        return floatArr;
    }
}