using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RecommandDialogue : MonoBehaviour
{
    public RecommendNPC npc;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogues;
    [SerializeField] private List<string> text;
    private string apiKey;


    private bool isDialogue = false;
    private bool isEscapeKeyActivated = false;
    private int currentCounter = 0; //현재 대사 인덱스
    private WaitForSeconds typingTime = new WaitForSeconds(0.05f); //한 글자씩 타이핑 되는 속도
    private void OnEnable()
    {
        apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        StartCoroutine(SendRequestToGPT());
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
            npc.enabled = true;
            this.enabled = false;
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
    public IEnumerator SendRequestToGPT()
    {
        string apiUrl = "https://api.openai.com/v1/chat/completions";
        var requestData = new
        {
            model = "gpt-4-turbo", // Use GPT-4 Turbo model
            messages = new[]
            {
                new { role = "system", content = $"{GameManager.Instance.talkings}의 발언들을 바탕으로 사용자의 성격을 분석하여 그 인격에 맞는 최적의 명언 한개를 줘." +
                $" 다른말없이 명언만 제시해줘, 만약에 대화가 없다면, 아무명언이나 줘, 한국어로 번역해서 줘" },
                new { role = "user", content = "" }
            },
            max_tokens = 100,
            temperature = 1
        };

        string jsonData = JsonConvert.SerializeObject(requestData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // API 키가 null이거나 빈 문자열인지 확인
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API key is null or empty.");
            yield break;
        }

        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var jsonResponse = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<OpenAIResponse>(jsonResponse);
            Debug.Log("GPT 결과 : " + response.choices[0].message.content.Trim());
            GameManager.Instance.result = response.choices[0].message.content.Trim();
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
