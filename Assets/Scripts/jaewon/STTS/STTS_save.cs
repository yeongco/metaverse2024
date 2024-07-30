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
    public Text diary;
    void Update()
    {
        // 'E' Ű�� ������ ���� ����
        if (Input.GetKeyDown(KeyCode.E))
        {
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
                AppendStringToFile(currentString);
                Debug.Log("String appended: " + currentString);
                stringGenerated = false;
                dialoge = currentString;
                StartCoroutine(SendRequestToGPT(dialoge));
            }
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
    //
    public IEnumerator SendRequestToGPT(string prompt)
    {
        string apiUrl = "https://api.openai.com/v1/chat/completions";
        var requestData = new
        {
            model = "gpt-4-turbo", // Use GPT-4 Turbo model
            messages = new[]
            {
                new { role = "system", content = $"Analyze the conversation in {prompt} and write it in the form of a diary in korean." },
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
