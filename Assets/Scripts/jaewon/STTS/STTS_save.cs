using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using Unity.VisualScripting;

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
    public CreateDiary createDiary;
    public Text instruction;
    public GameObject player;
    private void OnEnable()
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
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("녹화종료");
            playerMove.moveAvailable = true;
            player.GetComponent<CharacterController>().enabled = true;
            //player.GetComponentInChildren<Animator>().SetBool("IsWalk", true);

            if (TalkingUICon.instance.talkingNPC.name == "nsangdo")
            {
                playerCanSee.closestObject.GetComponent<KimController>().enabled = true;
                playerCanSee.closestObject.GetComponent<KimController>().CurrentState = playerCanSee.closestObject.GetComponent<KimIdleState>();
                //playerCanSee.closestObject.GetComponent<KimController>().anim.SetBool("LootAt", false);
            }
            else if (TalkingUICon.instance.talkingNPC.name == "nyuna")
            {
                playerCanSee.closestObject.GetComponent<YunaController>().enabled = true;
                playerCanSee.closestObject.GetComponent<YunaController>().CurrentState = playerCanSee.closestObject.GetComponent<YunaIdleState>();
                playerCanSee.closestObject.GetComponent<YunaController>().anim.SetBool("LootAt", false);
            }
            else
            {
                playerCanSee.closestObject.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
                playerCanSee.closestObject.GetComponent<NPCController>().enabled = true;
                playerCanSee.closestObject.GetComponent<NPCController>().CurrentState = playerCanSee.closestObject.GetComponent<NPCWalkState>();
                playerCanSee.closestObject.GetComponent<NPCController>().anim.SetBool("LootAt", false);
            }

            playerCanSee.closestObject = null;
            talkingUICon.isWaiting = true;
            AppendStringToFile(currentString);
            STTSUI.gameObject.SetActive(false);
            STTS.gameObject.SetActive(false);
            createDiary.enabled = true;
            instruction.text = "Space를 눌러 말을 걸어보세요!";
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
        Debug.Log("프롬포트: " + prompt);

        string apiUrl = "https://api.openai.com/v1/chat/completions";
        var requestData = new
        {
            model = "gpt-4-turbo", // Use GPT-4 Turbo model
            messages = new[]
            {
                new { role = "system", content = $"{prompt}의 요약해서 일기로 작성해줘.'/'를 기준으로 나눴을때, 홀수번째 내용이 내가 한 말이고, 짝수번째 내용이 '대화 상대방'이 한 말이야. 본인일기처럼 작성해줘. 정보가 부족하면 '대화 내용이 부족합니다'라고 출력해주고" },
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
            AppendDiaryToFile(response.choices[0].message.content.Trim());
            createDiary.enabled = false;
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
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
