using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class RecommendNPC : MonoBehaviour
{
    public RecommandDialogue2 recommandDialogue2;
    [Tooltip("모든 NPC 넣어주면 돼요")]
    [SerializeField] List<GameObject> npc;

    [Tooltip("Fade In 연출 이후 몇 초 후에 목소리를 재생할 건지")]
    [SerializeField] float waitSeconds = 1.0f;
    // 유나 : 0
    // 김씨 : 1
    // 춘자 : 2
    // 철수 : 3

    private void Start()
    {
        Debug.Log(GameManager.Instance.maxPerson);
        Recommend(GameManager.Instance.maxPerson, "안녕? 도란도의 생활은 재밌었어? 너와 함께라서 행복했어. 너도 늘 하늘하늘한 기분과 함께 행복하면 좋겠네. 고마워.");
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            recommandDialogue2.enabled = true;
        }
    }
    public void Recommend(int num, string text) // 해당하는 npc를 활성화하고, 받은 text를 tts로 재생함
    {
        if (num >= 0 && num <= npc.Count - 1)
        {
            StartCoroutine(ShowNPCandPlayTTS(num, text));
        }
        else
        {
            Debug.Log("NPC index가 범위를 초과했습니다.");
        }
    }

    IEnumerator ShowNPCandPlayTTS(int num, string text)
    {
        FadeController.Instance.JustFade();
        FadeController.Instance.StartFadeOut();
        npc[num].SetActive(true);
        yield return new WaitForSeconds(waitSeconds);
        PlayTTS(num, text);
    }

    public void PlayTTS(int num, string sentence)
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
        string avatar_name = npc[num].name;
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
        }
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
