using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GPT : MonoBehaviour
{
    public string result;
    public bool isReadyToGetGPT = false;
    public ClovaSpeechRecognizer clovaSpeechRecognizer;
    public TTS tts;
    private string selectedPersonality;
    private string apiKey;
    public STTS_save save;
    public PlayerCanSee playerCanSee;
    private Dictionary<string, string> personalities = new Dictionary<string, string>()
    {
        {"yuna", "이름은 유나, 바닷가 근처 카페 개업이라는 꿈을 품고 도시 직장 생활을 정리 해 이곳으로 온 30대 여성. 고민을 잘 들어주며 감성적인 말을 잘한다./성격:INFJ, 조용한, 낭만을 향유, 애늙은이/좋아하는 것:정적인, 독서, 낭만, 재즈, 풍경화 그리기/싫어하는 것:경쟁, 정신없는 것"},
        {"friendly", "You are a friendly and helpful assistant, always ready to provide support with a warm and cheerful demeanor."},
        {"professional", "You are a professional advisor, providing clear and concise information in a formal tone."}
    };

    private void Start()
    {
        // 환경 변수에서 API 키를 읽어옴
        apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        // API 키가 설정되지 않은 경우 에러 메시지 출력
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API key is not set in environment variables.");
            return;
        }

        SelectPersonality(playerCanSee.closestObject.gameObject.name);
        Debug.Log(selectedPersonality);
    }

    public void Update()
    {
        if (isReadyToGetGPT)
        {
            isReadyToGetGPT = false;
            StartCoroutine(SendRequestToGPT(clovaSpeechRecognizer.getResult));
        }
    }

    private void SelectPersonality(string a)
    {
        if (a.Contains("yuna"))
        {
            selectedPersonality = personalities["yuna"];
        }
        else if (a.Contains("friendly"))
        {
            selectedPersonality = personalities["friendly"];
        }
        else
        {
            selectedPersonality = personalities["professional"];
        }
    }

    public IEnumerator SendRequestToGPT(string prompt)
    {
        string apiUrl = "https://api.openai.com/v1/chat/completions";
        var requestData = new
        {
            model = "gpt-4-turbo", // Use GPT-4 Turbo model
            messages = new[]
            {
                new { role = "system", content = $"{selectedPersonality}의 추론된 성격과 말투로 말해줘, 한국어로 20단어를 넘기지 말아줘" },
                new { role = "user", content = prompt }
            },
            max_tokens = 500,
            temperature = 0.8
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
            tts.isReadyToTTS = true;
            result = response.choices[0].message.content.Trim();
            save.currentString += result + "/";
            Debug.Log("대화 내용 = " + save.currentString);
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
