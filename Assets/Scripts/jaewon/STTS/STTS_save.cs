using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
public class STTS_save : MonoBehaviour
{

    public string currentString;
    public bool stringGenerated = false;
    public string dialoge;
    private string apiKey;
    public Text diary;
    public GameObject STTS;
    public GameObject STTSUI;
    public PlayerMove playerMove;
    public PlayerCanSee playerCanSee;
    public TalkingUICon talkingUICon;
    public GameObject player;
    private string diaries = null;
    private void Start()
    {
        apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        // API 키가 설정되지 않은 경우 에러 메시지 출력
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API key is not set in environment variables.");
            return;
        }
        // 'E' Ű�� ������ ���� ����
        if (!stringGenerated)
        {
            // ���ο� ���ڿ� ����
            currentString = GenerateNewString();
            Debug.Log("New string generated: " + currentString);
            stringGenerated = true;
        }
        else
        {
            // ���ڿ��� ���Ͽ� �߰�
            //AppendStringToFile(currentString);
            stringGenerated = false;
            dialoge = currentString;
            StartCoroutine(SendRequestToGPT(dialoge));
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("녹화종료");
            Debug.Log("String appended: " + currentString);
            AppendStringToFile(currentString);

            playerMove.enabled = true;
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponentInChildren<Animator>().SetBool("IsWalk", true);

            playerCanSee.closestObject.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
            playerCanSee.closestObject.GetComponent<NPCController>().enabled = true;
            playerCanSee.closestObject.GetComponent<NPCController>().CurrentState = playerCanSee.closestObject.GetComponent<NPCWalkState>();
            playerCanSee.closestObject.GetComponent<NPCController>().anim.SetBool("LootAt", false);
            playerCanSee.closestObject = null;
            talkingUICon.isWaiting = true;
            StartCoroutine(SendRequestToGPT(dialoge));
        }
    }
    // ���ο� ���ڿ��� �����ϴ� �޼ҵ�
    string GenerateNewString()
    {
        // ���÷� GUID�� ����Ͽ� ���ڿ� ����
        return System.Guid.NewGuid().ToString();
    }

    // ���ڿ��� ���Ͽ� �߰��ϴ� �޼ҵ�
    void AppendStringToFile(string str)
    {
        // ���� ���� ��� ����
        string path = Path.Combine(Application.persistentDataPath, "savedStrings.txt");

        // ���ڿ��� ���Ͽ� �߰�
        File.AppendAllText(path, str + "\n");

        Debug.Log("String appended to: " + path);
    }
    void AppendDiaryToFile(string str)
    {
        // ���� ���� ��� ����
        string path = Path.Combine(Application.persistentDataPath, "savedDiaries.txt");

        // ���ڿ��� ���Ͽ� �߰�
        File.AppendAllText(path, str + "\n");

        Debug.Log("String appended to: " + path);
    }

    //
    public IEnumerator SendRequestToGPT(string prompt)
    {
        string apiUrl = "https://api.openai.com/v1/chat/completions";
        var requestData = new
        {
            model = "gpt-4-turbo", // Use GPT-4 Turbo model
            messages = new[]
            {
                new { role = "system", content = $"{prompt}의 대화를 유추해서 한국어로 50~70단어의 일기로 작성해줘. 일기가 충분치 않으면 그냥 '일기 없음' 으로 표기해줘" },
                new { role = "user", content = prompt }
            },
            max_tokens = 2000,
            temperature = 1
        };

        string jsonData = JsonConvert.SerializeObject(requestData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var jsonResponse = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<OpenAIResponse>(jsonResponse);
            Debug.Log("GPT diary : " + response.choices[0].message.content.Trim());
            diary.text = response.choices[0].message.content.Trim();
            AppendDiaryToFile(response.choices[0].message.content.Trim());
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
        STTS.gameObject.SetActive(false);
        STTSUI.gameObject.SetActive(false);
    }

    [Serializable]
    private class OpenAIResponse
    {
        public Choice[] choices;
    }

    [Serializable]
    private class Choice
    {
        public Message message;
    }

    [Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

}
