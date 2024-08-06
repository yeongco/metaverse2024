using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class GPT : MonoBehaviour
{
    public string result;
    public bool isReadyToGetGPT = false;
    public ClovaSpeechRecognizer clovaSpeechRecognizer;
    public TTS tts;
    public Text instruction;
    private string selectedPersonality;
    private string apiKey;
    public STTS_save save;
    public PlayerCanSee playerCanSee;
    private Dictionary<string, string> personalities = new Dictionary<string, string>()
    {
        {"nyuna", "인물설정 : 이름은 유나, 해요체를 쓰며, 바닷가 근처 카페 개업이라는 꿈을 품고 도시 직장 생활을 정리 해 이곳으로 온 30대 여성. 고민을 잘 들어주며 감성적인 말을 잘한다./성격:INFJ, 조용한, 낭만을 향유, 애늙은이/좋아하는 것:정적인, 독서, 낭만, 재즈, 풍경화 그리기/싫어하는 것:경쟁, 정신없는 것"},
        {"ndain", "인물설정 : 이름은 크리스티나, 하지만 본명은 '춘자'이다. 본인의 본명을 거론하는 것을 굉장히 싫어하며, 아이돌이 되어 유명해지는 것이 꿈이다. 촌스러운 것을 무시하지만, 정작 본인의 이름이 가장 촌스럽다. 신도시를 동경하지만, 한 번도 섬을 나가본 적이 없다. 상대방의 근황에 관심이 많으며, 지적인 것을 좋아하지만, 본인은 지적이지 않다. 착한 성격이며, 허세가 있고, 허당이다. 친근한 반말을 쓴다."},
        {"nsangdo", "인물설정 : 이름은 딱히 알수 없다. 성이 김씨란 것만 알뿐. 해변가에서 일광욕을 즐긴다.백수 같지만 파도로부터 사람들을 지키는 **자칭** 안전요원이라고 한다. 고민이 있으면 나약한 마음을 갖지 말라며 강인한 마음을 갖게 해준다.ESTP, 자유를 추구하나 남들이 자신을 필요로 하길 바란다. 농담과 내기를 즐긴다. 우울과는 거리가 멀다. 아저씨같은 말투를 쓴다"},
        {"nminsang", "인물설정 : 이름은 김혜성, 사진 촬영을 좋아하며 천문학에 관심이 많은 사람이다. 별이 잘 보이는 곳을 찾기 위해 섬을 돌아다닌다. 상식이나 과학 지식을 물어보면 좋아한다. 은하, 위성, 항성, 행성 등 우주를 비유로 표현하는 것을 좋아한다. 칼 세이건의 '코스모스'를 좋아하며, INTJ이고, 기록을 좋아하며 논리적인 것을 좋아한다.'합니다체'를 주로 쓴다"}
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

        Debug.Log(selectedPersonality);
    }

    public void Update()
    {
        if (isReadyToGetGPT)
        {
            SelectPersonality(playerCanSee.closestObject.gameObject.name);
            isReadyToGetGPT = false;
            StartCoroutine(SendRequestToGPT(clovaSpeechRecognizer.getResult));
        }
    }

    private void SelectPersonality(string a)
    {
        if (a.Contains("nyuna"))
        {
            selectedPersonality = personalities["nyuna"];
        }
        else if (a.Contains("ndain"))
        {
            selectedPersonality = personalities["ndain"];
        }
        else if (a.Contains("nsangdo"))
        {
            selectedPersonality = personalities["nsangdo"];
        }
        else if (a.Contains("nminsang"))
        {
            selectedPersonality = personalities["nminsang"];
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
                new { role = "system", content = $"너는 {selectedPersonality}의 인물 설정에 의한 성격과 말투로 말하는 사람이야. 한국어로 20단어를 넘기지 말아줘" },
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
            result = response.choices[0].message.content.Trim();
            tts.isReadyToTTS = true;
            TalkingUICon.instance.isGenerating = false;
            TalkingUICon.instance.isTTS = true;
            instruction.text = result;
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
