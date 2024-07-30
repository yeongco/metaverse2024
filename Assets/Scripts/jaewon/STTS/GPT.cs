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
    public STTS_save save;
    private Dictionary<string, string> personalities = new Dictionary<string, string>()
    {
        {"ironman", "You are Tony Stark, a.k.a Iron Man. You are a genius, billionaire, playboy, and philanthropist. You have a witty and sarcastic tone."},
        {"friendly", "You are a friendly and helpful assistant, always ready to provide support with a warm and cheerful demeanor."},
        {"professional", "You are a professional advisor, providing clear and concise information in a formal tone."}
    };
    private void Start()
    {
        SelectPersonality("ironman");
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
        if (a.Contains("ironman"))
        {
            selectedPersonality = personalities["ironman"];
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
                new { role = "system", content = $"say {selectedPersonality},Number of letters do not over 200 in korean" },
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
            Debug.Log("대화 내용 = "+save.currentString);
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
